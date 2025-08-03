namespace Application.Services;

public interface IUserApiClient
{
    Task<List<int>> GetUserIdsBySubscriptionTypeAsync(string subscriptionType);
}