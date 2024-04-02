using jcdcdev.Umbraco.ReadingTime.Core.Models;
using Umbraco.Cms.Core.Models;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Persistence;

public interface IReadingTimeRepository
{
    Task<int> DeleteAsync(Guid key);
    Task<ReadingTimeDto> GetOrCreate(Guid key, IDataType dataType);
    Task PersistAsync(ReadingTimeDto dto);
    Task<ReadingTimeDto?> Get(Guid key, int dataTypeId);
    Task<ReadingTimeDto?> Get(Guid key, Guid dataTypeKey);
}
