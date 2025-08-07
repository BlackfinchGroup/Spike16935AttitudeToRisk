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

    public static async Task<IResult> Handle([FromRoute]Guid id, [FromBody] StartQuestionsRequest request, IMediator mediator)
    {
        var command = new StartQuestionSetCommand()
        {
            QuestionSetId = id,
            SessionsId = request.SessionId
        };

        var result = await mediator.Send(command);

        return result.MatchResult(success => TypedResults.NoContent());
    }
}

public class StartQuestionSetCommandHandler(IRepository<QuestionSetSession> repository, ILogger<StartQuestionSetCommandHandler> logger)
    : IRequestHandler<StartQuestionSetCommand, ErrorOr<CommandResponse>>
{
    public async Task<ErrorOr<CommandResponse>> Handle(StartQuestionSetCommand request, CancellationToken cancellationToken)
    {
        await repository.Create(
            new QuestionSetSession(request.SessionsId, request.QuestionSetId), cancellationToken);

        return CommandResponse.Default;
    }
}

public class StartQuestionSetCommand : IRequest<ErrorOr<CommandResponse>>
{
    public Guid QuestionSetId { get; set; }
    public Guid SessionsId { get; set; }
}