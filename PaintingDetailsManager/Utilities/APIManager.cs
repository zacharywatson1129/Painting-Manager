
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaintingLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaintingDetailsManager.Utilities
{
    /// <summary>
    /// This class will manage the starting and stopping of the Synchronization API.
    /// </summary>
    public class APIManager
    {
        private static Microsoft.AspNetCore.Builder.WebApplication? _webApp;
        private static IHost _apiHost;
        private static CancellationTokenSource _cancellationTokenSource;
        private static string apiUrl { get; set; } = string.Empty;
        private static bool isAPIRunning = false;
        string _ipAddress = "";
        string _portNumber = "";

        public APIManager(string ipAddress, string portNumber)
        {
            _ipAddress = ipAddress;
            _portNumber = portNumber;
        }

        public void StartAPI()
        {
            // First, let's change our current directory to be that of the web api.
            var webApiProjectDir = GetWebApiProjectDirectory();
            Directory.SetCurrentDirectory(webApiProjectDir);

            // Create a builder with some WebApplicationOptions
            // that we specify. 
            var builder = WebApplication.CreateBuilder(
                new WebApplicationOptions()
                {
                    EnvironmentName = "Development",
                    ApplicationName = "WebApiDemo"
                });

            // Configure Kestrel to use HTTPS
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(7140); // HTTP
                options.ListenAnyIP(7141, listenOptions =>
                {
                    listenOptions.UseHttps(); // HTTPS
                });
            });

            // We could inject the ConnectionString. However, we need the SAME dataaccess object.
            // So we inject the same dataAccess object. 

            // builder.Services.AddSingleton(dataAccess);

            // Add services to the container
            builder.Services.AddControllers(); // Enable controllers

            // -------------------------------------------
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            _webApp = builder.Build();

            // Configure the HTTP request pipeline (middleware)
            _webApp.UseRouting();

#pragma warning disable ASP0014
            _webApp.UseEndpoints(endpoints =>
            {
                // Map controller routes
                endpoints.MapControllers();
            });
#pragma warning restore ASP0014


            // ----------------------------------------------------------
            // Configure the HTTP request pipeline.
            if (_webApp.Environment.IsDevelopment())
            {
                // We need these to be able to explore the API through the browser.
                _webApp.UseSwagger();
                _webApp.UseSwaggerUI();
            }

            _webApp.UseHttpsRedirection();

            _webApp.UseAuthorization();

            int success = 1;

            
            try
            {
                _webApp.RunAsync(apiUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting Web API: {ex.Message}");
                success = 0;
            }

            if (success == 1)
            {
                isAPIRunning = true;
                Console.WriteLine("\n\nThe Web api has started successfully!");
            }
        }

        /// <summary>
        /// Gets the directory containing the WebApiDemo project.
        /// </summary>
        /// <returns>A string containing the directory of the WebApiDemo project.</returns>
        private string GetWebApiProjectDirectory()
        {
            // Get the base directory (where the console app is running from)
            var baseDir = AppContext.BaseDirectory;

            // Go up two levels from the bin folder to the solution directory
            var solutionDir = Path.Combine(baseDir, "../../../../");

            // Combine the solution directory with the WebApiDemo folder
            var webApiProjectDir = Path.Combine(solutionDir, "WebApiDemo");

            return Path.GetFullPath(webApiProjectDir); // Get the full path            
        }

        public void StopAPI() 
        {
            
        }
    }
}
