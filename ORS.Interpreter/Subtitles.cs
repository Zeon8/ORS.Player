using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Interpreter
{
    public partial class Subtitles : ObservableObject
    {
        [ObservableProperty] private string _text;

        public void Show(string text)
        {
            Text = text;
        }

        public void Hide()
        {
            Text = string.Empty;
        }
    }
}
