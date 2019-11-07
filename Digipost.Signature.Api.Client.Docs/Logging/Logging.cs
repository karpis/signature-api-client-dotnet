using System;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Portal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Digipost.Signature.Api.Client.Docs.Logging
{
    public class Logging
    {
        private static IServiceProvider CreateServiceProviderAndSetUpLogging()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

            var serviceProvider = services.BuildServiceProvider();
            SetUpLoggingForTesting(serviceProvider);

            return serviceProvider;
        }

        private static void SetUpLoggingForTesting(IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            loggerFactory.AddNLog(new NLogProviderOptions {CaptureMessageTemplates = true, CaptureMessageProperties = true});
            LogManager.LoadConfiguration("./nlog.config");
        }

        static void Main(string[] args)
        {
            ClientConfiguration clientConfiguration = null;
            var serviceProvider = CreateServiceProviderAndSetUpLogging();
            var client = new PortalClient(clientConfiguration, serviceProvider.GetService<ILoggerFactory>());
        }
    }
}
