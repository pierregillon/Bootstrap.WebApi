using System.Text;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace Bootstrap.Tests.Acceptance.Utils;

public class TestClient
{
    private readonly ScenarioContext _context;
    private readonly ErrorDriver _errorDriver;

    protected TestClient(ScenarioContext context, ErrorDriver errorDriver)
    {
        _context = context;
        _errorDriver = errorDriver;
    }

    private HttpClient HttpClient => _context.Get<HttpClient>();

    public async Task Post(string path, object body) =>
        await _errorDriver.TryExecute(
            async () =>
            {
                var response = await HttpClient.PostAsync(path, ToStringContent(body));

                await ProcessError(path, response);
            });

    public async Task Put(string path, object body) =>
        await _errorDriver.TryExecute(
            async () =>
            {
                var response = await HttpClient.PutAsync(path, ToStringContent(body));

                await ProcessError(path, response);
            }
        );

    public async Task<T> Get<T>(string path)
    {
        var json = await HttpClient.GetStringAsync(path);

        var options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};

        return JsonSerializer.Deserialize<T>(json, options) ??
               throw new InvalidOperationException($"Unable to deserialize the response to {typeof(T)}");
    }

    private static StringContent ToStringContent(object body)
    {
        var json = JsonSerializer.Serialize(body);

        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private static async Task ProcessError(string path, HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();

            throw new Exception($"{response.StatusCode} on {path} : {result}");
        }
    }
}
