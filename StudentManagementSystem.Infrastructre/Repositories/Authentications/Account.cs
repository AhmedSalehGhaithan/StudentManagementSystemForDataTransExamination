using StudentManagementSystem.Application.Dtos.AccountDTOs.Request.Account;
using StudentManagementSystem.Application.Dtos.AccountDTOs.Response;
using StudentManagementSystem.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Domain.Core.AuthenticationEntities;
using StudentManagementSystem.Infrastructre.Data;
using StudentManagementSystem.Application.Dtos.AccountDTOs.Response.Account;

namespace StudentManagementSystem.Infrastructre.Repositories.Authentications;

public class Account(RoleManager<IdentityRole> roleManager,
                     UserManager<ApplicationUser> userManager,
                     SignInManager<ApplicationUser> signInManager,
                     IConfiguration config,
                     ApplicationDbContext context) : IAccount
{

    public async Task<Response> CreateAccountAsync(CreateAccountDTO model)
    {
        if (await userManager.FindByEmailAsync(model.EmailAddress) != null)
            return new Response(false, "User already exists");

        var user = new ApplicationUser()
        {
            UserName = model.EmailAddress,
            Email = model.EmailAddress
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return new Response(false, string.Join(", ", result.Errors.Select(e => e.Description)));

        var roleResult = await userManager.AddToRoleAsync(user, model.Role);
        if (!roleResult.Succeeded)
            return new Response(false, string.Join(", ", roleResult.Errors.Select(e => e.Description)));

        return new Response(true, "Account created successfully");
    }

    public async Task<LoginResponse> LoginAccountAsync(LoginDTO model)
    {
        var user = await userManager.FindByEmailAsync(model.EmailAddress);
        if (user is null) return new LoginResponse(false, "User not found");

        var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded) return new LoginResponse(false, "Invalid credentials");

        string jwtToken = await GenerateToken(user);
        string refreshToken = GenerateRefreshToken();

        if (string.IsNullOrEmpty(jwtToken))
            return new LoginResponse(false, "Error generating token");

        var saveResult = await SaveRefreshToken(user.Id, refreshToken);
        if (saveResult.Flag)
            return new LoginResponse(true, "Login successful", jwtToken, refreshToken);

        return new LoginResponse(false, "Error saving refresh token");
    }


    public async Task<Response> CreateRoleAsync(CreateRoleDTO model)
    {
        if (await roleManager.RoleExistsAsync(model.Name!))
            return new Response(false, "Role already exists");

        var result = await roleManager.CreateAsync(new IdentityRole(model.Name!));
        if (!result.Succeeded)
            return new Response(false, string.Join(", ", result.Errors.Select(e => e.Description)));

        return new Response(true, "Role created successfully");
    }

    public async Task<Response> DeleteUserAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return new Response(false, "User not found");

        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return new Response(false, string.Join(", ", result.Errors.Select(e => e.Description)));

        return new Response(true, "User deleted successfully");
    }

   

    public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model)
    {
        var token = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == model.Token);
        if (token is null) return new LoginResponse(false, "Invalid refresh token");

        var user = await userManager.FindByIdAsync(token.UserId!);
        if (user is null) return new LoginResponse(false, "User not found");

        string newToken = await GenerateToken(user);
        string newRefreshToken = GenerateRefreshToken();

        await SaveRefreshToken(user.Id, newRefreshToken);
        return new LoginResponse(true, "Token refreshed", newToken, newRefreshToken);
    }

    private async Task<string> GenerateToken(ApplicationUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, (await userManager.GetRolesAsync(user)).FirstOrDefault()!)
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await userManager.Users.ToListAsync();
        var userDtos = new List<UserResponseDto>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            userDtos.Add(new UserResponseDto(
                user.Id,
                user.UserName!,
                user.Email!,
                user.Name,
                roles
            ));
        }

        return userDtos;
    }

    private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    private async Task<Response> SaveRefreshToken(string userId, string token)
    {
        try
        {
            var existingToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);
            if (existingToken is null)
                context.RefreshTokens.Add(new RefreshToken { UserId = userId, Token = token });
            else
                existingToken.Token = token;

            await context.SaveChangesAsync();
            return new Response(true, "Token saved");
        }
        catch
        {
            return new Response(false, "Error saving token");
        }
    }
}