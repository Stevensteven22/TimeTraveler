using System.Windows.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Libary.ViewModels;

public class GameThreeViewModel :ViewModelBase
{
     private bool _isGameOver = false; // 游戏是否结束的标志
        private  Flyer _ball;
        private Obstacle _obstacle1;
        private Obstacle _obstacle2;
        private Obstacle _obstacle3;
        private Obstacle _obstacle4;
        private readonly IFlyService _flyService;
        private DispatcherTimer _timer;
        
        // 硬编码的窗口宽高
        private const double WindowWidth = 1200;
        private const double WindowHeight = 800;

        // 公开属性，供 UI 绑定
        public bool IsGameOver
        {
            get => _isGameOver;
            set
            {
                if (_isGameOver != value)
                {
                    _isGameOver = value;
                    OnPropertyChanged(nameof(IsGameOver));
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
        public double BallRadius => _ball.Radius;

        

        
        public ICommand FlapCommand { get; }
        public ICommand RestartCommand { get; }

        public GameThreeViewModel(IFlyService flyService)
        {
            _flyService = flyService;
            _ball = new Flyer(100, 100, 25);  // 初始化小球
           // _obstacle = new Obstacle(600, 100, 100, 200);
            
           _obstacle1 = new Obstacle(1200, 0, 100, 300);
           _obstacle2 = new Obstacle(1200, 600, 100, 200);
           _obstacle3 = new Obstacle(1200, 700, 100, 100);
           _obstacle4 = new Obstacle(1200, 0, 1000, 50);
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16) // 60FPS
            };
            _timer.Tick += (sender, e) => Update();
            _timer.Start();
            
            
            // 使用 RelayCommand 实现 FlapCommand
            FlapCommand = new RelayCommand(Flap);
            
            RestartCommand = new RelayCommand(ResetGame); // 创建“重新开始”命令
        }

        public void Update()
        {
            if (_isGameOver)
            {
                return; // 游戏结束，停止更新
            }

            _ball.UpdatePosition();
            OnPropertyChanged(nameof(BallX));  // 更新小球X位置
            OnPropertyChanged(nameof(BallY));  // 更新小球Y位置
            
            
            
            _obstacle1.UpdatePosition1(5);  // 每次更新障碍物向左移动5像素
            OnPropertyChanged(nameof(ObstacleX1)); // 更新障碍物的X位置
            OnPropertyChanged(nameof(ObstacleY1)); // 更新障碍物的Y位置
            
            _obstacle2.UpdatePosition1(8);  // 每次更新障碍物向左移动5像素
            OnPropertyChanged(nameof(ObstacleX2)); // 更新障碍物的X位置
            OnPropertyChanged(nameof(ObstacleY2)); // 更新障碍物的Y位置
            
            _obstacle3.UpdatePosition1(10);  // 每次更新障碍物向左移动5像素
            OnPropertyChanged(nameof(ObstacleX3)); // 更新障碍物的X位置
            OnPropertyChanged(nameof(ObstacleY3)); // 更新障碍物的Y位置
            
            _obstacle4.UpdatePosition2(15);  // 每次更新障碍物向左移动5像素
            OnPropertyChanged(nameof(ObstacleX4)); // 更新障碍物的X位置
            OnPropertyChanged(nameof(ObstacleY4)); // 更新障碍物的Y位置

           //碰撞检测
           if (CheckCollision())
           {
                GameOver(); // 如果碰撞发生，游戏结束
            }
            


            // 限制小球在屏幕内跳跃，防止超出边界
            if (_ball.Y > WindowHeight - _ball.Radius) // 检查是否触地
            {
                _ball.Y = WindowHeight - _ball.Radius; // 限制在地面之上
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
            var obstacles = new[] { _obstacle1, _obstacle2, _obstacle3 ,_obstacle4}; // 所有障碍物的集合
            foreach (var obstacle in obstacles)
            {
                // 计算小球中心到障碍物矩形的最近距离
                double nearestX = Math.Max(obstacle.X, Math.Min(_ball.X, obstacle.X + obstacle.Width));
                double nearestY = Math.Max(obstacle.Y, Math.Min(_ball.Y, obstacle.Y + obstacle.Height));

                // 计算小球中心与矩形最近点的距离
                double deltaX = _ball.X - nearestX;
                double deltaY = _ball.Y - nearestY;

                // 如果距离小球的半径小于障碍物的矩形，发生碰撞
                if ((deltaX * deltaX + deltaY * deltaY) < (_ball.Radius * _ball.Radius))
                {
                    return true; // 发生碰撞，返回 true
                }
            }
            return false; // 如果没有与任何障碍物发生碰撞，则返回 false
        }
        
        // 碰撞检测逻辑，检查小球与障碍物的碰撞
      

        // 游戏结束的处理逻辑
        private void GameOver()
        {
            IsGameOver = true; // 设置游戏结束标志
            _ball.Velocity = 0; // 停止小球运动

            // 可以在这里添加其他Game Over处理，例如弹出对话框、重置按钮等
        }

        // 小球跳跃的逻辑
        public void Flap()
        {   
            Console.WriteLine("Space");
            if (_isGameOver) return; // 游戏结束时，禁用跳跃
            _ball.Flap();
            OnPropertyChanged(nameof(BallY));  // 触发跳跃时更新Y位置
        }
        
        public void ResetGame()
        {
            _ball = new Flyer(100, 100, 25);  // 重置小球的位置和半径
            _obstacle1 = new Obstacle(800, 300, 100, 300);  // 重置障碍物的位置和尺寸
            _obstacle2 = new Obstacle(900, 400, 100, 200);
            _obstacle3 = new Obstacle(1000, 0, 100, 100);
        
            IsGameOver = false;  // 设置游戏未结束
            _timer.Start();  // 启动定时器
        }
}