using System.IO;
using System.Windows;
using FileReaderApp.Services;
using FileReaderLibrary.Readers;

namespace FileReaderApp;

public partial class MainWindow : Window
{
    private readonly FileReadService _fileReadService = new(new TextFileReader());

    public MainWindow()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "samples", "hello.txt");
        try
        {
            SampleContentText.Text = _fileReadService.Read(path);
        }
        catch (Exception ex)
        {
            SampleContentText.Text = $"Failed to read {path}:\n{ex.Message}";
        }
    }
}