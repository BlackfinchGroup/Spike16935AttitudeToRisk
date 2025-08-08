namespace AttitudeQuestions.Domain;

public class Question(Guid questionId, string text, int weighting)
{
    public Guid QuestionId { get; private set; } = questionId;
    public string Text { get; private set; } = text;
    public int Weighting { get; private set; } = weighting;
}
