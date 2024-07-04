# jcdcdev.Umbraco.ReadingTime

[![Umbraco Marketplace](https://img.shields.io/badge/Umbraco-Marketplace-%233544B1?style=flat&logo=umbraco)](https://marketplace.umbraco.com/package/jcdcdev.umbraco.readingtime)
[![GitHub License](https://img.shields.io/github/license/jcdcdev/jcdcdev.Umbraco.ReadingTime?color=8AB803&label=License&logo=github)](https://github.com/jcdcdev/jcdcdev.Umbraco.ReadingTime/blob/main/LICENSE)
[![NuGet Downloads](https://img.shields.io/nuget/dt/jcdcdev.Umbraco.ReadingTime?color=cc9900&label=Downloads&logo=nuget)](https://www.nuget.org/packages/jcdcdev.Umbraco.ReadingTime/)
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
     ![A screenshot of the BackOffice showing Reading Time data type](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Umbraco.ReadingTime/main/docs/screenshots/datatype.png)
3. Save and publish content.
4. Reading Time will display in the backoffice

   ![A screenshot of the BackOffice showing Reading Time](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Umbraco.ReadingTime/main/docs/screenshots/backoffice.png)

## Using the value in your templates

In your template, you can accessing the Reading Time property value like any other property:

```html
 @Model.ReadingTime.DisplayTime()
```

![A screenshot of page showing Reading Time](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Umbraco.ReadingTime/main/docs/screenshots/displaytime.png)

### Overriding the default display
The `DisplayTime` method will format the reading time as a string using [Humanizer](https://github.com/Humanizr/Humanizer). This supports variants, meaning the reading time will be displayed based on the pluralisation rules of the current culture (e.g. "1 minute", "2 minutes", "0 minuter").

Min and max `TimeUnit` values are derived from the Data Type settings. The below example shows how you can ensure only seconds are displayed.

```csharp
 Model.ReadingTime.DisplayTime(minUnit: TimeUnit.Second, maxUnit: TimeUnit.Second)
```

## Configuration

You can change the average words per minute in the data type settings.

When creating a new data type, the default will be 200 words per minute.

## Limitations

**Values are derived from published content only.** 

Unpublished (saved) content is _not_ included in the calculation.

**Words per minute applies to all variants.**

It is not currently possible to configure words per minute per culture.

## Extending

You can extend the data type to support additional editors by implementing the `IReadingTimeValueProvider` interface.

```csharp
public class MyCustomReadingTimeValueProvider : IReadingTimeValueProvider
{
    public bool CanConvert(IPropertyType type)
    {
        return type.EditorAlias == "MyCustomEditorAlias";
    }

    public TimeSpan? GetReadingTime(IProperty property, string? culture, string? segment, IEnumerable<string> availableCultures, ReadingTimeConfiguration config)
    {
        var value = property.GetValue(culture, segment, true);
        if (value is string text)
        {
            return text.GetReadingTime(config.WordsPerMinute);
        }

        return null;
     }
 }
```

Don't forget to register your custom value provider:

```csharp
public class Composer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.ReadingTimeValueProviders().Append<MyCustomReadingTimeValueProvider>();
    }
}
```

## Contributing

Contributions to this package are most welcome! Please read the [Contributing Guidelines](https://github.com/jcdcdev/jcdcdev.Umbraco.ReadingTime/blob/main/.github/CONTRIBUTING.md).

## Acknowledgments (thanks!)

- LottePitcher - [opinionated-package-starter](https://github.com/LottePitcher/opinionated-package-starter)