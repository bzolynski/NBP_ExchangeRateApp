using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NBP_ExchangeRateApp.Library.Services;
using NBP_ExchangeRateApp.Ui.Factories.ViewModelFactories;
using NBP_ExchangeRateApp.Ui.States.Navigator;
using NBP_ExchangeRateApp.Ui.ViewModels;
using NBP_ExchangeRateApp.Ui.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NBP_ExchangeRateApp.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IConfiguration Configuration => AddCongfiguration();

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceProvider = CreateServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var window = scope.ServiceProvider.GetRequiredService<ShellView>();
                window.DataContext = scope.ServiceProvider.GetRequiredService<ShellViewModel>();

                window.Show();
            }
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddSingleton(Configuration);

            services.AddHttpClient("NBP", c =>
            {
                c.BaseAddress = new Uri("http://api.nbp.pl/api/");
                c.DefaultRequestHeaders.Add("Accept", "application/json ");
            });


            // Views
            services.AddSingleton<ShellView>();
            services.AddSingleton<CurrencyTablesView>();
            services.AddSingleton<GoldPricesView>();

            // ViewModels
            services.AddSingleton<ShellViewModel>();
            services.AddScoped<CurrencyTablesViewModel>();
            services.AddScoped<GoldPricesView>();

            // Services
            services.AddScoped<INBPCurrencyRateService, NBPCurrencyRateService>();
            services.AddScoped<INBPGoldValueService, NBPGoldValueService>();

            // States
            services.AddSingleton<INavigator, Navigator>();

            // Factories
            services.AddScoped<IRootViewModelFactory, RootViewModelFactory>();
            services.AddScoped<IViewModelFactory<CurrencyTablesViewModel>, CurrencyTablesViewModelFactory>();
            services.AddScoped<IViewModelFactory<GoldPricesViewModel>, GoldPricesViewModelFactory>();

            return services.BuildServiceProvider();
        }

        private IConfiguration AddCongfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }

    
}
