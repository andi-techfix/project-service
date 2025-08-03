namespace Application.Services;

public interface IUserApiClient
{
    Task<IList<int>> GetUserIdsBySubscriptionTypeAsync(string subscriptionType);
}