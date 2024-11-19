﻿using System.Text;
using TimeTraveler.Libary.Models;

namespace TimeTraveler.Libary.Services;

using System;
using System.Collections.ObjectModel;



public interface IMazeService
{
    void GenerateMaze();  // 生成迷宫
    string GetMazeRepresentation();  // 获取迷宫的文本表示
    bool IsMoveValid(int x, int y);  // 检查是否可以移动到指定位置
}

public class MazeService : IMazeService
{
    private char[,] _maze;
    private int _playerX;
    private int _playerY;

    public void GenerateMaze()
    {
        // 创建一个简单的 5x5 迷宫
        _maze = new char[10, 10]
        {
            { '#', '#', '#', '#', '#', '#', '#', '#', '#', '#' },
            { '#', ' ', ' ', ' ', ' ','#', ' ', ' ', ' ', '#' },
            { '#', ' ',' ', '#', '#', ' ', '#', '#', ' ', '#' },
            { '#', ' ', ' ','#', ' ', ' ', ' ','#', ' ', '#' },
            { '#', ' ', '#', '#',' ', ' ', ' ', '#', ' ', '#' },
            { '#', ' ', '#', ' ', '#', ' ', ' ','#', ' ', '#' },
            { '#', ' ', ' ', ' ',' ', '#', '#', '#', ' ', '#' },
            { '#', ' ', '#', ' ', ' ',' ', ' ', ' ', ' ', '#' },
            { '#', ' ', '#', '#',' ', '#', ' ', ' ', ' ', '#' },
            { '#', '#', '#', '#', '#', '#', '#', ' ','#', '#' }
        };

        // 设置玩家的初始位置
        _playerX = 1;
        _playerY = 1;
    }

    public string GetMazeRepresentation()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < _maze.GetLength(0); i++)
        {
            for (int j = 0; j < _maze.GetLength(1); j++)
            {
                sb.Append(_maze[i, j]);
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public bool IsMoveValid(int x, int y)
    {
        if (x < 0 || x >= _maze.GetLength(0) || y < 0 || y >= _maze.GetLength(1))
        {
            return false;
        }
        return _maze[y, x] != '#';  // 如果是墙壁 ('#')，则无法移动
    }
}


