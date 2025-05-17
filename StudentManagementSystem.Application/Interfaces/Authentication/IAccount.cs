using StudentManagementSystem.Application.Dtos.AccountDTOs.Request.Account;
using StudentManagementSystem.Application.Dtos.AccountDTOs.Response;
using StudentManagementSystem.Application.Dtos.AccountDTOs.Response.Account;

namespace StudentManagementSystem.Application.Interfaces.Authentication;

public interface IAccount
{
    Task<Response> CreateAccountAsync(CreateAccountDTO model);
    Task<LoginResponse> LoginAccountAsync(LoginDTO model);
    Task<Response> CreateRoleAsync(CreateRoleDTO model);
    Task<Response> DeleteUserAsync(string email);
    Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model);
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
}

public record class Response(bool Flag = false, string Message = null!);
