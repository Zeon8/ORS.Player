using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Interpreter
{
    public partial class Selection : ObservableObject
    {
        [ObservableProperty]
        private string? firstOption;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SecondOptionAvailable))]
        private string? _secondOption;

        [ObservableProperty]
        private bool _visible;

        public bool SecondOptionAvailable => SecondOption is not null;

        public void Hide() => Visible = false;

        public void ShowOptions(string firstOption, string? secondOption)
        {
            Visible = true;
            FirstOption = firstOption;
            if (secondOption is not null)
                SecondOption = secondOption;
            else
                SecondOption = null;
        }
    }
}
