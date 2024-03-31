﻿using Humanizer.Localisation;
using jcdcdev.Umbraco.ReadingTime.Core.Extensions;
using jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Core;

public class ReadingTimeNotificationHandler :
    INotificationAsyncHandler<ContentSavedNotification>,
    INotificationAsyncHandler<SendingContentNotification>,
    INotificationAsyncHandler<ContentDeletingNotification>
{
    private readonly IReadingTimeService _readingTimeService;
    private readonly ILocalizedTextService _localizedTextService;

    public ReadingTimeNotificationHandler(IReadingTimeService readingTimeService, ILocalizedTextService localizedTextService)
    {
        _readingTimeService = readingTimeService;
        _localizedTextService = localizedTextService;
    }

    public async Task HandleAsync(ContentDeletingNotification notification, CancellationToken cancellationToken)
    {
        foreach (var content in notification.DeletedEntities)
        {
            await _readingTimeService.DeleteAsync(content.Key);
        }
    }

    public async Task HandleAsync(ContentSavedNotification notification, CancellationToken cancellationToken)
    {
        foreach (var item in notification.SavedEntities)
        {
            await _readingTimeService.Process(item);
        }
    }

    public async Task HandleAsync(SendingContentNotification notification, CancellationToken cancellationToken)
    {
        if (!notification.Content.Key.HasValue)
        {
            return;
        }

        var properties = new List<ContentPropertyDisplay>();
        foreach (var variant in notification.Content.Variants)
        {
            foreach (var tab in variant.Tabs)
            {
                var tabbedProperties = tab.Properties?.ToList() ?? new List<ContentPropertyDisplay>();
                if (!tabbedProperties.Any())
                {
                    continue;
                }

                foreach (var property in tabbedProperties)
                {
                    if (property.PropertyEditor == null)
                    {
                        continue;
                    }

                    if (property.PropertyEditor.Alias != Constants.PropertyEditorAlias)
                    {
                        continue;
                    }

                    properties.Add(property);
                }
            }
        }

        if (!properties.Any())
        {
            return;
        }

        var model = await _readingTimeService.GetAsync(notification.Content.Key.Value);

        foreach (var property in properties)
        {
            if (model == null)
            {
                property.Value = _localizedTextService.Localize(Constants.LocalisationKeys.Area, Constants.LocalisationKeys.SaveAndPublishToGenerateReadingTime);
                continue;
            }

            var culture = property.Culture;
            var value = model.Value(culture);
            if (value == null)
            {
                property.Value = _localizedTextService.Localize(Constants.LocalisationKeys.Area, Constants.LocalisationKeys.SaveAndPublishToGenerateReadingTime);
                continue;
            }

            var config = property.ConfigNullable ?? new Dictionary<string, object?>();
            var min = (TimeUnit)(config.TryGetValue(Constants.Configuration.MinUnit, out var mn) && mn is int minTime ? minTime : ReadingTimeConfiguration.DefaultMinTimeUnit);
            var max = (TimeUnit)(config.TryGetValue(Constants.Configuration.MaxUnit, out var mx) && mx is int maxTime ? maxTime : ReadingTimeConfiguration.DefaultMaxTimeUnit);
            property.Value =
                $"""
                 <span>
                    {value.ReadingTime.DisplayTime(min, max, culture)}
                 </span>
                 """;
        }
    }
}
