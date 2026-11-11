using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagementApp.Models;
using LifeManagementApp.Services;

namespace LifeManagementApp.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly ITodoRepository _todoRepository;
    private readonly IWeatherService _weatherService;

    [ObservableProperty]
    private ObservableCollection<TodoItem> _topTodos = new();

    [ObservableProperty]
    private WeatherInfo? _currentWeather;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _weatherError;

    [ObservableProperty]
    private bool _hasWeatherError;

    public HomeViewModel(ITodoRepository todoRepository, IWeatherService weatherService)
    {
        _todoRepository = todoRepository;
        _weatherService = weatherService;
    }

    [RelayCommand]
    public async Task LoadDataAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            HasWeatherError = false;
            WeatherError = null;

            // 1. Haetaan kaikki todot ja valitaan 3 tärkeintä keskeneräistä:
            // Ensin prioriteetti (suurin ensin), sitten eräpäivä (aikaisin ensin)
            var allTodos = await _todoRepository.GetAllTodosAsync();
            var topThree = allTodos
                .Where(t => !t.IsDone)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .Take(3)
                .ToList();

            TopTodos = new ObservableCollection<TodoItem>(topThree);

            // 2. Haetaan tämänhetkinen sää Vaasasta
            CurrentWeather = await _weatherService.GetCurrentWeatherAsync();
        }
        catch (Exception ex)
        {
            // Osa-alue 7 (5/5 p): Jos verkkoa ei ole, näytetään virheilmoitus eikä kaaduta
            HasWeatherError = true;
            WeatherError = $"Säätietojen haku epäonnistui: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}