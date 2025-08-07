namespace AttitudeQuestions.Domain;

public class Question
{
    public Guid Id { get; set; }
    public required string Text { get; set; }
    public int Weighting { get; set; }
}
