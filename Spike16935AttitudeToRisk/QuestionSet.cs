namespace Spike16935AttitudeToRisk;

internal class QuestionSet
{
    public Guid Id { get; set; }
    public required List<Question> Questions { get; set; }
    public required string Title { get; internal set; }
    public Guid? NextQuestionSetId { get; set; }
}

internal class Question
{
    public Guid Id { get; set; }
    public required string Text { get; set; }
    public int Weighting { get; set; }
}
