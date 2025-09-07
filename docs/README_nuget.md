# jcdcdev.Umbraco.ReadingTime

[![Documentation](https://img.shields.io/badge/Docs-Quickstart-394933?style=flat&logo=github)](https://github.com/jcdcdev/jcdcdev.Umbraco.ReadingTime/#quick-start)
[![Umbraco Marketplace](https://img.shields.io/badge/Umbraco-Marketplace-%233544B1?style=flat&logo=umbraco)](https://marketplace.umbraco.com/package/jcdcdev.Umbraco.ReadingTime)
[![License](https://img.shields.io/github/license/jcdcdev/jcdcdev.Umbraco.ReadingTime?color=8AB803&label=License&logo=github)](https://github.com/jcdcdev/jcdcdev.Umbraco.ReadingTime?tab=MIT-1-ov-file)
[![NuGet Downloads](https://img.shields.io/nuget/dt/jcdcdev.Umbraco.ReadingTime?color=cc9900&label=Downloads&logo=nuget)](https://www.nuget.org/packages/jcdcdev.Umbraco.ReadingTime)
[![Project Website](https://img.shields.io/badge/Project%20Website-jcdc.dev-jcdcdev?style=flat&color=3c4834&logo=data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNiIgaGVpZ2h0PSIxNiIgZmlsbD0id2hpdGUiIGNsYXNzPSJiaSBiaS1wYy1kaXNwbGF5IiB2aWV3Qm94PSIwIDAgMTYgMTYiPgogIDxwYXRoIGQ9Ik04IDFhMSAxIDAgMCAxIDEtMWg2YTEgMSAwIDAgMSAxIDF2MTRhMSAxIDAgMCAxLTEgMUg5YTEgMSAwIDAgMS0xLTF6bTEgMTMuNWEuNS41IDAgMSAwIDEgMCAuNS41IDAgMCAwLTEgMG0yIDBhLjUuNSAwIDEgMCAxIDAgLjUuNSAwIDAgMC0xIDBNOS41IDFhLjUuNSAwIDAgMCAwIDFoNWEuNS41IDAgMCAwIDAtMXpNOSAzLjVhLjUuNSAwIDAgMCAuNS41aDVhLjUuNSAwIDAgMCAwLTFoLTVhLjUuNSAwIDAgMC0uNS41TTEuNSAyQTEuNSAxLjUgMCAwIDAgMCAzLjV2N0ExLjUgMS41IDAgMCAwIDEuNSAxMkg2djJoLS41YS41LjUgMCAwIDAgMCAxSDd2LTRIMS41YS41LjUgMCAwIDEtLjUtLjV2LTdhLjUuNSAwIDAgMSAuNS0uNUg3VjJ6Ii8+Cjwvc3ZnPg==)](https://jcdc.dev/umbraco-packages/reading-time)


Custom Data Type for calculating reading time. With full variant support!

The following editors are currently supported:

- Rich Text
- Markdown
- Block Grid
- Block List
- Nested Content
- Textstring
- Textarea

## Quick Start

1. Install the [NuGet package](https://www.nuget.org/packages/jcdcdev.Umbraco.ReadingTime) in your Umbraco CMS website project.
   ```
    dotnet add package jcdcdev.Umbraco.ReadingTime
   ```
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

## Contributing

Contributions to this package are most welcome! Please visit the [Contributing](https://github.com/jcdcdev/jcdcdev.Umbraco.ReadingTime/contribute) page.

## Acknowledgements (Thanks)

- LottePitcher - [opinionated-package-starter](https://github.com/LottePitcher/opinionated-package-starter)



