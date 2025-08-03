using Application.Queries.GetMostPopularIndicatorsQuery;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Moq;

namespace UnitTests;

public class GetPopularIndicatorsQueryHandlerTests
{
    private readonly Mock<IUserApiClient> _userApiClientMock = new();
    private readonly Mock<IProjectRepository> _projectRepositoryMock = new();
    private readonly GetPopularIndicatorsQueryHandler _handler;

    public GetPopularIndicatorsQueryHandlerTests()
    {
        _handler = new GetPopularIndicatorsQueryHandler(
            _projectRepositoryMock.Object,
            _userApiClientMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnEmptyList_When_NoUserIds()
    {
        // Arrange
        const string subscriptionType = "premium";
        _userApiClientMock
            .Setup(u => u.GetUserIdsBySubscriptionTypeAsync(subscriptionType))
            .ReturnsAsync([]);

        var query = new GetPopularIndicatorsQuery(subscriptionType);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
        _userApiClientMock.Verify(u => u.GetUserIdsBySubscriptionTypeAsync(subscriptionType), Times.Once);
        _projectRepositoryMock.Verify(r => r.GetProjectsByUserIdsAsync(It.IsAny<IEnumerable<int>>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnTopThreeIndicatorsOrderedByCountThenName_When_ProjectsExist()
    {
        // Arrange
        const string subscriptionType = "basic";
        var userIds = new List<int> { 1, 2 };
        _userApiClientMock
            .Setup(u => u.GetUserIdsBySubscriptionTypeAsync(subscriptionType))
            .ReturnsAsync(userIds);
        
        var projects = new List<Project>
        {
            new()
            {
                Charts =
                [
                    new Chart
                    {
                        Indicators = new List<Indicator>
                        {
                            new() { Name = "MA" },
                            new() { Name = "RSI" },
                            new() { Name = "BB" },
                        }
                    },

                    new Chart
                    {
                        Indicators = new List<Indicator>
                        {
                            new() { Name = "MA" },
                            new() { Name = "BB" },
                        }
                    }
                ]
            },
            new()
            {
                Charts =
                [
                    new Chart
                    {
                        Indicators = new List<Indicator>
                        {
                            new() { Name = "MA" },
                            new() { Name = "Ichimoku" },
                        }
                    }
                ]
            }
        };

        _projectRepositoryMock
            .Setup(r => r.GetProjectsByUserIdsAsync(userIds))
            .ReturnsAsync(projects);

        var query = new GetPopularIndicatorsQuery(subscriptionType);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("MA");
        result[0].UsedCount.Should().Be(3);
        result[1].Name.Should().Be("BB");
        result[1].UsedCount.Should().Be(2);
        result[2].Name.Should().Be("Ichimoku");
        result[2].UsedCount.Should().Be(1);

        _userApiClientMock.Verify(u => u.GetUserIdsBySubscriptionTypeAsync(subscriptionType), Times.Once);
        _projectRepositoryMock.Verify(r => r.GetProjectsByUserIdsAsync(userIds), Times.Once);
    }
}