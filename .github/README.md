# Reading Time

[![Documentation](https://img.shields.io/badge/Documentation-123?color=394933&logo=data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNiIgaGVpZ2h0PSIxNiIgZmlsbD0iY3VycmVudENvbG9yIiBjb2xvcj0id2hpdGUiIGNsYXNzPSJiaSBiaS1ib29rIiB2aWV3Qm94PSIwIDAgMTYgMTYiPgogIDxwYXRoIGQ9Ik0xIDIuODI4Yy44ODUtLjM3IDIuMTU0LS43NjkgMy4zODgtLjg5MyAxLjMzLS4xMzQgMi40NTguMDYzIDMuMTEyLjc1MnY5Ljc0NmMtLjkzNS0uNTMtMi4xMi0uNjAzLTMuMjEzLS40OTMtMS4xOC4xMi0yLjM3LjQ2MS0zLjI4Ny44MTF6bTcuNS0uMTQxYy42NTQtLjY4OSAxLjc4Mi0uODg2IDMuMTEyLS43NTIgMS4yMzQuMTI0IDIuNTAzLjUyMyAzLjM4OC44OTN2OS45MjNjLS45MTgtLjM1LTIuMTA3LS42OTItMy4yODctLjgxLTEuMDk0LS4xMTEtMi4yNzgtLjAzOS0zLjIxMy40OTJ6TTggMS43ODNDNy4wMTUuOTM2IDUuNTg3LjgxIDQuMjg3Ljk0Yy0xLjUxNC4xNTMtMy4wNDIuNjcyLTMuOTk0IDEuMTA1QS41LjUgMCAwIDAgMCAyLjV2MTFhLjUuNSAwIDAgMCAuNzA3LjQ1NWMuODgyLS40IDIuMzAzLS44ODEgMy42OC0xLjAyIDEuNDA5LS4xNDIgMi41OS4wODcgMy4yMjMuODc3YS41LjUgMCAwIDAgLjc4IDBjLjYzMy0uNzkgMS44MTQtMS4wMTkgMy4yMjItLjg3NyAxLjM3OC4xMzkgMi44LjYyIDMuNjgxIDEuMDJBLjUuNSAwIDAgMCAxNiAxMy41di0xMWEuNS41IDAgMCAwLS4yOTMtLjQ1NWMtLjk1Mi0uNDMzLTIuNDgtLjk1Mi0zLjk5NC0xLjEwNUMxMC40MTMuODA5IDguOTg1LjkzNiA4IDEuNzgzIi8+Cjwvc3ZnPg==&style=for-the-badge)](https://docs.jcdc.dev/jcdcdev-umbraco-readingtime/latest)
[![Umbraco Marketplace](https://img.shields.io/badge/Umbraco%20Marketplace-%23f5c1bc?logo=umbraco&logoColor=162335&style=for-the-badge)](https://marketplace.umbraco.com/package/jcdcdev.Umbraco.ReadingTime)
[![GitHub](https://img.shields.io/badge/GitHub-1?logo=github&color=232925&style=for-the-badge)](https://github.com/jcdcdev/jcdcdev.Umbraco.ReadingTime)
[![NuGet Downloads](https://img.shields.io/nuget/dt/jcdcdev.Umbraco.ReadingTime?labelColor=4536d3&color=4536d3&label=NuGet&logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/jcdcdev.Umbraco.ReadingTime)
[![Project Website](https://img.shields.io/badge/Project%20Website-jcdcdev?color=3c4834&logo=data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNiIgaGVpZ2h0PSIxNiIgZmlsbD0id2hpdGUiIGNsYXNzPSJiaSBiaS1wYy1kaXNwbGF5IiB2aWV3Qm94PSIwIDAgMTYgMTYiPgogIDxwYXRoIGQ9Ik04IDFhMSAxIDAgMCAxIDEtMWg2YTEgMSAwIDAgMSAxIDF2MTRhMSAxIDAgMCAxLTEgMUg5YTEgMSAwIDAgMS0xLTF6bTEgMTMuNWEuNS41IDAgMSAwIDEgMCAuNS41IDAgMCAwLTEgMG0yIDBhLjUuNSAwIDEgMCAxIDAgLjUuNSAwIDAgMC0xIDBNOS41IDFhLjUuNSAwIDAgMCAwIDFoNWEuNS41IDAgMCAwIDAtMXpNOSAzLjVhLjUuNSAwIDAgMCAuNS41aDVhLjUuNSAwIDAgMCAwLTFoLTVhLjUuNSAwIDAgMC0uNS41TTEuNSAyQTEuNSAxLjUgMCAwIDAgMCAzLjV2N0ExLjUgMS41IDAgMCAwIDEuNSAxMkg2djJoLS41YS41LjUgMCAwIDAgMCAxSDd2LTRIMS41YS41LjUgMCAwIDEtLjUtLjV2LTdhLjUuNSAwIDAgMSAuNS0uNUg3VjJ6Ii8+Cjwvc3ZnPg==&style=for-the-badge)](https://jcdc.dev/umbraco-packages/reading-time)


Custom Data Type for calculating reading time. With full variant support!

The following editors are currently supported:

- Rich Text
- Markdown
- Block Grid
- Block List
- Nested Content
- Textstring
- Textarea

> [!IMPORTANT]
> Version 16 will only receive security updates and no new features.

> Please review the [security policy](https://github.com/jcdcdev/jcdcdev.Umbraco.ReadingTime?tab=security-ov-file#supported-versions) for more information.

## Installation

### Install Package

```csharp
dotnet add package jcdcdev.Umbraco.ReadingTime 
```

## Configuration

You can change the average words per minute in the data type settings.

When creating a new data type, the default will be 200 words per minute.

## Security

> [!NOTE]
> This project takes security and support seriously.
> Please visit the [Security](https://github.com/jcdcdev/jcdcdev.Umbraco.ReadingTime?tab=security-ov-file) page for more information.



## Contributing

Contributions to this package are most welcome! Please visit the [Contributing](https://github.com/jcdcdev/jcdcdev.Umbraco.ReadingTime/contribute) page.

## Acknowledgements

Thank you to the following projects and individuals for their contributions. High five, you rock! 🤘🦄

- LottePitcher - [opinionated-package-starter](https://github.com/LottePitcher/opinionated-package-starter)
- Matthew-Wise - [Matthew-Wise](https://github.com/Matthew-Wise)
- Miguel Guedelha - [Miguel Guedelha](https://github.com/MiguelGuedelha)



