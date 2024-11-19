using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using TimeTraveler.Libary.ViewModels;

namespace TimeTraveler.Views;

public partial class GameTwoView : UserControl
{
    private readonly GameTwoViewModel _viewModel;
    public GameTwoView()
    {
        _viewModel = ServiceLocator.Current.GameTwoViewModel;
        InitializeComponent();
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
}