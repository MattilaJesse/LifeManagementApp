using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagementApp.Models;
using LifeManagementApp.Services;

namespace LifeManagementApp.ViewModels;

public partial class TodoListViewModel : ObservableObject
{
    private readonly ITodoRepository _todoRepository;

    [ObservableProperty]
    private ObservableCollection<TodoItem> _todos = new();

    [ObservableProperty]
    private string _sortBy = "Priority"; // "Priority" tai "DueDate"

    public TodoListViewModel(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    [RelayCommand]
    public async Task LoadTodosAsync()
    {
        var items = await _todoRepository.GetAllTodosAsync();

        IEnumerable<TodoItem> sorted = SortBy == "DueDate"
            ? items.OrderBy(t => t.IsDone).ThenBy(t => t.DueDate).ThenByDescending(t => t.Priority)
            : items.OrderBy(t => t.IsDone).ThenByDescending(t => t.Priority).ThenBy(t => t.DueDate);

        Todos = new ObservableCollection<TodoItem>(sorted);
    }

    [RelayCommand]
    private async Task SortByPriorityAsync()
    {
        SortBy = "Priority";
        await LoadTodosAsync();
    }

    [RelayCommand]
    private async Task SortByDueDateAsync()
    {
        SortBy = "DueDate";
        await LoadTodosAsync();
    }

    [RelayCommand]
    private async Task ToggleDoneAsync(TodoItem? item)
    {
        if (item is null) return;

        item.IsDone = !item.IsDone;
        await _todoRepository.UpdateTodoAsync(item);
        await LoadTodosAsync();
    }

    [RelayCommand]
    private async Task DeleteTodoAsync(TodoItem? item)
    {
        if (item is null) return;

        await _todoRepository.DeleteTodoAsync(item);
        Todos.Remove(item);
    }

    [RelayCommand]
    private async Task GoToAddTodoAsync()
    {
        // Siirrytään lisäyssivulle (TodoId = 0 tarkoittaa uutta tehtävää)
        await Shell.Current.GoToAsync("AddTodoPage");
    }

    [RelayCommand]
    private async Task GoToEditTodoAsync(TodoItem? item)
    {
        if (item is null) return;

        // Osa-alue 2 (5/5 p): Parametrinvälitys sivulta toiselle Shell-reitityksellä
        await Shell.Current.GoToAsync($"AddTodoPage?TodoId={item.Id}");
    }
}