using Microsoft.AspNetCore.Identity;

namespace StudentManagementSystem.Domain.Core.AuthenticationEntities;

public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
}

public class RefreshToken
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string? Token { get; set; }
}