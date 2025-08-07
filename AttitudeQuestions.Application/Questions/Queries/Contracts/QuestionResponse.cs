namespace AttitudeQuestions.Application.Questions.Queries.Contracts;

public class QuestionResponse
{
    public Guid Id { get; init; }
    public required string Text { get; init; }
    public int Weighting { get; init; }
}