namespace AttitudeQuestions.Domain;

public class QuestionSet
{
    public Guid Id { get; set; }
    public required List<Question> Questions { get; set; }
    public required string Title { get; set; }
    public Guid? NextQuestionSetId { get; set; }
    public ChartData? ChartData { get; set; }
}
