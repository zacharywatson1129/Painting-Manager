using Caliburn.Micro;
using PaintingDetailsManager.EventModels;
using PaintingLibrary;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PaintingDetailsManager.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        const string IMG_DIALOG_FILTER = "All Image Files | *.*";
        readonly string imagesFolderPath = System.IO.Path.GetFullPath(System.IO.Path.Combine
            (System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            @"..\..\Data\Images\"));
       

        public ShellViewModel(SimpleContainer container, NotesViewModel notesVM, 
                                IEventAggregator events, IWindowManager windowManager)
        {
            _container = container;
            _notesVM = notesVM;
            _events = events;
            _events.Subscribe(this);
            _windowManager = windowManager;
            LoadDefaultView();
        }

        private SimpleContainer _container;
        private IEventAggregator _events;
        private IWindowManager _windowManager;
        private NotesViewModel _notesVM;
        private bool isAPIRunning = false;
        private string ipAddress = "";
        private string portNumber = "";
        public string APIUrl { get { return $"{ipAddress}:{portNumber}"; } }

        private static string GetWebApiProjectDirectory()
        {
            // Get the base directory (where the console app is running from)
            var baseDir = AppContext.BaseDirectory;

            // Go up two levels from the bin folder to the solution directory
            var solutionDir = Path.Combine(baseDir, "../../../../");

            // Combine the solution directory with the WebApiDemo folder
            var webApiProjectDir = Path.Combine(solutionDir, "WebApiDemo");

            return Path.GetFullPath(webApiProjectDir); // Get the full path
        }

        /// <summary>
        /// Starts the Web API asynchronously--we do not await it, we allow it to run in background. 
        /// This method also Configures Swagger, Kestrel, etc. It performs all basic Program.cs tasks
        /// of the WebAPI project.
        /// </summary>
        /*private static void StartWebApi()
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

            builder.Services.AddSingleton(dataAccess);

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

            Console.Clear();
            Console.WriteLine("Attempting to start the Web API...\n\n");
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

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }*/

        public void LoadDefaultView()
        {
            ActivateItem(_container.GetInstance<DefaultViewModel>());
        }


        public void CreateNewImage()
        {
            Stream checkStream = null;

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            openFileDialog.Multiselect = false;

            openFileDialog.Filter = IMG_DIALOG_FILTER;

            if ((bool)openFileDialog.ShowDialog())
            {
                try
                {
                    if ((checkStream = openFileDialog.OpenFile()) != null)
                    {
                        AddPaintingViewModel vm = _container.GetInstance<AddPaintingViewModel>();

                        _events.PublishOnUIThread(new AddPaintingEvent
                            ( new PaintingLibrary.Models.FileImage()
                                {
                                    FileName = openFileDialog.SafeFileName,
                                    CompleteFilePath = openFileDialog.FileName
                                }
                            )
                        );

                        _windowManager.ShowDialog(vm, null, null);
                    }
                }
                catch (Exception ex)
                {
                    // MessageBox.Show("Sorry, the application has run into a problem.");
                    Trace.TraceInformation("Error during opening image dialog inside . " + DateTime.Now + "\n" + ex.Message);
                    // You must close or flush the trace to empty the output buffer.
                    Trace.Flush();
                }
            }

        }

        public void LoadNotesPage()
        {
            // We won't need a new instance each time because
            // we can't change the notes from any other view.
            // Might be a little faster anyway.
            ActivateItem(_notesVM);
        }

        public void LoadListView()
        {
            // Going to create a new instance each time since
            // we might need to reload the database anyway.
            ActivateItem(_container.GetInstance<ListViewModel>());
        }

        public void LoadGalleryView()
        {
            ActivateItem(_container.GetInstance<GalleryViewModel>());
        }

        public void LoadAPIConfigureView()
        {
            ActivateItem(_container.GetInstance<APIConfigureViewModel>());
        }
    }
}