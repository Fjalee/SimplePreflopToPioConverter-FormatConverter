using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Extensions.Logging;

namespace FormatConverter
{
    public class Program
    {
        private static ILogger<Program>? _logger;
        private static AppSettingsOptions _config;

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            _logger?.LogCritical("Unhandled exception occured\n" + (Exception)e.ExceptionObject);
            throw new Exception("Global Exception Handler");
        }

        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            _logger = serviceProvider.GetService<ILogger<Program>>();
            _config = serviceProvider.GetService<IOptions<AppSettingsOptions>>().Value;

            var matchesTreeCreator = serviceProvider.GetService<IMatchesTreeCreator>();
            matchesTreeCreator.Create(_config.InputDir);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var config = SetupConfiguration();

            services
                .AddAutoMapper(typeof(Program))
                .Configure<AppSettingsOptions>(
                    config.GetSection("AppSettings"))
                .Configure<PositionNamesOptions>(
                    config.GetSection("AppSettings").GetSection("PositionNames"));

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.AddNLog("nlog.config");
            });
            LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));

            services
                .AddSingleton<IPositionsMetaData, PositionsMetaData>()             //delete this line (its from template) 
                .AddTransient<IMatchesTreeCreator, MatchesTreeCreator>();
        }

        private static IConfiguration SetupConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile($"local-appsettings.json", false)
                .AddJsonFile($"appsettings.json", false)
                .SetBasePath(Directory.GetCurrentDirectory())
                .Build();
        }
    }
}
