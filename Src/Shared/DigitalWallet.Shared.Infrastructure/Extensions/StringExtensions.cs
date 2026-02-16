using System.Text.RegularExpressions;

namespace DigitalWallet.Shared.Infrastructure.Extensions;

public static partial class StringExtensions
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var startUnderscore = MyRegex().Replace(input, "$1_$2");
        return startUnderscore.ToLowerInvariant();
    }

    [GeneratedRegex(@"([a-z0-9])([A-Z])")]
    private static partial Regex MyRegex();
}