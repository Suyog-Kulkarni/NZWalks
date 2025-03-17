using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
// what the difference between ControllerBase and Controller? 
// ControllerBase is a class that is used to create a controller that does not use views. It is used to create web APIs.
// Controller is a class that is used to create a controller that uses views. It is used to create web applications.
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenRepository _tokenRepository;
    public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
    {
        _userManager = userManager;
        _tokenRepository = tokenRepository;
    }
    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO requestDTO)
    {
        var user = new IdentityUser
        {
            UserName = requestDTO.Username,
            Email = requestDTO.Username,
        };

        var result = await _userManager.CreateAsync(user, requestDTO.Password);

        if (result.Succeeded)
        {
            var addRoles = await _userManager.AddToRolesAsync(user, requestDTO.Roles);
            // the diff between AddToRoleAsync and AddToRolesAsync is that AddToRoleAsync is used to add a single role to the user and AddToRolesAsync is used to add multiple roles to the user
            if (addRoles.Succeeded)
            {
                return Ok("User is Registered!");
            }
            else
            {
                return BadRequest(addRoles.Errors);


            }
        }

        return BadRequest(result.Errors);
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginDTO.Username);

        if(user != null)
        {
            var checkPassword = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (checkPassword)
            {
                // get the user role
                var userRole = await _userManager.GetRolesAsync(user);

                if (userRole != null) 
                {
                    var token = _tokenRepository.CreateJwtToken(user, userRole.ToList());
                    var response = new LoginResponseDTO
                    {
                        JwtToken = token
                    };

                    
                    return Ok(response);
                }
            }
        }

        return BadRequest("Email or Password is incorrect");
    }
}

