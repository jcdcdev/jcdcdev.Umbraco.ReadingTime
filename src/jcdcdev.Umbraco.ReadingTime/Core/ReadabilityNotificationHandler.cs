using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;

namespace jcdcdev.Umbraco.ReadingTime.Core;

public class ReadingTimeNotificationHandler :
    INotificationAsyncHandler<ContentSavedNotification>,
    INotificationAsyncHandler<SendingContentNotification>,
    INotificationAsyncHandler<ContentDeletingNotification>
{
    private readonly IReadingTimeService _ReadingTimeService;

    public ReadingTimeNotificationHandler(IReadingTimeService ReadingTimeService)
    {
        _ReadingTimeService = ReadingTimeService;
    }

    public async Task HandleAsync(ContentDeletingNotification notification, CancellationToken cancellationToken)
    {
        foreach (var content in notification.DeletedEntities)
        {
            await _ReadingTimeService.DeleteAsync(content.Key);
        }
    }

    public async Task HandleAsync(ContentSavedNotification notification, CancellationToken cancellationToken)
    {
        foreach (var item in notification.SavedEntities)
        {
            await _ReadingTimeService.Process(item);
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

        var model = await _ReadingTimeService.GetAsync(notification.Content.Key.Value);

        foreach (var property in properties)
        {
            if (model == null)
            {
                property.Value = "Save and publish to generate reading time";
                continue;
            }

            var culture = property.Culture;
            var value = model.Value(culture);
            if (value == null)
            {
                property.Value = "Save and publish to generate reading time";
                continue;
            }

            property.Value =
                $"""
                 <span class="bold">Reading Time:</span> {value.DisplayTime()}
                 """;
        }
    }
}