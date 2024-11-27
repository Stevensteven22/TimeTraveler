using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Helpers;
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
    public ObservableCollection<ObservableCollection<MazeCell>> MazeGrid { get; } 
        = new ObservableCollection<ObservableCollection<MazeCell>>(); // 迷宫格子集合

    public int RowCount { get; } = 10;  // 固定为 10 行
    public int ColumnCount { get; } = 10;  // 固定为 10 列

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

    public void StartNewGame()
    {
        _mazeService.GenerateMaze();
        _playerX = 1;  // 玩家初始位置
        _playerY = 1;
        GameStatus = "游戏进行中...";
        UpdateMaze();
    }

    public void UpdateMaze()
    {
        // 获取迷宫的表示
        var mazeRepresentation = _mazeService.GetMazeRepresentation();

        if (string.IsNullOrEmpty(mazeRepresentation))
        {
            GameStatus = "迷宫加载失败！";
            return;
        }

        var mazeArray = mazeRepresentation.Split('\n');
        MazeGrid.Clear();

        for (int i = 0; i < RowCount; i++)
        {
            var mazeRow = new ObservableCollection<MazeCell>();
            for (int j = 0; j < ColumnCount; j++)
            {
                char character = mazeArray[i][j];
                var mazeCell = new MazeCell
                {
                    Character = character.ToString(),
                    Image = GetImageForCharacter(character),
                    Row = i,
                    Column = j
                };

                // 如果是玩家的位置，更新玩家图像
                if (_playerX == j && _playerY == i)
                {
                    mazeCell.Image = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/Player.png"));
                }

                mazeRow.Add(mazeCell);
            }
            MazeGrid.Add(mazeRow);
        }

    }

    private Bitmap GetImageForCharacter(char character)
    {
        // 根据字符返回对应的图片
        switch (character)
        {
            case 'P':
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/Player.png"));
            case '#':
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/Wall.png"));
            case ' ':
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/Floor.png"));
            default:
                return ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/Floor.png"));
        }
    }

    // 玩家移动
    private void MoveUp() => MakeMove(0, -1);
    private void MoveDown() => MakeMove(0, 1);
    private void MoveLeft() => MakeMove(-1, 0);
    private void MoveRight() => MakeMove(1, 0);

    private void MakeMove(int dx, int dy)
    {
        int newX = _playerX + dx;
        int newY = _playerY + dy;

        if (newX == 8 && newY == 9)
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
        if (isOK)
        {
            WeakReferenceMessenger.Default.Send<object, string>(true, "OnResultSubmitted");
        }
        else
        {
            WeakReferenceMessenger.Default.Send<object, string>(false, "OnResultSubmitted");
        }
    }
}

  public class MazeCell
  {
      public string Character { get; set; }  // 表示字符
      public Bitmap Image { get; set; }  // 对应的图片
      public int Row { get; set; }  // 行位置
      public int Column { get; set; }  // 列位置
  }


   