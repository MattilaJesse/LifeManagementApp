using LifeManagementApp.Models;
using LifeManagementApp.ViewModels;

namespace LifeManagementApp.Views;

public partial class TodoListPage : ContentPage
{
    private readonly TodoListViewModel _viewModel;
    private bool _isLoading;

    public TodoListPage(TodoListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _isLoading = true;
        await _viewModel.LoadTodosCommand.ExecuteAsync(null);
        _isLoading = false;
    }

    private async void OnTodoCheckBoxChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (_isLoading) return;

        if (sender is CheckBox checkBox && checkBox.BindingContext is TodoItem item)
        {
            // Päivitetään tila tietokantaan
            item.IsDone = e.Value;
            await _viewModel.UpdateTodoStatusCommand.ExecuteAsync(item);
        }
    }
}