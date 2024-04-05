using jcdcdev.Umbraco.ReadingTime.Core;
using Umbraco.Cms.Core.Manifest;

namespace jcdcdev.Umbraco.ReadingTime;

internal class ManifestFilter : IManifestFilter
{
    public void Filter(List<PackageManifest> manifests)
    {
        manifests.Add(new PackageManifest
        {
            PackageName = Constants.Package.Name,
            Version = GetType().Assembly.GetName().Version?.ToString(3) ?? "0.1.0",
            AllowPackageTelemetry = true
        });
    }
}
