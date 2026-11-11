using LifeManagementApp.Data;
using LifeManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LifeManagementApp.Services;

public class TodoRepository : ITodoRepository
{
    private readonly AppDbContext _context;

    public TodoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TodoItem>> GetAllTodosAsync()
    {
        // Include(t => t.Category) lataa myös tehtävään liittyvän kategorian tiedot (1-N relaatio)
        return await _context.TodoItems
            .Include(t => t.Category)
            .ToListAsync();
    }

    public async Task<TodoItem?> GetTodoByIdAsync(int id)
    {
        return await _context.TodoItems
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddTodoAsync(TodoItem item)
    {
        _context.TodoItems.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTodoAsync(TodoItem item)
    {
        _context.TodoItems.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTodoAsync(TodoItem item)
    {
        _context.TodoItems.Remove(item);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }
}