using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Helpers;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.Services;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace TimeTraveler.Libary.ViewModels;

public partial class GameViewModel : ViewModelBase
{
    private readonly IResultVerifyService _resultVerifyService;
    public WindowNotificationManager? NotificationManager { get; set; }

    public string Problem1Answer = ""; //苹果

    [ObservableProperty]
    private string _problem2Answer = ""; //攫金鸦印

    [ObservableProperty]
    private string _problem3Answer = ""; //琉璃百合

    [ObservableProperty]
    private GamePuzzleOption _selectedOption;

    [ObservableProperty]
    private ObservableCollection<GamePuzzleOption> _options =
        new ObservableCollection<GamePuzzleOption>()
        {
            new GamePuzzleOption()
            {
                Text = "琉璃百合",
                Icon = ImageHelper.LoadFromResource(
                    new Uri("avares://TimeTraveler/Assets/琉璃百合.png")
                ),
            },
            new GamePuzzleOption()
            {
                Text = "霓裳花",
                Icon = ImageHelper.LoadFromResource(
                    new Uri("avares://TimeTraveler/Assets/霓裳花.png")
                ),
            },
            new GamePuzzleOption()
            {
                Text = "清心",
                Icon = ImageHelper.LoadFromResource(
                    new Uri("avares://TimeTraveler/Assets/清心.png")
                ),
            },
        };

    public GameViewModel(IResultVerifyService resultVerifyService)
    {
        _resultVerifyService = resultVerifyService;
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
        var verifyResult = _resultVerifyService.BeforeVerify(
            new
            {
                Problem1Answer = Problem1Answer,
                Problem2Answer = Problem2Answer,
                Problem3Answer = Problem3Answer,
            }
        );
        if (!verifyResult.Item1)
        {
            Enum.TryParse<NotificationType>("Warning", out var notificationType);
            NotificationManager?.Show(
                new Notification("警告", verifyResult.Item2),
                showIcon: true,
                showClose: true,
                type: notificationType
            );
            return;
        }

        WeakReferenceMessenger.Default.Send<object, string>(2, "OnForwardNavigation");
        if (
            _resultVerifyService.Verify(
                new
                {
                    Problem1Answer = Problem1Answer,
                    Problem2Answer = Problem2Answer,
                    Problem3Answer = Problem3Answer,
                }
            )
        /*Problem1Answer == "苹果"
        && Problem2Answer == "攫金鸦印"
        && Problem3Answer == "琉璃百合"*/
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
