using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace jcdcdev.Umbraco.ReadingTime.Core;

public class ReadingTimeNotificationHandler(IReadingTimeService calculationService)
    : INotificationAsyncHandler<ContentSavingNotification>
{
    public async Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken)
    {
        foreach (var content in notification.SavedEntities)
        {
            await calculationService.CalculateAndSetReadingTime(content);
        }
    }
}
