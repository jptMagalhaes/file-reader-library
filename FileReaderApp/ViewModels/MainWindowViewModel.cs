using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FileReaderApp.Coordinators;
using FileReaderApp.Services;
using FileReaderLibrary.Models;

namespace FileReaderApp.ViewModels;

public sealed class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly FileReadSessionCoordinator _coordinator;
    private readonly IFilePickerService _filePickerService;
    private string? _selectedSampleFile;
    private string? _selectedFilePath;
    private FileFormat _selectedFormat = FileFormat.Text;
    private UserRole _selectedRole = UserRole.Admin;
    private bool _useEncryption;
    private bool _useRoleSecurity;
    private string _output = "Select a sample or browse for your file, then click Read.";

    public MainWindowViewModel(
        FileReadSessionCoordinator coordinator,
        IFilePickerService filePickerService)
    {
        _coordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
        _filePickerService = filePickerService ?? throw new ArgumentNullException(nameof(filePickerService));

        SampleFiles = new ObservableCollection<string>(_coordinator.ListSampleFiles());
        _selectedSampleFile = SampleFiles.FirstOrDefault();

        ReadCommand = new RelayCommand(ExecuteRead, CanExecuteRead);
        BrowseCommand = new RelayCommand(ExecuteBrowse);

        UpdatePathFromSample();
    }

    public ObservableCollection<string> SampleFiles { get; }

    public Array AvailableFormats { get; } = Enum.GetValues<FileFormat>();

    public Array AvailableRoles { get; } = Enum.GetValues<UserRole>();

    public string? SelectedSampleFile
    {
        get => _selectedSampleFile;
        set
        {
            if (_selectedSampleFile == value)
            {
                return;
            }

            _selectedSampleFile = value;
            UpdatePathFromSample();
            if (SelectedFilePath is not null)
            {
                SelectedFormat = InferFormatFromExtension(SelectedFilePath);
            }
            OnPropertyChanged();
            ((RelayCommand)ReadCommand).RaiseCanExecuteChanged();
        }
    }

    public string? SelectedFilePath
    {
        get => _selectedFilePath;
        private set
        {
            if (_selectedFilePath == value)
            {
                return;
            }

            _selectedFilePath = value;
            OnPropertyChanged();
            ((RelayCommand)ReadCommand).RaiseCanExecuteChanged();
        }
    }

    public FileFormat SelectedFormat
    {
        get => _selectedFormat;
        set
        {
            if (_selectedFormat == value)
            {
                return;
            }

            _selectedFormat = value;
            OnPropertyChanged();
        }
    }

    public UserRole SelectedRole
    {
        get => _selectedRole;
        set
        {
            if (_selectedRole == value)
            {
                return;
            }

            _selectedRole = value;
            OnPropertyChanged();
        }
    }

    public bool UseEncryption
    {
        get => _useEncryption;
        set
        {
            if (_useEncryption == value)
            {
                return;
            }

            _useEncryption = value;
            OnPropertyChanged();
        }
    }

    public bool UseRoleSecurity
    {
        get => _useRoleSecurity;
        set
        {
            if (_useRoleSecurity == value)
            {
                return;
            }

            _useRoleSecurity = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsRoleSelectionEnabled));
        }
    }

    public bool IsRoleSelectionEnabled => UseRoleSecurity;

    public string Output
    {
        get => _output;
        private set
        {
            if (_output == value)
            {
                return;
            }

            _output = value;
            OnPropertyChanged();
        }
    }

    public ICommand ReadCommand { get; }

    public ICommand BrowseCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private bool CanExecuteRead() => !string.IsNullOrWhiteSpace(SelectedFilePath);

    private void ExecuteBrowse()
    {
        var path = _filePickerService.PickFile();
        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        _selectedSampleFile = null;
        OnPropertyChanged(nameof(SelectedSampleFile));
        SelectedFilePath = path;
        SelectedFormat = InferFormatFromExtension(path);
    }

    private void ExecuteRead()
    {
        var request = new FileReadRequest
        {
            Path = SelectedFilePath!,
            Format = SelectedFormat,
            UseEncryption = UseEncryption,
            UseRoleSecurity = UseRoleSecurity,
            Role = SelectedRole
        };

        try
        {
            Output = _coordinator.Read(request);
        }
        catch (Exception ex)
        {
            Output = $"Failed to read {SelectedFilePath}:{Environment.NewLine}{ex.Message}";
        }
    }

    private void UpdatePathFromSample()
    {
        SelectedFilePath = string.IsNullOrWhiteSpace(SelectedSampleFile)
            ? null
            : Path.Combine(_coordinator.SamplesDirectory, SelectedSampleFile);
    }

    private static FileFormat InferFormatFromExtension(string path) =>
        Path.GetExtension(path).ToLowerInvariant() switch
        {
            ".xml" => FileFormat.Xml,
            ".json" => FileFormat.Json,
            _ => FileFormat.Text
        };

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
