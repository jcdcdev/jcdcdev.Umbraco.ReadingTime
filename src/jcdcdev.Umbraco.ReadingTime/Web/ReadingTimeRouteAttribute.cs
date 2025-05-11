using Umbraco.Cms.Web.Common.Routing;

namespace jcdcdev.Umbraco.ReadingTime.Web;

public class ReadingTimeRouteAttribute(string template) : BackOfficeRouteAttribute($"ReadingTime/api/v{{version:apiVersion}}/{template.TrimStart('/')}");
