using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Helpers;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Libary.ViewModels;

public partial class GameFiveViewModel : ViewModelBase
{
    private readonly IResultVerifyFiveService _resultVerifyFiveService;
    public WindowNotificationManager? NotificationManager { get; set; }

    public string Problem1Answer = ""; //C

    [ObservableProperty]
    private string _problem3Answer = ""; //辛焱

    [ObservableProperty]
    private GamePuzzleOption _selectedOption;

    [ObservableProperty]
    private ObservableCollection<GamePuzzleOption> _options =
        new ObservableCollection<GamePuzzleOption>()
        {
            new GamePuzzleOption()
            {
                Text = "雷电将军",
                Icon = ImageHelper.LoadFromResource(
                    new Uri("avares://TimeTraveler/Assets/雷电将军.png")
                ),
            },
            new GamePuzzleOption()
            {
                Text = "辛焱",
                Icon = ImageHelper.LoadFromResource(
                    new Uri("avares://TimeTraveler/Assets/辛焱.png")
                ),
            },
        };

    public GameFiveViewModel(IResultVerifyFiveService resultVerifyFiveService)
    {
        _resultVerifyFiveService = resultVerifyFiveService;
        this.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(SelectedOption))
            {
                Problem3Answer = SelectedOption.Text;
            }
        };
    }
    public ICommand SelectionChangedCommand =>
        new RelayCommand<GamePuzzleOption>(option =>
        {
            Problem3Answer = option.Text;
        });

    [RelayCommand]
    public void GoToResultView()
    {
        var verifyResult = _resultVerifyFiveService.BeforeVerifyFive(
            new
            {
                Problem1Answer = Problem1Answer,
                Problem3Answer = Problem3Answer,
            }
        );
        if (!verifyResult.Item1)
        {
            Enum.TryParse<NotificationType>("Warning", out var notificationType);
            NotificationManager?.Show(
                new Notification("警告", verifyResult.Item2),
                //showIcon: true,
                //showClose: true,
                type: notificationType
            );
            return;
        }

        WeakReferenceMessenger.Default.Send<object, string>(2, "OnForwardNavigation");
        if (
            _resultVerifyFiveService.VerifyFive(
                new
                {
                    Problem1Answer = Problem1Answer,
                    Problem3Answer = Problem3Answer,
                }
            )
        )
        {
            WeakReferenceMessenger.Default.Send<object, string>(true, "OnResultSubmitted");
        }
        else
        {
            WeakReferenceMessenger.Default.Send<object, string>(false, "OnResultSubmitted");
        }
    }

    [RelayCommand]
    public void SelectAnswer(string answer)
    {
        Problem1Answer = answer;
    }

    
}