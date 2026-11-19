using CorrosionModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WPFPlateModel.ViewModels;

namespace WPFPlateModel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            Window entranceWindow = _serviceProvider.GetService<EntranceWindow>() ?? throw new Exception("Ошибка в конфигурации сервисов");
            entranceWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<PlateStateStorage>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>();
            services.AddTransient<EntranceViewModel>();
            services.AddTransient<EntranceWindow>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }
}
