# Fullstack.SerilogloggingDemo

Lightweight demo project that shows how to configure and use Serilog for structured logging in a .NET 10 application.

## Overview

This repository contains a small .NET application demonstrating best-practice logging using Serilog. It includes a minimal setup typically configured in Program.cs and examples for console and file sinks.

## Prerequisites

- .NET 10 SDK (install from https://dotnet.microsoft.com/) 
- Visual Studio 2022/2026 or any editor that supports .NET development

## Build and run (CLI)

1. Open a terminal in the repository root (PowerShell is recommended).
2. Restore and build:

   dotnet restore
   dotnet build

3. Run the project:

   dotnet run --project ./

Or open the solution in Visual Studio and run the startup project.

## Configuration

The sample config lives in Program.cs and (optionally) appsettings.*.json. Typical Serilog packages used in this kind of project:

- Serilog
- Serilog.AspNetCore
- Serilog.Sinks.Console
- Serilog.Sinks.File

A minimal Program.cs Serilog bootstrap looks like:

```csharp
// configure Serilog early in Program.cs
Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Debug()
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
	.CreateLogger();

builder.Host.UseSerilog();
```

Adjust sinks and minimum level according to your environment (Development vs Production).

## Tests

If the solution includes unit tests, run them with:

   dotnet test

## Troubleshooting

- If logs do not appear, verify Serilog initialization runs before other services are built.
- Check file permissions for file sinks.

## License

This project is provided as-is. Check repository license if any.
