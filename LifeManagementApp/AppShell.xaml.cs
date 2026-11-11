using LifeManagementApp.Views;

namespace LifeManagementApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Rekisteröidään AddTodoPage Shell-reitiksi, jotta sille voidaan navigoida ja välittää parametreja
        Routing.RegisterRoute(nameof(AddTodoPage), typeof(AddTodoPage));
    }
}