﻿using jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Indexing;

public class LegacyNestedContentReadingTimeValueProvider(IContentTypeService contentTypeService) : ReadingTimeValueProviderBase
{
    private readonly DefaultPropertyIndexValueFactory _converter = new();

    public override bool CanConvert(IPropertyType type) => type.PropertyEditorAlias is Constants.PropertyEditors.Aliases.NestedContent;

    public override TimeSpan? GetReadingTime(IProperty property, string? culture, string? segment, IEnumerable<string> availableCultures, ReadingTimeConfiguration config)
    {
        // TODO - Improve this
        var contentTypeDictionary = contentTypeService.GetAll().ToDictionary(x => x.Key, x => x);
        var values = _converter.GetIndexValues(property, culture, segment, true, availableCultures, contentTypeDictionary);
        return ProcessIndexValues(values, config.WordsPerMinute);
    }
}
