using Microsoft.Extensions.Logging;
using PaintingLibrary;

namespace BlazorHybridMAUIApp
{
    public static class MauiProgram
    {

        //private const string connectionString = @"Data Source =Data\Working\paintings.db;";
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();

            // Create just the path to the database, create the folders.
            string databaseDirectory = Path.Combine(FileSystem.AppDataDirectory, "Data", "Working");
            if (!Directory.Exists(databaseDirectory))
            {
                Directory.CreateDirectory(databaseDirectory);
            }

            // This doesn't mean the file exists, but
            // we'll reference it for our database connection string.
            var databasePath = Path.Combine(databaseDirectory, "paintings.db");
            var connectionString = $"Data Source={databasePath}";

            builder.Services.AddSingleton<SqliteDataAccess>(sp =>
            {
                return new SqliteDataAccess(connectionString);
            });
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
