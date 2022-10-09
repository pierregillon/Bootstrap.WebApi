using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<Guid> Post(string path, object body) =>
        await _errorDriver.TryExecute(
            async () =>
            {
                var response = await HttpClient.PostAsync(path, ToStringContent(body));

                await ProcessError(path, response);

                var json = await response.Content.ReadAsStringAsync();

                return Deserialize<Guid>(json);
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

        return Deserialize<T>(json);
    }

    private static T Deserialize<T>(string json)
    {
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
            var problemDetailsJson = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(problemDetailsJson))
            {
                throw HttpException.From(path, response.StatusCode);
            }
            else
            {
                var problemDetails = Deserialize<ProblemDetails>(problemDetailsJson);

                throw HttpException.From(path, problemDetails);
            }
        }
    }
}
