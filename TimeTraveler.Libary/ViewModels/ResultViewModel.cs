﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Helpers;
using TimeTraveler.Libary.Models;

namespace TimeTraveler.Libary.ViewModels;

public partial class ResultViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _result;

    [RelayCommand]
    private void GoToReturnView()
    {
        WeakReferenceMessenger.Default.Send<object, string>(3, "OnForwardNavigation");
    }

    public ResultViewModel()
    {
        OnLoadedCommand = new AsyncRelayCommand(OnLoadedAsync);
        WeakReferenceMessenger.Default.Register<object, string>(this, "OnResultSubmitted", (sender, message) =>
        {
            if (message is bool isSucceed && isSucceed)
            {
                IsOK = true;
                Result = "  **悬念**:这块神秘的时之石到底是什么?它带艾琳去了哪里?而那个警告声里的“代价”又指的是什么?";
                WeakReferenceMessenger.Default.Send<object, string>(new object(), "OnGameSucceed");
            }else 
            {
                IsOK = false;
                Result = "时间旅行者似乎没有找到那块神秘的时之石...";
                WeakReferenceMessenger.Default.Send<object, string>(new object(), "OnGameFailed");
            }
        });
       
    }

   
    private ICommand OnLoadedCommand { get; }

    [ObservableProperty]
    private bool _isOK;

    public async Task OnLoadedAsync()
    {
        /*// 创建一个 Random 对象
        Random random = new Random();

        // 生成一个随机的布尔值
        IsOK = random.Next(2) == 1; // 0 或 1，0 对应 false，1 对应 true

        if (IsOK)
        {
            WeakReferenceMessenger.Default.Send<object, string>(new object(), "OnGameSucceed");
            Result = "Congratulations! You have succeeded.";
        }
        else
        {
            WeakReferenceMessenger.Default.Send<object, string>(new object(), "OnGameFailed");
            Result = "Sorry, you have failed.";
        }*/

        await Task.CompletedTask;
    }
}
