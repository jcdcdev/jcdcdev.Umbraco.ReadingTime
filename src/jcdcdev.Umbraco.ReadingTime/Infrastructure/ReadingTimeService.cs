using jcdcdev.Umbraco.ReadingTime.Core;
using jcdcdev.Umbraco.ReadingTime.Core.Composing;
using jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure;

public class ReadingTimeService : IReadingTimeService
{
    private readonly ReadingTimeValueProviderCollection _valueProviders;
    private readonly IDataTypeService _dataTypeService;
    private readonly ILogger<ReadingTimeService> _logger;

    public ReadingTimeService(
        ReadingTimeValueProviderCollection valueProviders,
        IDataTypeService dataTypeService,
        ILogger<ReadingTimeService> logger)
    {
        _valueProviders = valueProviders;
        _dataTypeService = dataTypeService;
        _logger = logger;
    }

    public async Task CalculateAndSetReadingTime(IContent content)
    {
        var readingTimeProperties = content.Properties
            .Where(x => x.PropertyType.PropertyEditorAlias == Constants.PropertyEditorAlias)
            .ToList();

        if (readingTimeProperties.Count == 0)
        {
            return;
        }

        foreach (var readingTimeProperty in readingTimeProperties)
        {
            await ProcessReadingTimeProperty(content, readingTimeProperty);
        }
    }

    private async Task ProcessReadingTimeProperty(IContent content, IProperty readingTimeProperty)
    {
        var dataType = await _dataTypeService.GetAsync(readingTimeProperty.PropertyType.DataTypeKey);
        if (dataType == null)
        {
            _logger.LogWarning("DataType not found for property {PropertyAlias}", readingTimeProperty.Alias);
            return;
        }

        var config = dataType.ConfigurationAs<ReadingTimeConfiguration>();
        if (config == null)
        {
            _logger.LogWarning("Configuration not found for property {PropertyAlias}", readingTimeProperty.Alias);
            return;
        }

        var propertyType = readingTimeProperty.PropertyType;
        if (propertyType.VariesByCulture())
        {
            foreach (var culture in content.AvailableCultures)
            {
                var totalSeconds = CalculateTotalSeconds(content, culture, null, config);
                content.SetValue(readingTimeProperty.Alias, totalSeconds, culture);
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Set reading time for {ContentName} ({Culture}): {Seconds}s",
                        content.Name, culture, totalSeconds);
                }
            }
        }
        else
        {
            var totalSeconds = CalculateTotalSeconds(content, null, null, config);
            content.SetValue(readingTimeProperty.Alias, totalSeconds);
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Set reading time for {ContentName}: {Seconds}s", content.Name, totalSeconds);
            }
        }
    }

    private int CalculateTotalSeconds(IContent content, string? culture, string? segment, ReadingTimeConfiguration config)
    {
        var time = TimeSpan.Zero;

        foreach (var property in content.Properties)
        {
            if (property.PropertyType.PropertyEditorAlias == Constants.PropertyEditorAlias)
            {
                continue;
            }

            var provider = _valueProviders.FirstOrDefault(x => x.CanConvert(property.PropertyType));
            if (provider == null)
            {
                continue;
            }

            var propertyCulture = property.PropertyType.VariesByCulture() ? culture : null;
            var propertySegment = property.PropertyType.VariesBySegment() ? segment : null;
            var readingTime = provider.GetReadingTime(property, propertyCulture, propertySegment, content.AvailableCultures, config);
            if (readingTime.HasValue)
            {
                time += readingTime.Value;
            }
        }

        return (int)time.TotalSeconds;
    }
}
