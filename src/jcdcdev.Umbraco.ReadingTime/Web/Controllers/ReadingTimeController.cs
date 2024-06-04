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
public class ReadingTimeController : ControllerBase
{
    private readonly IReadingTimeService _service;
    private readonly IDataTypeService _dataTypeService;

    public ReadingTimeController(IReadingTimeService service, IDataTypeService dataTypeService)
    {
        _service = service;
        _dataTypeService = dataTypeService;
    }

    [HttpGet]
    [Produces(typeof(ReadingTimeResponse))]
    public async Task<IActionResult> Get(string contentKey, string dataTypeKey, string? culture = null)
    {
        Guid.TryParse(contentKey, out var contentGuid);
        Guid.TryParse(dataTypeKey, out var dataTypeGuid);
        var readingTime = await _service.GetAsync(contentGuid, dataTypeGuid);
        if (readingTime == null)
        {
            return NoContent();
        }

        var value = readingTime.Value(culture);
        if (value == null)
        {
            return NoContent();
        }

        var dataType = await _dataTypeService.GetAsync(dataTypeGuid);
        var config = dataType?.ConfigurationAs<ReadingTimeConfiguration>();
        if (config == null)
        {
            return BadRequest();
        }

        var model = new ReadingTimeResponse(value.ReadingTime.DisplayTime(config.Min, config.Max, culture));
        return Ok(model);
    }
}

public class ReadingTimeResponse
{
    public ReadingTimeResponse(string readingTime)
    {
        ReadingTime = readingTime;
    }

    public string ReadingTime { get; }
}
