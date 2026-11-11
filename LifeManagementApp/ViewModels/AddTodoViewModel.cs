using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagementApp.Models;
using LifeManagementApp.Services;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace LifeManagementApp.ViewModels;

[QueryProperty(nameof(TodoId), "TodoId")]
public partial class AddTodoViewModel : ObservableObject
{
    private readonly ITodoRepository _todoRepository;

    [ObservableProperty]
    private int _todoId;

    [ObservableProperty]
    private string _pageTitle = "Uusi tehtävä";

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string? _notes;

    [ObservableProperty]
    private int _priority = 3;

    [ObservableProperty]
    private DateTime _dueDate = DateTime.Today.AddDays(1);

    [ObservableProperty]
    private bool _isDone;

    [ObservableProperty]
    private ObservableCollection<Category> _categories = new();

    [ObservableProperty]
    private Category? _selectedCategory;

    [ObservableProperty]
    private string? _validationError;

    [ObservableProperty]
    private bool _hasValidationError;

    public AddTodoViewModel(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    // Kun Shell asettaa TodoId-parametrin navigoinnin yhteydessä, ladataan tiedot
    partial void OnTodoIdChanged(int value)
    {
        _ = InitializeAsync();
    }

    public async Task InitializeAsync()
    {
        var cats = await _todoRepository.GetCategoriesAsync();
        Categories = new ObservableCollection<Category>(cats);

        if (TodoId > 0)
        {
            PageTitle = "Muokkaa tehtävää";
            var existing = await _todoRepository.GetTodoByIdAsync(TodoId);
            if (existing is not null)
            {
                Title = existing.Title;
                Notes = existing.Notes;
                Priority = existing.Priority;
                DueDate = existing.DueDate;
                IsDone = existing.IsDone;
                SelectedCategory = Categories.FirstOrDefault(c => c.Id == existing.CategoryId) ?? Categories.FirstOrDefault();
            }
        }
        else
        {
            PageTitle = "Uusi tehtävä";
            SelectedCategory = Categories.FirstOrDefault();
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        // Validointi: tyhjää otsikkoa ei hyväksytä
        if (string.IsNullOrWhiteSpace(Title))
        {
            ValidationError = "Otsikko ei voi olla tyhjä.";
            HasValidationError = true;
            return;
        }

        HasValidationError = false;
        ValidationError = null;

        int categoryId = SelectedCategory?.Id ?? 1;

        if (TodoId > 0)
        {
            var existing = await _todoRepository.GetTodoByIdAsync(TodoId);
            if (existing is not null)
            {
                existing.Title = Title.Trim();
                existing.Notes = Notes?.Trim();
                existing.Priority = Priority;
                existing.DueDate = DueDate;
                existing.IsDone = IsDone;
                existing.CategoryId = categoryId;

                await _todoRepository.UpdateTodoAsync(existing);
            }
        }
        else
        {
            var newItem = new TodoItem
            {
                Title = Title.Trim(),
                Notes = Notes?.Trim(),
                Priority = Priority,
                DueDate = DueDate,
                IsDone = false,
                CategoryId = categoryId
            };

            await _todoRepository.AddTodoAsync(newItem);
        }

        // Palataan edelliselle sivulle
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}