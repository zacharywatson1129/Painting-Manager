using Caliburn.Micro;
using PaintingDetailsManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PaintingDetailsManager
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            // container holds an instance of itself to pass out
            // when we ask for a container.
            _container.Instance(_container);

            // Two important Caliburn Micro entities we may need to
            // new up instances of. Could be more in future, but this
            // is a start.
            // This creates ONE instance for the entire application.
            // We don't want to do this for everything, but we really
            // only need ONE window manager and one event aggregator.
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();

            // Don't want to use a reflection a lot because of performance
            // but it won't hurt to do it once at startup.
            // Get every type in our application, then we filter it down.
            // We only want to get classes whose name ends in ViewModels
            // (we only want ViewModels, in other words).
            // We turn that into a list, then ForEach type, we 
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));


        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainWindowViewModel>();           
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
