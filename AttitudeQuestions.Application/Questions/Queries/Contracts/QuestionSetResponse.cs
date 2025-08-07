namespace AttitudeQuestions.Application.Questions.Queries.Contracts;

public class QuestionSetResponse
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public required string SubTitle { get; init; }
    public Guid? NextQuestionSetId { get; init; }
    public required IList<QuestionResponse> Questions { get; init; }
}
