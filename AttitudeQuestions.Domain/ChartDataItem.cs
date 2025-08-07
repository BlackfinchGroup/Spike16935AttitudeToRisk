namespace AttitudeQuestions.Domain;

public class ChartDataItem
{
    public required string Label { get; set; }
    public int Ordinal { get; set; }
    public int Value { get; set; }
}