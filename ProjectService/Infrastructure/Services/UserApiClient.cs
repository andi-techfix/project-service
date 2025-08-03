using Application.Services;
using FluentRest;

namespace Infrastructure.Services;

public class UserApiClient(HttpClient httpClient) : IUserApiClient
{
    public async Task<List<int>> GetUserIdsBySubscriptionTypeAsync(string subscriptionType)
    {
        var response = await httpClient.GetAsync<List<int>>(b => b
            .AppendPath("api")
            .AppendPath("users")
            .AppendPath("bySubscription")
            .AppendPath(subscriptionType));

        return response ?? [];
    }
}