namespace Spike16935AttitudeToRisk;

internal class QuestionSet
{
    public Guid Id { get; set; }
    public required List<Question> Questions { get; set; }
    public required string Title { get; internal set; }
    public Guid? NextQuestionSetId { get; set; }
    public ChartData? ChartData { get; set; }
}

internal class Question
{
    public Guid Id { get; set; }
    public required string Text { get; set; }
    public int Weighting { get; set; }
}

internal class ChartData
{
    public required string Title { get; set; }
    public required List<ChartDataItem> Data { get; set; }
}

internal class ChartDataItem
{
    public required string Label { get; set; }
    public int Ordinal { get; set; }
    public int Value { get; set; }
}