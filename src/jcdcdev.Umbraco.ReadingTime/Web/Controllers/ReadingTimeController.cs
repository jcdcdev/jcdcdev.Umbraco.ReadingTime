using jcdcdev.Umbraco.ReadingTime.Core;
using jcdcdev.Umbraco.ReadingTime.Core.Extensions;
using jcdcdev.Umbraco.ReadingTime.Core.Models;
using jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Authorization;
using Umbraco.Cms.Web.Common.Routing;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Web.Controllers;

[ApiController]
[BackOfficeRoute("readingtime/api")]
[Authorize(Policy = AuthorizationPolicies.BackOfficeAccess)]
public class ReadingTimeController(IReadingTimeService service, IDataTypeService dataTypeService) : ControllerBase
{
    [HttpGet]
    [Produces(typeof(ReadingTimeResponse))]
    public async Task<IActionResult> Get(string contentKey, string dataTypeKey, string? culture = null)
    {
        Guid.TryParse(contentKey, out var contentGuid);
        Guid.TryParse(dataTypeKey, out var dataTypeGuid);
        var readingTime = await service.GetAsync(contentGuid, dataTypeGuid);
        if (readingTime == null)
        {
            return NoContent();
        }

        var value = readingTime.Value(culture);
        if (value == null)
        {
            return NoContent();
        }

        var dataType = await dataTypeService.GetAsync(dataTypeGuid);
        var config = dataType?.ConfigurationAs<ReadingTimeConfiguration>();
        if (config == null)
        {
            return BadRequest();
        }

        var model = new ReadingTimeResponse(value.ReadingTime.DisplayTime(config.Min, config.Max, culture), readingTime.UpdateDate);
        return Ok(model);
    }
}

public class ReadingTimeResponse(string readingTime, DateTime updateDate)
{
    public DateTime UpdateDate { get; } = updateDate;
    public string ReadingTime { get; } = readingTime;
}
