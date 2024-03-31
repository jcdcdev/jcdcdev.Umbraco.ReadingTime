using jcdcdev.Umbraco.ReadingTime.Core.Models;
using Umbraco.Cms.Core.Models;

namespace jcdcdev.Umbraco.ReadingTime.Core;

public interface IReadingTimeService
{
    Task ScanTree(int homeId);
    Task Process(IContent item);
    Task<ContentReadingTimeModel?> GetAsync(Guid key);
    Task<int> DeleteAsync(Guid key);
}
