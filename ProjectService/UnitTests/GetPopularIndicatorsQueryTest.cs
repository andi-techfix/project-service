using Application.Queries.GetMostPopularIndicatorsQuery;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Moq;

namespace UnitTests;

public class GetPopularIndicatorsQueryHandlerTests
{
    private const string PremiumSubscription = "premium";
    private const string BasicSubscription   = "basic";

    private static readonly List<int> EmptyUserIds   = [];
    private static readonly List<int> SampleUserIds  = [1, 2];
    
    private static readonly Indicator MA        = new() { Name = "MA" };
    private static readonly Indicator BB        = new() { Name = "BB" };
    private static readonly Indicator RSI       = new() { Name = "RSI" };
    private static readonly Indicator Ichimoku  = new() { Name = "Ichimoku" };
    
    private static readonly List<Project> ProjectsForBasic =
    [
        new()
        {
            Charts =
            [
                new Chart
                {
                    Indicators = new List<Indicator>
                    {
                        MA, RSI, BB
                    }
                },

                new Chart
                {
                    Indicators = new List<Indicator>
                    {
                        MA, BB
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
                        MA, Ichimoku
                    }
                }
            ]
        }
    ];

    private static readonly GetPopularIndicatorsQuery EmptyQuery = new(PremiumSubscription);
    private static readonly GetPopularIndicatorsQuery BasicQuery = new(BasicSubscription);
    
    private readonly Mock<IUserApiClient>     _userApiClientMock = new();
    private readonly Mock<IProjectRepository> _projectRepoMock   = new();
    private readonly GetPopularIndicatorsQueryHandler _handler;

    public GetPopularIndicatorsQueryHandlerTests()
    {
        _handler = new GetPopularIndicatorsQueryHandler(
            _projectRepoMock.Object,
            _userApiClientMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnEmptyList_When_NoUserIds()
    {
        // Arrange
        _userApiClientMock
            .Setup(u => u.GetUserIdsBySubscriptionTypeAsync(PremiumSubscription))
            .ReturnsAsync(EmptyUserIds);

        // Act
        var result = await _handler.Handle(EmptyQuery, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
        _userApiClientMock.Verify(u => u.GetUserIdsBySubscriptionTypeAsync(PremiumSubscription), Times.Once);
        _projectRepoMock.Verify(r => r.GetProjectsByUserIdsAsync(It.IsAny<IEnumerable<int>>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnTopThreeIndicatorsOrderedByCountThenName_When_ProjectsExist()
    {
        // Arrange
        _userApiClientMock
            .Setup(u => u.GetUserIdsBySubscriptionTypeAsync(BasicSubscription))
            .ReturnsAsync(SampleUserIds);

        _projectRepoMock
            .Setup(r => r.GetProjectsByUserIdsAsync(SampleUserIds))
            .ReturnsAsync(ProjectsForBasic);

        // Act
        var result = await _handler.Handle(BasicQuery, CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("MA");
        result[0].UsedCount.Should().Be(3);
        result[1].Name.Should().Be("BB");
        result[1].UsedCount.Should().Be(2);
        result[2].Name.Should().Be("Ichimoku");
        result[2].UsedCount.Should().Be(1);

        _userApiClientMock.Verify(u => u.GetUserIdsBySubscriptionTypeAsync(BasicSubscription), Times.Once);
        _projectRepoMock.Verify(r => r.GetProjectsByUserIdsAsync(SampleUserIds), Times.Once);
    }
}