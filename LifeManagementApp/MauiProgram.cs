using LifeManagementApp.Data;
using LifeManagementApp.Services;
using LifeManagementApp.ViewModels;
using LifeManagementApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LifeManagementApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // 1. SQLite-tietokantapolku ja AppDbContext
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "lifemanagement.db3");
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        // 2. Palvelut ja rajapinnat (Dependency Injection)
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<IWeatherService, OpenMeteoWeatherService>();
        builder.Services.AddScoped<ITodoRepository, TodoRepository>();

        // 3. ViewModelit (Transient: jokainen sivun avaus saa puhtaan tilan, erityisesti AddTodoViewModel)
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<TodoListViewModel>();
        builder.Services.AddTransient<AddTodoViewModel>();

        // 4. Sivut (Views)
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<TodoListPage>();
        builder.Services.AddTransient<AddTodoPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // Varmistetaan, että SQLite-tietokanta ja oletuskategoriat on luotu käynnistyessä
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        }

        return app;
    }
}