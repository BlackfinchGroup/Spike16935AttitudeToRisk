using AttitudeQuestions.Application.Shared;
using AttitudeQuestions.Domain;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AttitudeQuestions.Application.Questions.Commands;

public class AnswerQuestion
{
    public class AnswerQuestionRequest
    {
        public Guid QuestionId { get; set; }
        public int Weighting { get; set; }
    }

    public static async Task<IResult> Handle([FromRoute] Guid id, [FromBody] AnswerQuestionRequest request, IMediator mediator)
    {
        var command = new AnswerQuestionCommand()
        {
            SessionId = id,
            QuestionId = request.QuestionId,
            Weighting = request.Weighting
        };

        var result = await mediator.Send(command);

        return result.MatchResult(success => TypedResults.Created());
    }
}

public class AnswerQuestionCommandHandler(IRepository<QuestionSetSession> repository, ILogger<AnswerQuestionCommandHandler> logger)
    : IRequestHandler<AnswerQuestionCommand, ErrorOr<CommandResponse>>
{
    public async Task<ErrorOr<CommandResponse>> Handle(AnswerQuestionCommand request, CancellationToken cancellationToken)
    {
        var session = await repository.Get(request.SessionId, cancellationToken);

        if (session is null)
        {
            logger.LogWarning("Session with Id {SessionId} not found.", request.SessionId);
            return Error.NotFound("Session.NotFound", "The requested session does not exist.");
        }

        var answer = new QuestionAnswer(request.QuestionId, request.Weighting);
        session.Answers.Add(answer);

        return CommandResponse.Default;
    }
}

public class AnswerQuestionCommand : IRequest<ErrorOr<CommandResponse>>
{
    public Guid SessionId { get; set; }
    public Guid QuestionId { get; set; }
    public int Weighting { get; set; }
}