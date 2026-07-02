# File Reader Library

Extensible C# library (Strategy + Decorator + Factory) for reading text, XML, and JSON files,
with optional encryption and role-based access control.

Built incrementally through user stories, each tagged as a version (`v1`–`v9`), plus a WPF demo app.

## Structure

```
FileReaderLibrary.sln
FileReaderLibrary/          # core library
FileReaderLibrary.Tests/    # xUnit tests
FileReaderApp/              # WPF interactive demo (bonus)
samples/                    # sample files for manual testing
```

## Requirements

- .NET 9 SDK
- Windows (WPF demo only)

## How to run

The library itself is **not executable** — validate it with tests or the WPF demo.

```powershell
# run all tests
dotnet test

# WPF demo — pick a sample or browse for a file
dotnet run --project FileReaderApp/FileReaderApp.csproj
```

In VS Code / Cursor: **F5** with the **FileReaderApp** launch profile (`.vscode/launch.json`).

Run commands from the folder that contains `FileReaderLibrary.sln`.

## WPF demo

The demo lets you:

- Select a file from `samples/` or browse for your own
- Choose format (Text, Xml, Json)
- Toggle encryption and role security
- Pick a role (Admin / Viewer)

Viewer access is restricted to an allowlist of sample filenames (`hello.rbac.*`).

## Version map

| Tag | User story |
|-----|------------|
| v1 | Read a text file |
| v2 | Read an XML file |
| v3 | Read an encrypted text file |
| v4 | Read XML files with role-based security |
| v5 | Read an encrypted XML file |
| v6 | Read a text file with role-based security |
| v7 | Read a JSON file |
| v8 | Read an encrypted JSON file |
| v9 | Read a JSON file with role-based security |
