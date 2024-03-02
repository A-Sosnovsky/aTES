using System;

namespace JwtRoleAuthentication.Models;

public class AuthResponse
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
    public Guid PublicId { get; set; }
}