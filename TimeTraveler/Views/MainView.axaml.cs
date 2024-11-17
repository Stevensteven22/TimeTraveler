using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using System;
using TimeTraveler.UserControls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace TimeTraveler.Views;

[PseudoClasses(PC_IsNavigated)]
public partial class MainView : UserControl
{
    public const string PC_IsNavigated = ":isNavigationEnabled";
    public bool IsNavigationButtonEnabled
    {
        get => GetValue(IsNavigationButtonEnabledProperty);
        set => SetValue(IsNavigationButtonEnabledProperty, value);
    }

    public static readonly StyledProperty<bool> IsNavigationButtonEnabledProperty =
        AvaloniaProperty.Register<MainView, bool>(
            nameof(IsNavigationButtonEnabled),
            defaultValue: false
        );



    public MainView()
    {
        InitializeComponent();
        IsNavigationButtonEnabledProperty.Changed.AddClassHandler<MainView>(
            IsNavigationButtonEnabledPropertyChanged
        );
       


    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (this.IsNavigationButtonEnabled)
            this.SetPseudoclasses(PC_IsNavigated, true);
        else
            this?.SetPseudoclasses(PC_IsNavigated, false);
    }

    private static void IsNavigationButtonEnabledPropertyChanged(
        object sender,
        AvaloniaPropertyChangedEventArgs e
    )
    {
        var control = sender as MainView;
        if (control != null && control.IsNavigationButtonEnabled)
            control.SetPseudoclasses(PC_IsNavigated, true);
        else
            control?.SetPseudoclasses(PC_IsNavigated, false);
    }

    private void SetPseudoclasses(string name, bool flag)
    {
        PseudoClasses.Set(name, flag);
    }
}
