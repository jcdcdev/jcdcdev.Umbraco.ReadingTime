using jcdcdev.Umbraco.ReadingTime.Core.Models;
using Umbraco.Cms.Core.Models;

namespace jcdcdev.Umbraco.ReadingTime.Core;

public interface IReadingTimeService
{
    Task ScanTree(int homeId);
    Task ScanAll();
    Task Process(IContent item);
    Task<int> DeleteAsync(Guid key);
    Task<int> DeleteAsync(IEnumerable<Guid> keys);
    Task<ReadingTimeDto?> GetAsync(Guid key, Guid dataTypeKey);
    Task<ReadingTimeDto?> GetAsync(Guid key, int dataTypeId);
}
