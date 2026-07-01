using System.IO;
using FileReaderLibrary.Composition;
using FileReaderLibrary.Models;

namespace FileReaderApp.Coordinators;

public sealed class FileReadSessionCoordinator(FileReaderFactory factory, string samplesDirectory)
{
    private readonly FileReaderFactory _factory = factory ?? throw new ArgumentNullException(nameof(factory));

    public string SamplesDirectory { get; } = samplesDirectory ?? throw new ArgumentNullException(nameof(samplesDirectory));

    public IReadOnlyList<string> ListSampleFiles()
    {
        if (!Directory.Exists(SamplesDirectory))
        {
            return [];
        }

        return [.. Directory.GetFiles(SamplesDirectory)
            .Select(Path.GetFileName)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
            .Cast<string>()];
    }

    public string Read(FileReadRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var reader = _factory.Create(request);
        return reader.Read(request.Path);
    }
}
