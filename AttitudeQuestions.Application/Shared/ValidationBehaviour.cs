using System.ComponentModel.DataAnnotations;
using ErrorOr;
using MediatR;

namespace AttitudeQuestions.Application.Shared;

public class ValidationBehavior<TRequest, TResponse>() : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);
        bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

        if (isValid)
        {
            return await next(cancellationToken);
        }

        var errors = validationResults
            .ConvertAll(error => error.MemberNames.Select(x => Error.Validation(
                code: x,
                description: error.ErrorMessage ?? "Is not Valid")))
            .SelectMany(x => x)
            .ToList();

        return (dynamic)errors;
    }
}
