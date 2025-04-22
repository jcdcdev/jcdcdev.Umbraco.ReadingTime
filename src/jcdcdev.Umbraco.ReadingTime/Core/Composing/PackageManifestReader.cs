using System.Reflection;
using jcdcdev.Umbraco.Core.Extensions;
using jcdcdev.Umbraco.Core.Web.Models.Manifests;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Infrastructure.Manifest;

namespace jcdcdev.Umbraco.ReadingTime.Core.Composing;

public class PackageManifestReader : IPackageManifestReader
{
    public Task<IEnumerable<PackageManifest>> ReadPackageManifestsAsync()
    {
        var extensions = new List<IManifest>();
        var packageManifest = new PackageManifest
        {
            Name = Constants.PackageName,
            Version = Assembly.GetAssembly(typeof(PackageManifestReader))?.GetName().Version?.ToSemVer()?.ToString() ?? "0.1.0",
            AllowPublicAccess = false,
            AllowTelemetry = true,
            Extensions = []
        };

        extensions.Add(new EntryPointManifest
        {
            Name = "reading-time.entrypoint",
            Alias = "reading-time.entrypoint",
            Js = "/App_Plugins/jcdcdev.Umbraco.ReadingTime/dist/reading-time.js",
        });

        packageManifest.Extensions = extensions.OfType<object>().ToArray();
        return Task.FromResult<IEnumerable<PackageManifest>>([packageManifest]);
    }
}
