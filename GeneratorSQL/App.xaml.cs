using System;
using System.Windows;
using GeneratorSQL.Services.Implementations;
using GeneratorSQL.Services.Interfaces;
using GeneratorSQL.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace GeneratorSQL
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();

            // Apply default theme
            var themeService = _serviceProvider.GetRequiredService<IThemeService>();
            themeService.SetTheme("Light");

            var mainWindow = _serviceProvider.GetRequiredService<Views.MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Core services
            services.AddSingleton<IDataGeneratorService, BogusDataGeneratorService>();
            services.AddSingleton<ISqlExportService, SqlExportService>();
            services.AddSingleton<IExportService, JsonExportService>();
            services.AddSingleton<IExportService, CsvExportService>();
            services.AddSingleton<IExportService, XmlExportService>();
            services.AddSingleton<IClipboardService, ClipboardService>();
            services.AddSingleton<IThemeService, ThemeService>();

            // ViewModels
            services.AddSingleton<MainViewModel>();

            // Views
            services.AddSingleton<Views.MainWindow>();
        }
    }
}
