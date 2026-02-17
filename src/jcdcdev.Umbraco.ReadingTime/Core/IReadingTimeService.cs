using Umbraco.Cms.Core.Models;

namespace jcdcdev.Umbraco.ReadingTime.Core;

public interface IReadingTimeService
{
    Task CalculateAndSetReadingTime(IContent content);
}
