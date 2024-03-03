using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace TasksService.Api;

internal static class ControllerExtensions
{
    public static Guid GetUserId(this ControllerBase httpContext)
    {
        var sidClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
        return Guid.Parse(sidClaim!.Value);
    }
}