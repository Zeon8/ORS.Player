using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Interpreter
{
    public partial class BlackBackground : ObservableObject
    {
        [ObservableProperty] private double _opacity = 0;
        [ObservableProperty] private TimeSpan _duration = TimeSpan.Zero;

        public void FadeIn(TimeSpan timeSpan)
        {
            Opacity = 0;
            Duration = timeSpan;
            Opacity = 1;
        }

        public void Hide()
        {
            Duration = TimeSpan.Zero;
            Opacity = 0;
        }
    }
}
