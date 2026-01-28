# jcdcdev.Umbraco.ReadingTime

[![Umbraco Marketplace](https://img.shields.io/badge/Umbraco%20Marketplace-%23f5c1bc?logo=umbraco&logoColor=162335)](https://marketplace.umbraco.com/package/jcdcdev.Umbraco.ReadingTime)
[![GitHub](https://img.shields.io/badge/GitHub-1?logo=github&color=232925)](https://github.com/jcdcdev/jcdcdev.Umbraco.ReadingTime)
[![NuGet Downloads](https://img.shields.io/nuget/dt/jcdcdev.Umbraco.ReadingTime?labelColor=4536d3&color=4536d3&label=NuGet&logo=nuget)](https://www.nuget.org/packages/jcdcdev.Umbraco.ReadingTime)
[![Project Website](https://img.shields.io/badge/Project%20Website-jcdcdev?color=3c4834&logo=data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNiIgaGVpZ2h0PSIxNiIgZmlsbD0id2hpdGUiIGNsYXNzPSJiaSBiaS1wYy1kaXNwbGF5IiB2aWV3Qm94PSIwIDAgMTYgMTYiPgogIDxwYXRoIGQ9Ik04IDFhMSAxIDAgMCAxIDEtMWg2YTEgMSAwIDAgMSAxIDF2MTRhMSAxIDAgMCAxLTEgMUg5YTEgMSAwIDAgMS0xLTF6bTEgMTMuNWEuNS41IDAgMSAwIDEgMCAuNS41IDAgMCAwLTEgMG0yIDBhLjUuNSAwIDEgMCAxIDAgLjUuNSAwIDAgMC0xIDBNOS41IDFhLjUuNSAwIDAgMCAwIDFoNWEuNS41IDAgMCAwIDAtMXpNOSAzLjVhLjUuNSAwIDAgMCAuNS41aDVhLjUuNSAwIDAgMCAwLTFoLTVhLjUuNSAwIDAgMC0uNS41TTEuNSAyQTEuNSAxLjUgMCAwIDAgMCAzLjV2N0ExLjUgMS41IDAgMCAwIDEuNSAxMkg2djJoLS41YS41LjUgMCAwIDAgMCAxSDd2LTRIMS41YS41LjUgMCAwIDEtLjUtLjV2LTdhLjUuNSAwIDAgMSAuNS0uNUg3VjJ6Ii8+Cjwvc3ZnPg==)](https://jcdc.dev/umbraco-packages/reading-time)


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
> Version 13 will only receive security updates and no new features.

> Please review the [security policy](https://github.com/jcdcdev/jcdcdev.Umbraco.ReadingTime?tab=security-ov-file#supported-versions) for more information.

## Installation

### Install Package

```csharp
dotnet add package jcdcdev.Umbraco.ReadingTime 
```

## Quick Start

1. Install the NuGet package
2. Add the Reading Time data type to a document type. You can configure:
   - `Words per minute` (default is 200)
   - `Min Unit` (default is Minute)
   - `Max Unit` (default is Minute)
3. Save and publish content.
4. Reading Time will display in the backoffice

### Using the value in your templates

In your template, you can accessing the Reading Time property value like any other property:

```html
 @Model.ReadingTime.DisplayTime()
```

## Configuration

You can change the average words per minute in the data type settings.

When creating a new data type, the default will be 200 words per minute.

## Extending

The `DisplayTime` method will format the reading time as a string using [Humanizer](https://github.com/Humanizr/Humanizer). This supports variants, meaning the reading time will be displayed based on the pluralisation rules of the current culture (e.g. "1 minute", "2 minutes", "0 minuter").

Min and max `TimeUnit` values are derived from the Data Type settings. The below example shows how you can ensure only seconds are displayed.

```csharp
 Model.ReadingTime.DisplayTime(minUnit: TimeUnit.Second, maxUnit: TimeUnit.Second)
```

## Contributing

Contributions to this package are most welcome! Please visit the [Contributing](https://github.com/jcdcdev/jcdcdev.Umbraco.ReadingTime/contribute) page.

## Acknowledgements (Thanks)

- LottePitcher - [opinionated-package-starter](https://github.com/LottePitcher/opinionated-package-starter)



