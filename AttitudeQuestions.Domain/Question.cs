namespace AttitudeQuestions.Domain;

public class Question(Guid id, string text, int weighting)
{
    public Guid Id { get; private set; } = id;
    public string Text { get; private set; } = text;
    public int Weighting { get; private set; } = weighting;
}
