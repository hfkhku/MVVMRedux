using Autofac;
using MStopwatch;
using Prism.Autofac;
using Prism.Modularity;
using Stopwatch.Views;
using System.Windows;

namespace Stopwatch
{
    class Bootstrapper : AutofacBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            var catalog = (ModuleCatalog)ModuleCatalog;
            catalog.AddModule(typeof(MStopwatchModule));
        }

        protected override void ConfigureContainerBuilder(ContainerBuilder builder)
        {
            base.ConfigureContainerBuilder(builder);
        }
    }
}
