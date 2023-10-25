using QuizREST.Auth.Model;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace QuizREST.Auth
{
    public class RecourseOwnerAuthorizationHandler : AuthorizationHandler<RecourseOwnerRequirment, IUserOwnedRecourse>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RecourseOwnerRequirment requirement, IUserOwnedRecourse resource)
        {
            if (context.User.IsInRole(QuizRoles.Admin) || context.User.FindFirstValue(JwtRegisteredClaimNames.Sub) == resource.UserId)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

    public record RecourseOwnerRequirment : IAuthorizationRequirement;
}
