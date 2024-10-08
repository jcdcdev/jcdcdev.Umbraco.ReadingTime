using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;

namespace jcdcdev.Umbraco.ReadingTime.Core;

public class ReadingTimeNotificationHandler(
    IReadingTimeService readingTimeService) :
    INotificationAsyncHandler<ContentPublishedNotification>,
    INotificationAsyncHandler<ContentDeletingNotification>
{
    public async Task HandleAsync(ContentDeletingNotification notification, CancellationToken cancellationToken)
    {
        foreach (var content in notification.DeletedEntities)
        {
            await readingTimeService.DeleteAsync(content.Key);
        }
    }

    public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken)
    {
        foreach (var item in notification.PublishedEntities)
        {
            await readingTimeService.Process(item);
        }
    }
}
