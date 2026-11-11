using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using LifeManagementApp.Models;

namespace LifeManagementApp.Services;

public class OpenMeteoWeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;

    public OpenMeteoWeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherInfo> GetCurrentWeatherAsync(double latitude = 63.10, double longitude = 21.62)
    {
        // InvariantCulture varmistaa pisteen desimaalierottimeksi (esim. 63.10 eikä 63,10)
        string lat = latitude.ToString(CultureInfo.InvariantCulture);
        string lon = longitude.ToString(CultureInfo.InvariantCulture);

        // HUOM: wind_speed_unit=ms on kriittinen, muuten Open-Meteo palauttaa km/h!
        string url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m,wind_speed_10m&wind_speed_unit=ms";

        var response = await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(url);

        if (response?.Current is null)
        {
            throw new Exception("Säätietoja ei saatu jäsennettyä palvelimen vastauksesta.");
        }

        return new WeatherInfo
        {
            Temperature = response.Current.Temperature,
            WindSpeed = response.Current.WindSpeed
        };
    }

    // Sisäiset apuluokat Open-Meteon JSON-rakenteen lukemiseen
    private class OpenMeteoResponse
    {
        [JsonPropertyName("current")]
        public CurrentWeatherData? Current { get; set; }
    }

    private class CurrentWeatherData
    {
        [JsonPropertyName("temperature_2m")]
        public double Temperature { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public double WindSpeed { get; set; }
    }
}