using System.Windows.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Libary.ViewModels;

public partial class GameThreeViewModel :ViewModelBase
{
        private bool _isGameWon = false; // 游戏是否胜利
        private bool _isGameOver = false; // 游戏是否结束（碰撞或胜利）
        private  Flyer _ball;
        private Obstacle _obstacle1;
        private Obstacle _obstacle2;
        private Obstacle _obstacle3;
        private Obstacle _obstacle4;
        private readonly IFlyService _flyService;
        private DispatcherTimer _timer;
        private const double FrameDuration = 16; // 每帧16ms，即60FPS
        private const double SecondsPerFrame = FrameDuration / 1000.0; // 每帧的秒数
        private double _timeElapsedInSeconds = 0.0; // 游戏已进行的时间（秒）
        private const double GameDurationInSeconds = 10.0; // 游戏持续时长（秒）

        
        
        // 硬编码的窗口宽高
        private const double WindowWidth = 1200;
        private const double WindowHeight = 800;
        
        
        public bool IsGameWon
        {
            get => _isGameWon;
            set
            {
                if (_isGameWon != value)
                {
                    _isGameWon = value;
                    OnPropertyChanged(nameof(IsGameWon));
                }
            }
        }
        
        public bool IsGameOver
        {
            get => _isGameOver;
            set
            {
                if (_isGameOver != value)
                {
                    _isGameOver = value;
                    OnPropertyChanged(nameof(IsGameOver)); // 用于显示/隐藏游戏结束按钮
                }
            }
        }
        

        public double ObstacleX1 => _obstacle1.X;
        public double ObstacleY1 => _obstacle1.Y;
        public double ObstacleWidth1 => _obstacle1.Width;
        public double ObstacleHeight1 => _obstacle1.Height;
        
        public double ObstacleX2 => _obstacle2.X;
        public double ObstacleY2 => _obstacle2.Y;
        public double ObstacleWidth2 => _obstacle2.Width;
        public double ObstacleHeight2 => _obstacle2.Height;
        
        public double ObstacleX3 => _obstacle3.X;
        public double ObstacleY3 => _obstacle3.Y;
        public double ObstacleWidth3 => _obstacle3.Width;
        public double ObstacleHeight3 => _obstacle3.Height;
        
        public double ObstacleX4 => _obstacle4.X;
        public double ObstacleY4 => _obstacle4.Y;
        public double ObstacleWidth4 => _obstacle4.Width;
        public double ObstacleHeight4 => _obstacle4.Height;
        

        public double BallX => _ball.X;
        public double BallY => _ball.Y;
        public double BallWidth => _ball.Width;
        public double BallHeight => _ball.Height;
        
