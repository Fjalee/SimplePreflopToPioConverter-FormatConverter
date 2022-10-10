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

        public static PositionsMetaData InputPositionsMetaData { get; private set; }
        public static PositionsMetaData OutputPositionsMetaData { get; private set; }

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

            InputPositionsMetaData = new PositionsMetaData(_config.InputPositionNames.SBName, _config.InputPositionNames.BBName, _config.InputPositionNames.UTGName, _config.InputPositionNames.MP1Name, _config.InputPositionNames.MP2Name, _config.InputPositionNames.MP3Name, _config.InputPositionNames.HIJName, _config.InputPositionNames.COName, _config.InputPositionNames.BTNName);
            OutputPositionsMetaData = new PositionsMetaData(_config.OutputPositionNames.SBName, _config.OutputPositionNames.BBName, _config.OutputPositionNames.UTGName, _config.OutputPositionNames.MP1Name, _config.OutputPositionNames.MP2Name, _config.OutputPositionNames.MP3Name, _config.OutputPositionNames.HIJName, _config.OutputPositionNames.COName, _config.OutputPositionNames.BTNName);

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
                .Configure<OutputPositionNamesOptions>(
                    config.GetSection("AppSettings").GetSection("OutputPositionNames"))
                .Configure<InputPositionNamesOptions>(
                    config.GetSection("AppSettings").GetSection("InputPositionNames"));

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.AddNLog("nlog.config");
            });
            LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));

            services
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
