using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizREST.Auth;
using QuizREST.Auth.Model;
using System.Threading.Tasks;

namespace QuizREST.Controllers;
[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly UserManager<QuizRestUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    public AuthController(UserManager<QuizRestUser> userManager, IJwtTokenService jwtTokenService) 
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
    {
        var user = await _userManager.FindByNameAsync(registerUserDto.UserName);
        if (user != null)
        {
            return BadRequest("Request invalid");
        }

        var newUser = new QuizRestUser { Email = registerUserDto.email, UserName = registerUserDto.UserName };

        var createUserResult = await _userManager.CreateAsync(newUser, registerUserDto.Password);
        if (!createUserResult.Succeeded)
        {
            return BadRequest("Could not create a user");
        }

        await _userManager.AddToRoleAsync(newUser, QuizRoles.QuizUser);

        return CreatedAtAction(nameof(Register), new UserDto(newUser.Id, newUser.UserName, newUser.Email));
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user == null)
        {
            return BadRequest("Username or password invalid");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!isPasswordValid)
        {
            return BadRequest("Username or password invalid");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);

        return Ok(new SuccessfulLoginDto(accessToken));
    }
}
