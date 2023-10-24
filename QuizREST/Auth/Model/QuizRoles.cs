using System.Collections.Generic;

namespace QuizREST.Auth.Model;

public class QuizRoles
{
    public const string Admin = nameof(Admin);
    public const string QuizUser = nameof(QuizUser);

    public static readonly IReadOnlyCollection<string> all = new[] { Admin, QuizUser };
}
