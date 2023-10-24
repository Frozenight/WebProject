using System.ComponentModel.DataAnnotations;

namespace QuizREST.Auth.Model;

public record RegisterUserDto([Required] string UserName, [EmailAddress][Required] string email, [Required] string Password);

public record LoginDto(string UserName, string Password);

public record UserDto(string id, string UserName, string Email);

public record SuccessfulLoginDto(string accessToken);
