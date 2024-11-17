using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Libary.ViewModels;

public partial class InitializationViewModel : ViewModelBase {
    //private readonly IChapterStorage _chapterStorage;
    private readonly IRootNavigationService _rootNavigationService;

    public InitializationViewModel(//IChapterStorage _chapterStorage,
        IRootNavigationService rootNavigationService) {
        //_chapterStorage = _chapterStorage;
        _rootNavigationService = rootNavigationService;
        
        OnInitializedCommand = new AsyncRelayCommand(OnInitializedAsync);
    }

    private ICommand OnInitializedCommand { get; }

    public async Task OnInitializedAsync() {
        IsInitialized=true;
        //if (!_chapterStorage.IsInitialized) {
            //await _chapterStorage.InitializeAsync();
        //}

        
        await Task.Delay(1000);
        IsInitialized=false;
    }
    
    [ObservableProperty]
    private bool _isInitialized = false;
    
    [RelayCommand]
    public void Start() {
        _rootNavigationService.NavigateTo(RootNavigationConstant.MainView);
    }
    
    [RelayCommand]
    public void Restart() {
        WeakReferenceMessenger.Default.Send(new object(),"OnRestarted");
        _rootNavigationService.NavigateTo(RootNavigationConstant.MainView);
    }
}