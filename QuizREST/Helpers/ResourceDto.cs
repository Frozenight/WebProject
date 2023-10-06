using System.Collections.Generic;

namespace QuizREST.Helpers;

public record ResourceDto<T>(T resource, IReadOnlyCollection<LinksDto> Links);
