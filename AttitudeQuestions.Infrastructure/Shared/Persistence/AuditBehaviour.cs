using AttitudeQuestions.Application.Shared;
using AttitudeQuestions.Domain.Shared;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AttitudeQuestions.Infrastructure.Shared.Persistence;

public class AuditBehavior<TRequest, TResponse>(QuestionDbContext context, ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr<CommandResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var result = await next(cancellationToken);

        if (result.IsError)
            return result;

        try
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is not AggregateRoot<Guid> domainEntity) continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Property("ModifiedAt").CurrentValue = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        return (dynamic)Error.Unexpected("General.Persistance", "a delete request has been received.");
                }
            }

            await context.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "An Error occurred while saving the data.");
            return (dynamic)Error.Unexpected("General.Persistance", "unable to complete your request please try again.");
        }
    }
}
