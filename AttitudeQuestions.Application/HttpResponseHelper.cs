using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace AttitudeQuestions.Application;

public static class HttpResponseHelper
{
    public static IResult CreateErrorResponse(List<Error> errors)
    {
        var firstError = errors.First();

        return firstError.Type switch
        {
            ErrorType.NotFound => TypedResults.NotFound(),
            ErrorType.Validation => TypedResults.ValidationProblem(errors.GroupBy(x => x.Code).ToDictionary(x => x.Key, x => x.Select(y => y.Description).ToArray())),
            ErrorType.Conflict => TypedResults.Conflict(firstError.Description),
            _ => TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError, title: "An unexpected error occurred")
        };
    }

    public static IResult MatchResult<TValue>(this ErrorOr<TValue> obj, Func<TValue, IResult> onSuccess) =>
        obj.Match(onSuccess, CreateErrorResponse);
}
