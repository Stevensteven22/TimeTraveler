namespace TimeTraveler.Libary.Models;

using CommunityToolkit.Mvvm.ComponentModel;




    public class MoleHole : ObservableObject
    {
        private string _display;

        public string Display
        {
            get => _display;
            set => SetProperty(ref _display, value);
        }
    }

