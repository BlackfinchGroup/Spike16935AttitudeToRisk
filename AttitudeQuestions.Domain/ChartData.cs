namespace AttitudeQuestions.Domain;

public class ChartData
{
    public required string Title { get; set; }
    public required List<ChartDataItem> Data { get; set; }
}
