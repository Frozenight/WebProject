using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace QuizREST.Auth
{
    public class JwtTokenService
    {
        private readonly SymmetricSecurityKey _authSigningKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtTokenService(IConfiguration configuration) {
            _authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            _issuer = configuration["JWT:ValidIssuer"];
            _audience = configuration["JWT:ValidAudience"];
        }

        public string CreateAccessToken(string userName)
    }
}
