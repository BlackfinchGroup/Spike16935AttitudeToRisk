using AttitudeQuestions.Application.Questions.Commands;
using AttitudeQuestions.Application.Questions.Queries;
using AttitudeQuestions.Application.Questions.Queries.Contracts;
using AttitudeQuestions.Application.Shared.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AttitudeQuestions.Application;

public static class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder MapQuestionsApi(this RouteGroupBuilder group)
    {
        group.MapPost("/start", StartQuestionsEndpoint.Handle)
            .WithActionName("StartQuestionSession")
            .Produces(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", GetQuestionEndpoint.Handle)
            .WithActionName("GetQuestionSet")
            .Produces<QuestionSetResponse>();

        group.MapPost("/answer/{id:guid}", AnswerQuestion.Handle)
            .WithActionName("AnswerQuestion")
            .Produces(StatusCodes.Status204NoContent);

        group.WithTags("Questions")
            .WithName("Questions");

        return group;
    }
}
