using CommunityToolkit.Mvvm.ComponentModel;

namespace ORS.Interpreter
{
    public partial class Subtitles : ObservableObject
    {
        [ObservableProperty] private string _text = string.Empty;

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
