using Application.Commands.Models;
using MediatR;

namespace Application.Queries.GetMostPopularIndicatorsQuery;

public record GetPopularIndicatorsQuery(string SubscriptionType) : IRequest<List<IndicatorResult>>;