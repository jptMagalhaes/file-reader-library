using Microsoft.Win32;

namespace FileReaderApp.Services;

public sealed class WpfFilePickerService : IFilePickerService
{
    public string? PickFile()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Select a file to read",
            Filter = "Supported files|*.txt;*.xml;*.json|Text|*.txt|XML|*.xml|JSON|*.json|All files|*.*",
            CheckFileExists = true,
            Multiselect = false
        };

        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }
}
