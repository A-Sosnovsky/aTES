using System;
using Microsoft.AspNetCore.Identity;

namespace JwtRoleAuthentication.Models;

public class ApplicationUser : IdentityUser
{
    public Guid PublicId { get; set; }
}