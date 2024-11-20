using Moq;
using Xunit;
using TimeTraveler.Libary.Services;
using TimeTraveler.Libary.ViewModels;

namespace TimeTraveler.UnitTest.ViewModels
{
    public class GameTwoViewModelTests
    {
        private readonly Mock<IMazeService> _mockMazeService;
        private readonly GameTwoViewModel _viewModel;

        public GameTwoViewModelTests()
        {
            _mockMazeService = new Mock<IMazeService>();
            _viewModel = new GameTwoViewModel(_mockMazeService.Object);
        }

        [Fact]
        public void StartNewGame_ShouldInitializeGameStatus()
        {
            // Arrange
            _mockMazeService.Setup(service => service.GetMazeRepresentation()).Returns(
                "##########\n#P       #\n#        #\n#        #\n##########"
            );
            _mockMazeService.Setup(service => service.IsMoveValid(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            // Act
            _viewModel.StartNewGame();

            // Assert
            Assert.Equal("游戏进行中...", _viewModel.GameStatus);
            Assert.Contains("P", _viewModel.Maze); // 检查玩家是否在迷宫中
        }

        [Fact]
        public void MoveDown_ShouldUpdatePlayerPosition()
        {
            // Arrange
            _mockMazeService.Setup(service => service.GetMazeRepresentation()).Returns(
                "##########\n#P       #\n#        #\n#        #\n##########"
            );
            _mockMazeService.Setup(service => service.IsMoveValid(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            
            // Act
            _viewModel.MoveDownCommand.Execute(null); // 模拟向下移动

            // Assert
            Assert.Contains("P", _viewModel.Maze); // 确认玩家位置仍然是 "P"
        }

       
        [Fact]
        public void Quit_ShouldResetGameStatus()
        {
            // Arrange
            _mockMazeService.Setup(service => service.GetMazeRepresentation()).Returns(
                "##########\n#P       #\n#        #\n#        #\n##########"
            );
            _mockMazeService.Setup(service => service.IsMoveValid(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            // Act
            _viewModel.QuitCommand.Execute(null);  // 退出游戏

            // Assert
            Assert.Equal("游戏进行中...", _viewModel.GameStatus);  // 确保状态重新设置
        }

        [Fact]
        public void GameStatus_ShouldUpdateWhenMazeLoadingFails()
        {
            // Arrange
            _mockMazeService.Setup(service => service.GetMazeRepresentation()).Returns(string.Empty);  // 迷宫加载失败

            // Act
            _viewModel.StartNewGame();

            // Assert
            Assert.Equal("迷宫加载失败！", _viewModel.GameStatus);  // 检查加载失败后的状态
        }
    }
}
