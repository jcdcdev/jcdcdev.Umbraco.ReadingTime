# jcdcdev.Umbraco.ReadingTime

[![Umbraco Version](https://img.shields.io/badge/Umbraco-10.4+-%233544B1?style=flat&logo=umbraco)](https://umbraco.com/products/umbraco-cms/)
[![NuGet](https://img.shields.io/nuget/vpre/jcdcdev.Umbraco.ReadingTime?color=0273B3)](https://www.nuget.org/packages/jcdcdev.Umbraco.ReadingTime)
[![GitHub license](https://img.shields.io/github/license/jcdcdev/jcdcdev.Umbraco.ReadingTime?color=8AB803)](../LICENSE)
[![Downloads](https://img.shields.io/nuget/dt/jcdcdev.Umbraco.ReadingTime?color=cc9900)](https://www.nuget.org/packages/jcdcdev.Umbraco.ReadingTime/)

Custom Data Type for calculating reading time.

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
2. Add the Reading Time data type to a document type.
   - Note: You can configure the words per minute in the data type settings.
3. Save and publish content.
4. Reading Time will display in the backoffice

![A screenshot of the BackOffice showing Reading Time](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Umbraco.ReadingTime/main/docs/screenshots/backoffice.png)

## Using the value in your templates

In your template, you can accessing the Reading Time property.

```csharp
<div class="alert alert-info">
    Read in @Model.ReadingTime.DisplayTime()
</div>
```

## Configuration

You can change the average words per minute in the data type settings.

When creating a new data type, the default will be 200 words per minute. To change this default, adjust your `appsettings.json` file:

```json
{
  "ReadingTime": {
    "WordsPerMinute": 200
  }
}
```

## Limitations

**Values are derived from published content only.** 

Draft content is _not_ included in the calculation.

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

Contributions to this package are most welcome! Please read the [Contributing Guidelines](CONTRIBUTING.md).

## Acknowledgments (thanks!)

- LottePitcher - [opinionated-package-starter](https://github.com/LottePitcher/opinionated-package-starter)