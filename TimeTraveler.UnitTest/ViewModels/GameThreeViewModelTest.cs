using Moq;
using TimeTraveler.Libary.Services;
using TimeTraveler.Libary.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace TimeTraveler.UnitTest.ViewModels;


public class GameThreeViewModelTests
{
    private readonly Mock<IFlyService> _mockFlyService;
    private readonly GameThreeViewModel _viewModel;


    public GameThreeViewModelTests()
    {
        _mockFlyService = new Mock<IFlyService>();
        _viewModel = new GameThreeViewModel(_mockFlyService.Object);

    }

    [Fact]
    public void Flap_WhenGameIsNotOver_ShouldUpdateBallPosition()
    {
        // Arrange
        _viewModel.ResetGame();
        double initialBallY = _viewModel.BallY;

        // Act
        _viewModel.Flap();
        _viewModel.Update();
        
        
        // Assert
        Assert.NotEqual(initialBallY, _viewModel.BallY); // 确保小球位置发生了变化
    }

    [Fact]
    public void Flap_WhenGameIsOver_ShouldNotUpdateBallPosition()
    {
        // Arrange
        _viewModel.ResetGame();
        _viewModel.GameOver(); // 使游戏结束
        double initialBallY = _viewModel.BallY;

        // Act
        _viewModel.Flap();
        _viewModel.Update();

        // Assert
        Assert.Equal(initialBallY, _viewModel.BallY); // 游戏结束后小球位置应不变
    }
    



}
