﻿using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Data.Core.Plugins;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.ViewModels;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace TimeTraveler.Views;

public partial class GameView : UserControl
{
    private readonly GameViewModel _viewModel;

    public GameView()
    {
        _viewModel = ServiceLocator.Current.GameViewModel;
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<object, string>(
            this,
            "OnRestarted",
            (r, p) =>
            {
                OnRestarted();
            }
        );
    }

    private void OnRestarted()
    {
        rb_Problem1Answer1.IsChecked = false;
        rb_Problem1Answer2.IsChecked = false;
        rb_Problem1Answer3.IsChecked = false;
        txb_Problem2Answer.Text = "";
        cb_Problem3Answer.SelectedIndex = -1;
        _viewModel.Problem2Answer = "";
        _viewModel.Problem3Answer = "";
        _viewModel.Problem1Answer = "";
    }

    private void Button_PointerEntered_1(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            button.Background = Brushes.AliceBlue;
        }
    }

    private void Button_PointerExited_2(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            button.Background = new SolidColorBrush(Color.Parse("#E9E5D9"));
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var topLevel = TopLevel.GetTopLevel(this);
        _viewModel.NotificationManager = new WindowNotificationManager(topLevel)
        {
            MaxItems = 3,
            Position = NotificationPosition.TopLeft,
        };
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);
        _viewModel.NotificationManager?.Uninstall();
    }
}
