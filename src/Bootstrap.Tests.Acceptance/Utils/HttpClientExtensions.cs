using System.Text;
using System.Text.Json;

namespace Bootstrap.Tests.Acceptance.Utils;

public static class HttpClientExtensions
{
    public static async Task Post(this HttpClient client, string path, object body)
    {
        var json = JsonSerializer.Serialize(body);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(path, content);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();

            throw new Exception($"{response.StatusCode} on {path} : {result}");
        }
    }

    public static async Task<T> Get<T>(this HttpClient client, string path)
    {
        var json = await client.GetStringAsync(path);

        var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

        return JsonSerializer.Deserialize<T>(json, options) ?? throw new InvalidOperationException($"Unable to deserialize the response to {typeof(T)}");
    }
}