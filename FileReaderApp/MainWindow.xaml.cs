using System.IO;
using System.Windows;
using FileReaderApp.Services;
using FileReaderLibrary.Composition;
using FileReaderLibrary.Infrastructure;
using FileReaderLibrary.Models;

namespace FileReaderApp;

public partial class MainWindow : Window
{
    private readonly FileReadService _fileReadService;

    public MainWindow()
    {
        InitializeComponent();

        var factory = new FileReaderFactory(
            new ReverseEncryptionAlgorithm(),
            new ConfigurableFileAccessPolicy());

        _fileReadService = new FileReadService(factory.Create(new FileReadRequest
        {
            Path = Path.Combine(AppContext.BaseDirectory, "samples", "hello.encrypted.xml"),
            Format = FileFormat.Xml,
            UseEncryption = true
        }));

        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "samples", "hello.encrypted.xml");
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