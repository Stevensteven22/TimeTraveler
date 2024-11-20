using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Libary.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


    public class GameTwoViewModel : ViewModelBase
    {
        private readonly IMazeService _mazeService;
        private string _gameStatus;
        private string _maze;
        public int _playerX;
        public int _playerY;
        public bool isOK = false;

        public GameTwoViewModel(IMazeService mazeService)
        {
            _mazeService = mazeService;
            StartNewGame();
            MoveUpCommand = new RelayCommand(MoveUp);
            MoveDownCommand = new RelayCommand(MoveDown);
            MoveLeftCommand = new RelayCommand(MoveLeft);
            MoveRightCommand = new RelayCommand(MoveRight);
            QuitCommand = new RelayCommand(Quit);
        }

        public string Maze
        {
            get => _maze;
            set => SetProperty(ref _maze, value);
        }

        public string GameStatus
        {
            get => _gameStatus;
            set => SetProperty(ref _gameStatus, value);
        }

        public IRelayCommand MoveUpCommand { get; }
        public IRelayCommand MoveDownCommand { get; }
        public IRelayCommand MoveLeftCommand { get; }
        public IRelayCommand MoveRightCommand { get; }

        public void StartNewGame()
        {
            _mazeService.GenerateMaze();
            _playerX = 1;  // 玩家初始位置
            _playerY = 1;
            GameStatus = "游戏进行中...";
            UpdateMaze();
        }

        private void UpdateMaze()
        {
            // 获取当前的迷宫表示，并将玩家位置标识为 "P"
            var mazeRepresentation = _mazeService.GetMazeRepresentation();

            // Check if the maze representation is null or empty
            if (string.IsNullOrEmpty(mazeRepresentation))
            {
                GameStatus = "迷宫加载失败！";
                return;
            }

            // 假设玩家位置的符号为 "P"
            var mazeArray = mazeRepresentation.Split('\n');
            mazeArray[_playerY] = mazeArray[_playerY].Remove(_playerX, 1).Insert(_playerX, "P");

            Maze = string.Join("\n", mazeArray);
        }


        private void MoveUp() => MakeMove(0, -1);
        private void MoveDown() => MakeMove(0, 1);
        private void MoveLeft() => MakeMove(-1, 0);
        private void MoveRight() => MakeMove(1, 0);

        private void MakeMove(int dx, int dy)
        {
            int newX = _playerX + dx;
            int newY = _playerY + dy;

            
             if (newX==8&&newY==9)
             {
                 GameStatus = "恭喜你通关！！";
                 isOK = true;
                 StartNewGame();
                 GoToResultView();
             }
             
            if (_mazeService.IsMoveValid(newX, newY))  // 检查是否可以移动
            {
                _playerX = newX;
                _playerY = newY;
                UpdateMaze();
            }

           
        }
        
        public IRelayCommand QuitCommand { get; }

        private void Quit()
        {
            this.isOK = false;
            GoToResultView();
        }
        
         public void GoToResultView()
            {
                                
                StartNewGame();
                WeakReferenceMessenger.Default.Send<object, string>(2, "OnForwardNavigation");
                if (isOK  == true)
                {
                    WeakReferenceMessenger.Default.Send<object, string>(true, "OnResultSubmitted");
                }
                else
                {
                    WeakReferenceMessenger.Default.Send<object, string>(false, "OnResultSubmitted");
                }
            }
    }

   