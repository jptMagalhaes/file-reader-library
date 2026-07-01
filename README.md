# File Reader Library

Biblioteca C# extensível (Strategy + Decorator + DI) para leitura de ficheiros de texto, XML e JSON,
com suporte opcional a encriptação e segurança baseada em papéis (roles).

Desenvolvida incrementalmente através de user stories, cada uma taggeada como uma versão (`v1`–`v9`).

## Estrutura

```
FileReaderLibrary.sln
FileReaderLibrary/          # biblioteca principal
FileReaderLibrary.Tests/    # testes xUnit
FileReaderApp/              # aplicação WPF de demonstração (bonus)
samples/                    # ficheiros de exemplo para testar manualmente
```

## Como correr

A library em si **não é executável** — valida com testes ou com a app WPF de demo.

```powershell
# testes (v1+)
dotnet test

# demo WPF — lê samples/hello.txt com TextFileReader
dotnet run --project FileReaderApp/FileReaderApp.csproj
```

No VS Code: F5 com perfil **FileReaderApp (WPF demo)** (`.vscode/launch.json`).

## Mapa de versões

| Tag | User story |
|-----|------------|
| v1 | Ler ficheiro de texto |
| v2 | Ler ficheiro XML |
| v3 | Ler ficheiro TEXT encriptado |
| v4 | Ler ficheiros XML com segurança baseada em papéis |
| v5 | Ler ficheiro XML encriptado |
| v6 | Ler ficheiro TEXT com segurança baseada em papéis |
| v7 | Ler ficheiro JSON |
| v8 | Ler ficheiro JSON encriptado |
| v9 | Ler ficheiro JSON com segurança baseada em papéis |
