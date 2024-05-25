using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace jcdcdev.Umbraco.ReadingTime.Core;

public class ReadingTimeNotificationHandler :
    INotificationAsyncHandler<ContentSavedNotification>,
    INotificationAsyncHandler<ContentDeletingNotification>
{
    private readonly ILocalizedTextService _localizedTextService;
    private readonly IReadingTimeService _readingTimeService;

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
}
