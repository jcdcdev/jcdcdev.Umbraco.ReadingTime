using jcdcdev.Umbraco.ReadingTime.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace jcdcdev.Umbraco.ReadingTime.Web;

public class ConfigApiSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.SwaggerDoc(Constants.Api.ApiName,
            new OpenApiInfo
            {
                Title = Constants.Api.Title,
                Version = "Latest",
                Description = Constants.Api.Description
            });
    }
}
