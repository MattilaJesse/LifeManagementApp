using LifeManagementApp.Models;

namespace LifeManagementApp.Services;

public interface IWeatherService
{
    Task<WeatherInfo> GetCurrentWeatherAsync(double latitude = 63.10, double longitude = 21.62);
}