using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SahoBackend.Swagger;
public class SwaggerConfigurationsOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider) : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider = apiVersionDescriptionProvider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateOpenApiInfo(description));
        }
    }
    private static OpenApiInfo CreateOpenApiInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "SahoBackend",
            Version = description.ApiVersion.ToString()
        };
        return info;
    }
}