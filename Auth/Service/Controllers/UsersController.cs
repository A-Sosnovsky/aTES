using System;
using System.Linq;
using System.Threading.Tasks;
using Contract.Dto.References;
using JwtRoleAuthentication.Data;
using JwtRoleAuthentication.Events;
using JwtRoleAuthentication.Models;
using JwtRoleAuthentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtRoleAuthentication.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEventsService _eventsService;
    private readonly ApplicationDbContext _context;
    private readonly TokenService _tokenService;

    public UsersController(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
        TokenService tokenService, IEventsService eventsService)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
        _eventsService = eventsService;
    }


    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegistrationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email,
            PublicId = Guid.NewGuid()
        };

        var result = await _userManager.CreateAsync(user, request.Password!);

        if (result.Succeeded)
        {
            var role = Roles.Popug;
            await _userManager.AddToRoleAsync(user, role);
            await _eventsService.UserCreated(user.PublicId, user.UserName!, [role]);
            request.Password = "";
            return CreatedAtAction(nameof(Register), new { email = request.Email, role = Roles.Popug }, request);
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return BadRequest(ModelState);
    }


    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var managedUser = await _userManager.FindByEmailAsync(request.Email!);

        if (managedUser == null)
        {
            return BadRequest("Bad credentials");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password!);

        if (!isPasswordValid)
        {
            return BadRequest("Bad credentials");
        }

        var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);
        if (userInDb is null)
        {
            return Unauthorized();
        }

        var roles = await _userManager.GetRolesAsync(managedUser);

        var accessToken = _tokenService.CreateToken(userInDb, roles);
        await _context.SaveChangesAsync();

        return Ok(new AuthResponse
        {
            PublicId = userInDb.PublicId,
            Username = userInDb.UserName,
            Email = userInDb.Email,
            Token = accessToken,
        });
    }

    [HttpPost("{email}")]
    public async Task<ActionResult> SetUserRole([FromRoute] string email, [FromBody] string[] roles)
    {
        var managedUser = await _userManager.FindByEmailAsync(email);

        if (managedUser == null)
        {
            return NotFound("User not found");
        }

        if (roles.Length == 0)
        {
            return BadRequest("Role");
        }

        var rolesString = roles.Select(r => r.ToString()).ToArray();
        var userRoles = await _userManager.GetRolesAsync(managedUser);

        await _userManager.RemoveFromRolesAsync(managedUser, userRoles);
        await _userManager.AddToRolesAsync(managedUser, rolesString);
        await _eventsService.UserRolesChanged(managedUser.PublicId, rolesString);

        return Ok();
    }
}