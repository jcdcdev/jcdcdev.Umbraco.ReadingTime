using jcdcdev.Umbraco.ReadingTime.Core;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;
using Umbraco.Cms.Infrastructure.Persistence.Dtos;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Persistence;

[TableName(Constants.TableName)]
[PrimaryKey("id")]
[ExplicitColumns]
public class ReadingTimePoco
{
    [Column(Name = "id")]
    [PrimaryKeyColumn]
    public int Id { get; set; }

    [Column(Name = "key")]
    [ForeignKey(typeof(NodeDto), Column = "uniqueId", Name = "FK_jcdcdevReadingTime_content_umbracoNode_uniqueId")]
    [NullSetting(NullSetting = NullSettings.NotNull)]
    public Guid Key { get; set; }

    [Column(Name = "dataTypeKey")]
    [ForeignKey(typeof(NodeDto), Column = "uniqueId", Name = "FK_jcdcdevReadingTime_dataTypeKey_umbracoNode_uniqueId")]
    [NullSetting(NullSetting = NullSettings.NotNull)]
    public Guid DataTypeKey { get; set; }

    [Column("data")]
    [NullSetting(NullSetting = NullSettings.Null)]
    [SpecialDbType(SpecialDbTypes.NVARCHARMAX)]
    public string? TextData { get; set; }

    [Column(Name = "dataTypeId")]
    [ForeignKey(typeof(NodeDto), Column = "id", Name = "FK_jcdcdevReadingTime_dataTypeId_umbracoNode_uniqueId")]
    [NullSetting(NullSetting = NullSettings.NotNull)]
    public int DataTypeId { get; set; }
}
