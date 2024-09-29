using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weather", async (string? zip, IHttpClientFactory httpClientFactory) =>
{
    if (string.IsNullOrEmpty(zip))
    {
        return Results.BadRequest("Zip Code Required.");
    }
    
    try
    {
        var httpClient = httpClientFactory.CreateClient();
        string weatherData = await GetWeatherDataAsync(zip, httpClient);
        return Results.Ok(weatherData);
    }
    catch (Exception ex)
    {
        // log.LogError($"Error: {ex.Message}");
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
})
.WithName("Weather")
.WithOpenApi();

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/hello", () =>
{
    return "hello world!";
})
.WithName("Hello")
.WithOpenApi();

app.Run();


static async Task<string> GetWeatherDataAsync(string zipCode, HttpClient httpClient)
    {
       string WeatherApiKey = "f1713d9f5c23f3ad64eaafe3a9060c95";
       string WeatherApi = "http://api.openweathermap.org/data/2.5/weather";
       string requestUrl = $"{WeatherApi}?zip={zipCode},us&appid={WeatherApiKey}&units=imperial";
       //var httpClient = new HttpClient();
       HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
       response.EnsureSuccessStatusCode();
       string responseBody = await response.Content.ReadAsStringAsync();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(responseBody);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    return $"Current weather in {weatherResponse.name}: {weatherResponse.main.temp} {weatherResponse.weather[0].description}";
    }
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

