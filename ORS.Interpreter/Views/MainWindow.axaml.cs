using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using LibVLCSharp.Avalonia;
using ORS.Interpreter.ViewModels;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ORS.Interpreter;

public partial class MainWindow : Window
{
    private MainViewModel _model;

    public MainWindow()
    {
        InitializeComponent();

        if (Design.IsDesignMode)
            return;

        DataContext = _model = new MainViewModel();
    }
}