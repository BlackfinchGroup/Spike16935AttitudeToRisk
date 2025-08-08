using AttitudeQuestions.Application.Questions.Queries.Contracts;
using AttitudeQuestions.Application.Questions.Queries.Interfaces;
using AttitudeQuestions.Domain;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AttitudeQuestions.Application.Questions.Queries;

public static class GetQuestionEndpoint
{
    public static async Task<IResult> Handle([FromRoute]Guid id, IMediator mediator)
    {
        var command = new GetQuestionSetQuery()
        {
            QuestionSetId = id,
        };

        var result = await mediator.Send(command);

        return result.MatchResult(success => TypedResults.Ok(success));
    }
}

public class GetQuestionSetQueryHandler(IQuestionSetQueryService queryService, ILogger<GetQuestionSetQueryHandler> logger)
    : IRequestHandler<GetQuestionSetQuery, ErrorOr<QuestionSetResponse>>
{
    public async Task<ErrorOr<QuestionSetResponse>> Handle(GetQuestionSetQuery request, CancellationToken cancellationToken)
    {
        QuestionSet? questionSet = await queryService.Get(request.QuestionSetId, cancellationToken);

        if (questionSet is null)
        {
            logger.LogWarning("Question set with Id {QuestionSetId} not found.", request.QuestionSetId);
            return Error.NotFound("QuestionSet.NotFound", "The requested question set does not exist.");
        }

        return new QuestionSetResponse
        {
            Id = questionSet.Id,
            Title = questionSet.Title,
            SubTitle = questionSet.SubTitle,
            NextQuestionSetId = questionSet.NextQuestionSetId,
            Questions = questionSet.Questions.Select(q => new QuestionResponse()
            {
                Id = q.QuestionId,
                Text = q.Text,
                Weighting = q.Weighting
            }).ToList()
        };
    }
}

public class GetQuestionSetQuery : IRequest<ErrorOr<QuestionSetResponse>>
{
    public Guid QuestionSetId { get; set; }
}