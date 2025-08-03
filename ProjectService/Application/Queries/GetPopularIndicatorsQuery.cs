using MediatR;

namespace Application.Queries
{
    public record GetPopularIndicatorsQuery(string SubscriptionType) : IRequest<IList<IndicatorResult>>;
    
    public record IndicatorResult(string Name, int Used);
}