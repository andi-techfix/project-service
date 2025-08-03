namespace Domain.Entities;

public class Chart
{
    public string Symbol { get; set; } = string.Empty;
    public string Timeframe { get; set; } = string.Empty;
    public IList<Indicator> Indicators { get; set; } = new List<Indicator>();
}