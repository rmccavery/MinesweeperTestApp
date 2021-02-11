namespace GameTestApp
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using CommonServiceLocator;
    using GameTestApp.Business;
    using GameTestApp.ViewModels;
    using Prism.Events;
    using Unity;
    using Unity.Injection;
    using Unity.Lifetime;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private UnityContainer container;
        private Views.MainWindow shell;

        /// <summary>
        /// Overriden app start up to register types and perform basic wire-up
        /// </summary>
        /// <param name="e">The startup event args</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create container and service locator
            this.container = new UnityContainer();
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(this.container));

            this.SetupExceptionHandling();

            // Unity type registrations
            this.RegisterTypes();

            this.shell = this.container.Resolve<Views.MainWindow>();
            this.shell.Show();
        }

        /// <summary>
        /// Registers the types used by this application
        /// </summary>
        protected void RegisterTypes()
        {
            IServiceLocator locator = new UnityServiceLocator(this.container);
            ServiceLocator.SetLocatorProvider(() => locator);

            this.container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
            this.container.RegisterType<GameController, GameController>(new ContainerControlledLifetimeManager());
            this.container.RegisterType<GameViewModel, GameViewModel>(new ContainerControlledLifetimeManager());

            // Register the shell and inject the GameViewModel as the data context
            this.container.RegisterType<Views.MainWindow, Views.MainWindow>(
                new ContainerControlledLifetimeManager(),
                new InjectionProperty("DataContext", this.container.Resolve<GameViewModel>()));
        }

        /// <summary>
        /// Sets up global exception handling
        /// </summary>
        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                this.LogUnhandledException((Exception) e.ExceptionObject, "### AppDomain.CurrentDomain.UnhandledException ###");

                if (e.IsTerminating)
                {
                    MessageBox.Show(
                        "Unhandled exception!",
                        Current?.MainWindow?.Title, MessageBoxButton.OK, MessageBoxImage.Error);

                    this.Shutdown();
                }
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                this.LogUnhandledException(e.Exception, "### TaskScheduler.UnobservedTaskException ###");
                e.SetObserved();
            };
        }

        /// <summary>
        /// Logs an unhandled exception
        /// </summary>
        /// <param name="exception">The exception that was thrown</param>
        /// <param name="source">The source of the exception</param>
        private void LogUnhandledException(Exception exception, string source)
        {
            string message = $"Unhandled exception ({source})";
            string stackMessage = string.Empty;

            try
            {
                AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
                message = $"Unhandled exception in {assemblyName.Name} v{assemblyName.Version}";
                stackMessage = $"Target : {exception.TargetSite}, Stack : {exception.StackTrace} Inner : {exception.InnerException?.Message}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in LogUnhandledException");
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Debug.WriteLine($"{message} -  {exception}");
                Debug.Write(stackMessage);
            }
        }
    }
}