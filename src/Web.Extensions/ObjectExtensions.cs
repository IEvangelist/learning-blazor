using System.Text.Json;
using static System.Text.Json.JsonSerializer;

namespace Learning.Blazor.Extensions
{
    public static class ObjectExtensions
    {
        static readonly JsonSerializerOptions s_options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static string? ToJson<T>(this T value) where T : class =>
            value is null ? null : Serialize(value, s_options);

        public static T? FromJson<T>(this string? json) where T : class =>
            json is null or { Length: 0 } ? default : Deserialize<T>(json, s_options);
    }
}
