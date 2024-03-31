using System.Text.Json;
using jcdcdev.Umbraco.ReadingTime.Core.Models;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Persistence;

public class ReadingTimeRepository : IReadingTimeRepository
{
    private readonly IScopeProvider _scopeProvider;

    public ReadingTimeRepository(IScopeProvider scopeProvider)
    {
        _scopeProvider = scopeProvider;
    }

    public Task<int> DeleteAsync(Guid key)
    {
        using var scope = _scopeProvider.CreateScope();

        var sql = scope.SqlContext
            .Sql()
            .Delete<ContentReadingTimePoco>()
            .Where<ContentReadingTimePoco>(x => x.Key == key);

        var data = scope.Database.Execute(sql);

        scope.Complete();

        return Task.FromResult(data);
    }

    public async Task<ContentReadingTimeModel> GetOrCreate(Guid key)
    {
        var dto = await GetByKey(key);
        if (dto != null)
        {
            return dto;
        }

        return new ContentReadingTimeModel
        {
            Key = key
        };
    }

    public async Task PersistAsync(ContentReadingTimeModel dto)
    {
        var poco = new ContentReadingTimePoco
        {
            Id = dto.Id,
            Key = dto.Key,
            TextData = JsonSerializer.Serialize(dto.Data)
        };

        using var scope = _scopeProvider.CreateScope();

        await scope.Database.SaveAsync(poco);

        scope.Complete();
    }

    public async Task<ContentReadingTimeModel?> GetByKey(Guid key)
    {
        using var scope = _scopeProvider.CreateScope();

        var sql = scope.SqlContext.Sql()
            .Select<ContentReadingTimePoco>()
            .From<ContentReadingTimePoco>()
            .Where<ContentReadingTimePoco>(x => x.Key == key);

        var result = await scope.Database.FetchAsync<ContentReadingTimePoco>(sql);

        scope.Complete();

        var record = result.FirstOrDefault();
        if (record == null)
        {
            return null;
        }

        var data = new List<ReadingTimeVariantModel?>();
        if (!record.TextData.IsNullOrWhiteSpace())
        {
            var attempt = JsonSerializer.Deserialize<List<ReadingTimeVariantModel?>>(record.TextData);
            if (attempt != null)
            {
                data = attempt;
            }
        }

        return new ContentReadingTimeModel
        {
            Id = record.Id,
            Key = record.Key,
            Data = data
        };
    }
}
