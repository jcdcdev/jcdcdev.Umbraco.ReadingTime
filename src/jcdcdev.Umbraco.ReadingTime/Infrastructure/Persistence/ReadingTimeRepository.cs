using System.Text.Json;
using jcdcdev.Umbraco.ReadingTime.Core.Models;
using Umbraco.Cms.Core.Models;
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
            .Delete<ReadingTimePoco>()
            .Where<ReadingTimePoco>(x => x.Key == key);

        var data = scope.Database.Execute(sql);

        scope.Complete();

        return Task.FromResult(data);
    }

    public async Task<ReadingTimeDto> GetOrCreate(Guid key, IDataType dataType)
    {
        var dto = await Get(key, dataType.Id);
        if (dto != null)
        {
            return dto;
        }

        return new ReadingTimeDto
        {
            Key = key,
            DataTypeId = dataType.Id,
            DataTypeKey = dataType.Key
        };
    }

    public async Task PersistAsync(ReadingTimeDto dto)
    {
        var poco = new ReadingTimePoco
        {
            Id = dto.Id,
            Key = dto.Key,
            TextData = JsonSerializer.Serialize(dto.Data),
            DataTypeId = dto.DataTypeId,
            DataTypeKey = dto.DataTypeKey
        };

        using var scope = _scopeProvider.CreateScope();

        await scope.Database.SaveAsync(poco);

        scope.Complete();
    }

    private static ReadingTimeDto? Map(ReadingTimePoco? result)
    {
        var record = result;
        if (record == null)
        {
            return null;
        }

        var data = new List<ReadingTimeVariantDto?>();
        if (!record.TextData.IsNullOrWhiteSpace())
        {
            var attempt = JsonSerializer.Deserialize<List<ReadingTimeVariantDto?>>(record.TextData);
            if (attempt != null)
            {
                data = attempt;
            }
        }

        return new ReadingTimeDto
        {
            Id = record.Id,
            Key = record.Key,
            DataTypeId = record.DataTypeId,
            DataTypeKey = record.DataTypeKey,
            Data = data
        };
    }

    public async Task<ReadingTimeDto?> Get(Guid key, int dataTypeId)
    {
        using var scope = _scopeProvider.CreateScope();

        var sql = scope.SqlContext.Sql()
            .Select<ReadingTimePoco>()
            .From<ReadingTimePoco>()
            .Where<ReadingTimePoco>(x => x.Key == key && x.DataTypeId == dataTypeId);

        var result = await scope.Database.FetchAsync<ReadingTimePoco>(sql);

        scope.Complete();

        return Map(result.FirstOrDefault());
    }

    public async Task<ReadingTimeDto?> Get(Guid key, Guid dataTypeKey)
    {
        using var scope = _scopeProvider.CreateScope();

        var sql = scope.SqlContext.Sql()
            .Select<ReadingTimePoco>()
            .From<ReadingTimePoco>()
            .Where<ReadingTimePoco>(x => x.Key == key && x.DataTypeKey == dataTypeKey);

        var result = await scope.Database.FetchAsync<ReadingTimePoco>(sql);

        scope.Complete();

        return Map(result.FirstOrDefault());
    }
}
