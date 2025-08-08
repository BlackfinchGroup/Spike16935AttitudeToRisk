using AttitudeQuestions.Application.Shared;
using AttitudeQuestions.Domain;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AttitudeQuestions.Application.Questions.Commands;

public static class StartQuestionsEndpoint
{
    public class StartQuestionsRequest
    {
        public Guid SessionId { get; set; }
    }

    public static async Task<IResult> Handle(IMediator mediator)
    {
        var command = new StartQuestionSetCommand();

        var result = await mediator.Send(command);

        return result.MatchResult(success => TypedResults.Created(result.Value.ToString()));
    }
}

public class StartQuestionSetCommandHandler(IRepository<QuestionSetSession> repository, ILogger<StartQuestionSetCommandHandler> logger)
    : IRequestHandler<StartQuestionSetCommand, ErrorOr<CommandResponse>>
{
    public async Task<ErrorOr<CommandResponse>> Handle(StartQuestionSetCommand request, CancellationToken cancellationToken)
    {
        var session = new QuestionSetSession(Guid.NewGuid());

        await repository.Create(session, cancellationToken);

        return CommandResponse.From(session.Id);
    }
}

public class StartQuestionSetCommand : IRequest<ErrorOr<CommandResponse>>
{
    public Guid QuestionSetId { get; set; }
    public Guid SessionId { get; set; }
}