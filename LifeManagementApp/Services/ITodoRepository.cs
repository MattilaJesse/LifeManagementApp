using LifeManagementApp.Models;

namespace LifeManagementApp.Services;

public interface ITodoRepository
{
    Task<List<TodoItem>> GetAllTodosAsync();
    Task<TodoItem?> GetTodoByIdAsync(int id);
    Task AddTodoAsync(TodoItem item);
    Task UpdateTodoAsync(TodoItem item);
    Task DeleteTodoAsync(TodoItem item);
    Task<List<Category>> GetCategoriesAsync();
}