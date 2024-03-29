﻿using FormatConverter.Helpers;
using FormatConverter.IllegalActions;
using FormatConverter.Output;
using FormatConverter.ValidtyCheckers;
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

            var matchesTreeCreator = serviceProvider.GetService<IMatchesTreeCreator>();
            var matchesTree = matchesTreeCreator.Create(_config.InputDir);
            var outputer = serviceProvider.GetService<IMatchesTreeOutputer>();
            var outputFilesCount = outputer.DoOutput(matchesTree);
            _logger.LogInformation("Created " + outputFilesCount + " files (no Fold as last move files.)");
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var config = SetupConfiguration();

            services
                .AddAutoMapper(typeof(Program))
                .Configure<AppSettingsOptions>(
                    config.GetSection("AppSettings"))
                .Configure<StrategyDelimitersOptions>(
                    config.GetSection("AppSettings").GetSection("InputPatterns").GetSection("StrategyDelimiters"));

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.AddNLog("nlog.config");
            });
            LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));

            services
                .AddTransient<IMatchesTreeCreator, MatchesTreeCreator>()
                .AddTransient<ILegalityChecker, LegalityChecker>()
                .AddTransient<ITurnsLegalityChecker, TurnsLegalityChecker>()
                .AddTransient<IMatchesTreeLegalityChecker, MatchesTreeLegalityChecker>()
                .AddTransient<ITurnHelper, TurnHelper>()
                .AddTransient<IC2StrategyValidityChecker, C2StrategyValidityChecker>()
                .AddTransient<IMatchesTreeOutputer, C2Output>();
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
