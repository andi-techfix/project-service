using Application.Commands.Models;
using Application.Services;
using Domain.Repositories;
using MediatR;

namespace Application.Queries.GetMostPopularIndicatorsQuery;

public class GetPopularIndicatorsQueryHandler(IProjectRepository projectRepository, IUserApiClient userApiClient)
    : IRequestHandler<GetPopularIndicatorsQuery, List<IndicatorResult>>
{
    public async Task<List<IndicatorResult>> Handle(GetPopularIndicatorsQuery request,
        CancellationToken cancellationToken)
    {
        var userIds = await userApiClient.GetUserIdsBySubscriptionTypeAsync(request.SubscriptionType);
        
        if (userIds.Count == 0) return [];
        
        var projects = await projectRepository.GetProjectsByUserIdsAsync(userIds);
        var indicatorCounts = projects
            .SelectMany(p => p.Charts)
            .SelectMany(c => c.Indicators)
                .GroupBy(i => i.Name)
            .Select(g => new IndicatorResult(g.Key, g.Count()))
                .OrderByDescending(r => r.UsedCount)
                .ThenBy(r => r.Name)
            .Take(3)
            .ToList();
        return indicatorCounts;
    }
}