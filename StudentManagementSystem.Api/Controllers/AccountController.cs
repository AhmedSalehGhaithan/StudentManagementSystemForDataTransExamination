using StudentManagementSystem.Application.Dtos.AccountDTOs.Request.Account;
using StudentManagementSystem.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.Dtos.AccountDTOs.Response;
using Microsoft.AspNetCore.Authorization;
using StudentManagementSystem.Application.Dtos.AccountDTOs.Response.Account;

namespace StudentManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccount _account) : ControllerBase
    {
        [HttpPost("identity/create")]
        public async Task<ActionResult<Response>> CreateAccount(CreateAccountDTO model)
        {
            if (!ModelState.IsValid) 
                return BadRequest("Model can not be null");
            return Ok(await _account.CreateAccountAsync(model));
        }

        [HttpPost("identity/login")]
        public async Task<ActionResult<LoginResponse>> LoginAccount(LoginDTO model)
        {
            if (!ModelState.IsValid) 
                return BadRequest("Model can not be null");
            return Ok(await _account.LoginAccountAsync(model));
        }

        [HttpDelete("identity/delete/account/{email}")]
        public async Task<ActionResult<Response>> DeleteAccount(string email)
        {
            if (!ModelState.IsValid) 
                return BadRequest("Email can not be null");
            return Ok(await _account.DeleteUserAsync(email));
        }

        [HttpPost("identity/refresh-token")]
        public async Task<ActionResult<LoginResponse>> RefreshToken(RefreshTokenDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model can not be null");
            return Ok(await _account.RefreshTokenAsync(model));
        }

        [HttpGet("identity/users")]
        [Authorize(Roles = "Admin,teacher")] // role based
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers()
        {
            return Ok(await _account.GetAllUsersAsync());
        }
        [HttpPost("identity/create/role")]
        public async Task<ActionResult<Response>> CreateRole(CreateRoleDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model can not be null");
            return Ok(await _account.CreateRoleAsync(model));
        }

    }
}