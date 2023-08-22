using Microsoft.IdentityModel.Tokens;

namespace UserAPI.Utils
{
    public static class SimpleExtensions
    {
        public static bool Matches(this string value, string match, bool caseSensitive = false) =>
            (value.IsNullOrEmpty() && match.IsNullOrEmpty()) ||
            (!value.IsNullOrEmpty() && !match.IsNullOrEmpty() &&
                value.Trim().Equals(match.Trim(), caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase));
    }
}
