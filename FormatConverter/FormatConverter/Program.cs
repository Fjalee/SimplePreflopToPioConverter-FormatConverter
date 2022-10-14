using FormatConverter.IllegalActions;
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
        public static List<PositionEnum> PositionsOrder { get; set; } = new List<PositionEnum>() { PositionEnum.UTG, PositionEnum.MP1, PositionEnum.MP2, PositionEnum.MP3, PositionEnum.HIJ, PositionEnum.CO, PositionEnum.BTN, PositionEnum.SB, PositionEnum.BB };

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

            InputPositionsMetaData = new PositionsMetaData(_config.InputPatterns.PositionNames.SBName, _config.InputPatterns.PositionNames.BBName, _config.InputPatterns.PositionNames.UTGName, _config.InputPatterns.PositionNames.MP1Name, _config.InputPatterns.PositionNames.MP2Name, _config.InputPatterns.PositionNames.MP3Name, _config.InputPatterns.PositionNames.HIJName, _config.InputPatterns.PositionNames.COName, _config.InputPatterns.PositionNames.BTNName);
            OutputPositionsMetaData = new PositionsMetaData(_config.OutputPatterns.PositionNames.SBName, _config.OutputPatterns.PositionNames.BBName, _config.OutputPatterns.PositionNames.UTGName, _config.OutputPatterns.PositionNames.MP1Name, _config.OutputPatterns.PositionNames.MP2Name, _config.OutputPatterns.PositionNames.MP3Name, _config.OutputPatterns.PositionNames.HIJName, _config.OutputPatterns.PositionNames.COName, _config.OutputPatterns.PositionNames.BTNName);

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
                    config.GetSection("AppSettings").GetSection("OutputPatterns.OutputPositionNames"))
                .Configure<InputPositionNamesOptions>(
                    config.GetSection("AppSettings").GetSection("InputPatterns.InputPositionNames"));

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.AddNLog("nlog.config");
            });
            LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));

            services
                .AddTransient<IMatchesTreeCreator, MatchesTreeCreator>()
                .AddTransient<ITurnsLegalityChecker, TurnsLegalityChecker>();
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
