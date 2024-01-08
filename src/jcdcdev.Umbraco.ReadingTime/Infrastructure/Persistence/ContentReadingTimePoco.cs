using jcdcdev.Umbraco.ReadingTime.Core;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;
using Umbraco.Cms.Infrastructure.Persistence.Dtos;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Persistence;

[TableName(Constants.TableName)]
[PrimaryKey("id")]
[ExplicitColumns]
public class ContentReadingTimePoco
{
    [Column(Name = "id")]
    [PrimaryKeyColumn]
    public int Id { get; set; }

    [Column(Name = "key")]
    [ForeignKey(typeof(NodeDto), Column = "uniqueId")]
    [NullSetting(NullSetting = NullSettings.NotNull)]
    public Guid Key { get; set; }

    [Column("data")]
    [NullSetting(NullSetting = NullSettings.Null)]
    [SpecialDbType(SpecialDbTypes.NVARCHARMAX)]
    public string? TextData { get; set; }
}