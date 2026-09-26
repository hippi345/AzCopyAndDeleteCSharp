# AzCopyAndDelete

[![CI](https://github.com/hippi345/AzCopyAndDeleteCSharp/actions/workflows/ci.yml/badge.svg)](https://github.com/hippi345/AzCopyAndDeleteCSharp/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

A small .NET console app that uploads a local file to Azure Blob Storage, optionally lists blobs in the container, and optionally deletes the local file after a successful upload.

## Features

- Interactive prompts for storage account, container, and file path (original behavior)
- Non-interactive mode via environment variables for automation
- Uses the current [`Azure.Storage.Blobs`](https://www.nuget.org/packages/Azure.Storage.Blobs) SDK on **.NET 8**
- Optional blob listing and local file cleanup after upload

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Setup

```bash
git clone https://github.com/hippi345/AzCopyAndDeleteCSharp.git
cd AzCopyAndDeleteCSharp
dotnet restore
dotnet build
```

## Configuration

Set credentials via a full connection string **or** account name + key. Never commit real secrets to source control.

| Variable | Description |
| --- | --- |
| `AZURE_STORAGE_CONNECTION_STRING` | Full Azure Storage connection string (preferred for automation) |
| `AZURE_STORAGE_ACCOUNT_NAME` | Storage account name (used with account key) |
| `AZURE_STORAGE_ACCOUNT_KEY` | Storage account key (used with account name) |
| `AZURE_STORAGE_CONTAINER` | Target blob container name |
| `AZURE_SOURCE_PATH` | Local file path to upload |
| `AZURE_BLOB_NAME` | Blob name in storage (defaults to the source path string, matching legacy behavior) |
| `AZURE_LIST_BLOBS` | `y`/`yes` or `n`/`no` to list blobs after upload |
| `AZURE_DELETE_LOCAL` | `y`/`yes` or `n`/`no` to delete the local file after upload |

Example (non-interactive):

```bash
export AZURE_STORAGE_CONNECTION_STRING="DefaultEndpointsProtocol=https;AccountName=...;AccountKey=...;EndpointSuffix=core.windows.net"
export AZURE_STORAGE_CONTAINER="backups"
export AZURE_SOURCE_PATH="/path/to/file.zip"
export AZURE_LIST_BLOBS="y"
export AZURE_DELETE_LOCAL="n"
dotnet run --project src/AzCopyAndDelete
```

## Usage

Interactive mode (prompts for any missing values):

```bash
dotnet run --project src/AzCopyAndDelete
```

Release build:

```bash
dotnet publish src/AzCopyAndDelete -c Release -o ./publish
./publish/AzCopyAndDelete
```

## Tests

Unit tests mock Azure Storage and run fully offline:

```bash
dotnet test
```

## Project structure

```
.
├── src/AzCopyAndDelete/          # Console application
├── tests/AzCopyAndDelete.Tests/  # xUnit tests
├── .github/workflows/ci.yml      # Build, format, and test on push/PR
├── AzCopyAndDelete.sln
└── README.md
```

## License

MIT — see [LICENSE](LICENSE).