        // 重置游戏状态
        private void ResetGame()
        {
            _ball = new Flyer(100, 100, 100, 100); // 初始化小球
            _obstacle1 = new Obstacle(1200, 500, 100, 300);
            _obstacle2 = new Obstacle(1200, 600, 100, 200);
            _obstacle3 = new Obstacle(1200, 650, 150, 150);
            _obstacle4 = new Obstacle(1200, 0, 1000, 42);

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16) // 60FPS
            };
            _timer.Tick += (sender, e) => TimerTick();
            _timer.Start();

            _isGameWon = false;
            _isGameOver = false;
            _timeElapsedInSeconds = 0;
            IsGameOver = false;  // 通过命令将 IsGameOver 设置为 false，隐藏 Next 按钮
            OnPropertyChanged(nameof(IsGameOver));  // 通知 UI 更新
        }
        

        
        public ICommand FlapCommand { get; }
        public ICommand RestartCommand { get; }
        
        public GameThreeViewModel(IFlyService flyService)
        {
            _flyService = flyService;
            
                _ball = new Flyer(100, 100, 100, 100); // 初始化小球
                _obstacle1 = new Obstacle(1200, 500, 100, 300);
                _obstacle2 = new Obstacle(1200, 600, 100, 200);
                _obstacle3 = new Obstacle(1200, 650, 150, 150);
                _obstacle4 = new Obstacle(1200, 0, 1000, 42);

                _timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(16) // 60FPS
                };
                _timer.Tick += (sender, e) => TimerTick();
                _timer.Start();

                _isGameWon = false;
                _isGameOver = false;
                _timeElapsedInSeconds = 0;
            

        // 使用 RelayCommand 实现 FlapCommand
            FlapCommand = new RelayCommand(Flap);
            RestartCommand = new RelayCommand(ResetGame);
        }

        public void Update()
        {
            if (_isGameWon || _isGameOver)
            {
                return; // 游戏结束，停止更新
            }
            
            
            _ball.UpdatePosition();
            OnPropertyChanged(nameof(BallX));  // 更新小球X位置
            OnPropertyChanged(nameof(BallY));  // 更新小球Y位置
            
            
            
            
            _obstacle1.UpdatePosition3(3);  // 每次更新障碍物向左移动5像素
            OnPropertyChanged(nameof(ObstacleX1)); // 更新障碍物的X位置
            OnPropertyChanged(nameof(ObstacleY1)); // 更新障碍物的Y位置
            
            _obstacle2.UpdatePosition1(6);  // 每次更新障碍物向左移动5像素
            OnPropertyChanged(nameof(ObstacleX2)); // 更新障碍物的X位置
            OnPropertyChanged(nameof(ObstacleY2)); // 更新障碍物的Y位置
            
            _obstacle3.UpdatePosition1(8);  // 每次更新障碍物向左移动5像素
            OnPropertyChanged(nameof(ObstacleX3)); // 更新障碍物的X位置
            OnPropertyChanged(nameof(ObstacleY3)); // 更新障碍物的Y位置
            
            _obstacle4.UpdatePosition2(15);  // 每次更新障碍物向左移动5像素
            OnPropertyChanged(nameof(ObstacleX4)); // 更新障碍物的X位置
            OnPropertyChanged(nameof(ObstacleY4)); // 更新障碍物的Y位置

           //碰撞检测
           if (CheckCollision())
           {
                 GameOver();
                 return;
            }
            


            // 限制小球在屏幕内跳跃，防止超出边界
           if (_ball.Y + _ball.Height > WindowHeight) // 检查是否触地
           {
               _ball.Y = WindowHeight - _ball.Height; // 限制在地面之上
               _ball.Velocity = 0; // 停止下落
           }

           if (_ball.Y < 0) // 检查是否跳出顶部
           {
               _ball.Y = 0; // 限制在顶部
               _ball.Velocity = 0; // 停止上升
           }
        }
        // 碰撞检测逻辑
        public bool CheckCollision()
        {
            // 遍历所有障碍物，检查与每个障碍物的碰撞
            var obstacles = new[] { _obstacle1, _obstacle2, _obstacle3, _obstacle4 };

            foreach (var obstacle in obstacles)
            {
                // 检查障碍物是否已经超出屏幕范围，避免不必要的计算
                if (obstacle.X + obstacle.Width < 0) // 如果障碍物已经完全移出屏幕
                {
                    continue; // 跳过该障碍物
                }

                // 检查小球是否与障碍物的水平边界重叠
                bool isBallInHorizontalRange = _ball.X + _ball.Width > obstacle.X && _ball.X < obstacle.X + obstacle.Width;

                // 检查小球是否与障碍物的垂直边界重叠
                bool isBallInVerticalRange = _ball.Y + _ball.Height > obstacle.Y && _ball.Y < obstacle.Y + obstacle.Height;

                // 如果小球在水平和垂直范围内，则发生碰撞
                if (isBallInHorizontalRange && isBallInVerticalRange)
                {
                    return true; // 发生碰撞，返回 true
                }
            }

            return false; // 如果没有与任何障碍物发生碰撞，则返回 false
        }
        
        
        // 游戏结束处理
        private void GameOver()
        {
            IsGameOver = true; // 设置游戏结束
            _timer.Stop(); // 停止游戏更新
            _ball.Velocity = 0; // 停止小球运动
        }
        
        private void GameWon()
        {
            IsGameOver = true;
            IsGameWon = true; // 设置游戏胜利
            _ball.Velocity = 0; // 停止小球运动
            
        }


        // 小球跳跃的逻辑
        public void Flap()
        {
            if (_isGameWon || _isGameOver) return; // 游戏结束时，禁用跳跃
            _ball.Flap();
            OnPropertyChanged(nameof(BallY));  // 触发跳跃时更新Y位置
        }

        private void TimerTick()
        {
            if (_isGameWon || _isGameOver)
            {
                return; // 如果游戏结束，停止计时
            }
            
            // 更新游戏时间
            _timeElapsedInSeconds += SecondsPerFrame;

// 如果游戏时间已达到 30秒并且没有发生碰撞，则触发胜利
            if (_timeElapsedInSeconds >= GameDurationInSeconds)
            {
                if (!CheckCollision()) // 如果没有碰撞发生
                {
                    GameWon(); // 游戏胜利
                    
                }
            }

            // 你可以在这里处理每帧需要执行的其他游戏逻辑
            Update();
        }
        
        
        [RelayCommand]
        public void GoToResultThreeView()
        {
            WeakReferenceMessenger.Default.Send<object, string>(2, "OnForwardNavigation");
            
            if (_isGameWon)
            {
                WeakReferenceMessenger.Default.Send<object, string>(true, "OnResultSubmitted");
            }
            else
            {
                WeakReferenceMessenger.Default.Send<object, string>(false, "OnResultSubmitted");
            }
            
        }
}