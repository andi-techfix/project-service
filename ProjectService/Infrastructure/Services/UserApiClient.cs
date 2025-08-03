using Application.Services;
using FluentRest;

namespace Infrastructure.Services;

public class UserApiClient(HttpClient httpClient) : IUserApiClient
{
    public async Task<IList<int>> GetUserIdsBySubscriptionTypeAsync(string subscriptionType)
    {
        var response = await httpClient.GetAsync<List<UserDto>>(b => b
            .AppendPath("api")
            .AppendPath("users")
            .AppendPath("bySubscription")
            .AppendPath(subscriptionType));

        return response?.Select(u => u.Id).ToList() ?? [];
    }

    private class UserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int SubscriptionId { get; set; }
    }
}