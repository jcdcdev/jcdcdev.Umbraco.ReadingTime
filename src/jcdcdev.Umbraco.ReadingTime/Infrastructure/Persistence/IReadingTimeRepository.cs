using jcdcdev.Umbraco.ReadingTime.Core.Models;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Persistence;

public interface IReadingTimeRepository
{
    Task<int> DeleteAsync(Guid key);
    Task<ContentReadingTimeModel> GetOrCreate(Guid key);
    Task PersistAsync(ContentReadingTimeModel dto);
    Task<ContentReadingTimeModel?> GetByKey(Guid key);
}
