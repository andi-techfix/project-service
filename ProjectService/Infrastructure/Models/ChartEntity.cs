namespace Infrastructure.Models;

public class ChartEntity
{
    public string Symbol { get; init; } = string.Empty;
    public string Timeframe { get; init; } = string.Empty;
    public List<IndicatorEntity> Indicators { get; init; } = [];
}