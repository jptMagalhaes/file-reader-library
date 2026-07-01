using System.IO;
using System.Windows;
using FileReaderApp.Coordinators;
using FileReaderApp.Services;
using FileReaderApp.ViewModels;
using FileReaderLibrary.Composition;
using FileReaderLibrary.Infrastructure;

namespace FileReaderApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var samplesDirectory = Path.Combine(AppContext.BaseDirectory, "samples");
            var factory = new FileReaderFactory(
                new ReverseEncryptionAlgorithm(),
                new ConfigurableFileAccessPolicy(
                [
                    "hello.rbac.json",
                    "hello.rbac.xml",
                    "hello.rbac.txt"
                ]));

            var coordinator = new FileReadSessionCoordinator(factory, samplesDirectory);
            DataContext = new MainWindowViewModel(coordinator, new WpfFilePickerService());
        }

        private void OnMinimizeClick(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void OnCloseClick(object sender, RoutedEventArgs e) => Close();
    }
}
