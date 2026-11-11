using LifeManagementApp.Models;
using LifeManagementApp.Services;
using LifeManagementApp.ViewModels;
using Xunit;

namespace LifeManagementApp.Tests;

public class DomainAndLogicTests
{
    [Theory]
    [InlineData(5.0, false)]
    [InlineData(5.1, true)]
    [InlineData(12.4, true)]
    [InlineData(2.6, false)]
    public void IsKitesurfWeather_ReturnsTrueOnlyWhenWindExceedsFive(double windSpeed, bool expected)
    {
        // Arrange
        var weather = new WeatherInfo
        {
            Temperature = 15.0,
            WindSpeed = windSpeed
        };

        // Act
        bool result = weather.IsKitesurfWeather;

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TopThreeSorting_ReturnsHighestPriorityAndEarliestDueDate_ExcludingDone()
    {
        // Arrange
        var todos = new List<TodoItem>
        {
            new() { Title = "Tehty korkea", Priority = 5, DueDate = DateTime.Today, IsDone = true },
            new() { Title = "Matala", Priority = 1, DueDate = DateTime.Today, IsDone = false },
            new() { Title = "Keski myöhäinen", Priority = 3, DueDate = DateTime.Today.AddDays(5), IsDone = false },
            new() { Title = "Keski aikainen", Priority = 3, DueDate = DateTime.Today.AddDays(1), IsDone = false },
            new() { Title = "Korkein", Priority = 5, DueDate = DateTime.Today.AddDays(2), IsDone = false }
        };

        // Act
        var topThree = todos
            .Where(t => !t.IsDone)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .Take(3)
            .ToList();

        // Assert
        Assert.Equal(3, topThree.Count);
        Assert.Equal("Korkein", topThree[0].Title);
        Assert.Equal("Keski aikainen", topThree[1].Title);
        Assert.Equal("Keski myöhäinen", topThree[2].Title);
    }

    [Fact]
    public async Task AddTodoViewModel_EmptyTitle_SetsValidationError()
    {
        // Arrange
        var viewModel = new AddTodoViewModel(new FakeTodoRepository())
        {
            Title = "   "
        };

        // Act
        await viewModel.SaveCommand.ExecuteAsync(null);

        // Assert
        Assert.True(viewModel.HasValidationError);
        Assert.False(string.IsNullOrWhiteSpace(viewModel.ValidationError));
    }

    private sealed class FakeTodoRepository : ITodoRepository
    {
        public Task<List<TodoItem>> GetAllTodosAsync() => Task.FromResult(new List<TodoItem>());
        public Task<TodoItem?> GetTodoByIdAsync(int id) => Task.FromResult<TodoItem?>(null);
        public Task AddTodoAsync(TodoItem item) => Task.CompletedTask;
        public Task UpdateTodoAsync(TodoItem item) => Task.CompletedTask;
        public Task DeleteTodoAsync(TodoItem item) => Task.CompletedTask;
        public Task<List<Category>> GetCategoriesAsync() => Task.FromResult(new List<Category>());
    }
}