using Avalonia.Controls;
using LibVLCSharp.Avalonia;
using ORS.Interpreter.ViewModels;

namespace ORS.Interpreter;

public partial class MainWindow : Window
{
    private readonly VideoView _videoView;
    private readonly MainViewModel _model;

    public MainWindow()
    {
        InitializeComponent();

        if (Design.IsDesignMode)
            return;

        _model = new MainViewModel();
        DataContext = _model;
        Opened += MainWindow_Opened;
    }

    private void MainWindow_Opened(object? sender, System.EventArgs e)
    {
        _model.Play();
    }
}