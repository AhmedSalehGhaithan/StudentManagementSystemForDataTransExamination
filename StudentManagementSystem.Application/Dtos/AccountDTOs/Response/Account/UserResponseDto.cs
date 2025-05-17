namespace StudentManagementSystem.Application.Dtos.AccountDTOs.Response.Account;

public record UserResponseDto(
string Id,
string UserName,
string Email,
string? Name,
IEnumerable<string> Roles);
