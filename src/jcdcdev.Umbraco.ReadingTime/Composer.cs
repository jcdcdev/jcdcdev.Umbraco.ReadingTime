using jcdcdev.Umbraco.ReadingTime.Core.Extensions;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace jcdcdev.Umbraco.ReadingTime;

public class Composer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.AddReadingTime();
    }
}
