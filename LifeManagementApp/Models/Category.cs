namespace LifeManagementApp.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigaatio-ominaisuus: Yhteen kategoriaan voi kuulua monta tehtävää (1-N relaatio)
    public List<TodoItem> TodoItems { get; set; } = new();
}