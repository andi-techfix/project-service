using Application.Services;
using Domain.Repositories;
using MediatR;

namespace Application.Queries
{
    public class GetPopularIndicatorsQueryHandler(IProjectRepository projectRepository, IUserApiClient userApiClient)
        : IRequestHandler<GetPopularIndicatorsQuery, IList<IndicatorResult>>
    {
        public async Task<IList<IndicatorResult>> Handle(GetPopularIndicatorsQuery request,
            CancellationToken cancellationToken)
        {
            var userIds = await userApiClient.GetUserIdsBySubscriptionTypeAsync(request.SubscriptionType);
            if (!userIds.Any()) return new List<IndicatorResult>();
            var projects = await projectRepository.GetProjectsByUserIdsAsync(userIds);
            var indicatorCounts = projects
                .SelectMany(p => p.Charts)
                .SelectMany(c => c.Indicators)
                .GroupBy(i => i.Name)
                .Select(g => new IndicatorResult(g.Key, g.Count()))
                .OrderByDescending(r => r.Used)
                .ThenBy(r => r.Name)
                .Take(3)
                .ToList();
            return indicatorCounts;
        }
    }
}