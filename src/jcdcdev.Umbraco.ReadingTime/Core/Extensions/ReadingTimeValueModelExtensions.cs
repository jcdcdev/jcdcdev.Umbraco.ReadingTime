using System.Diagnostics.CodeAnalysis;
using jcdcdev.Umbraco.ReadingTime.Core.Models;

namespace jcdcdev.Umbraco.ReadingTime.Core.Extensions;

public static class ReadingTimeValueModelExtensions
{
    public static bool IsValid([NotNullWhen(true)] this ReadingTimeValueModel? model) => model is not null && model.ReadingTime > TimeSpan.Zero;
}
