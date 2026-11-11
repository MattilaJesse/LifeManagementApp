namespace LifeManagementApp.Models;

public class WeatherInfo
{
    public double Temperature { get; set; }
    public double WindSpeed { get; set; }

    // Kitesurf-huomio laukeaa, kun tuuli on yli 5 m/s
    public bool IsKitesurfWeather => WindSpeed > 5.0;
}