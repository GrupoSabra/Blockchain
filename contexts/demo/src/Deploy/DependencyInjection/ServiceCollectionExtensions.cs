using Configuration;
using Core.Blockchain;
using Core.Blockchain.DependencyInjection;
using Deploy;
using Ethereum.Domain.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sabra.Framework.DomainModel.DependencyInjection;
using Sabra.Framework.Ethereum.Services;
using Sabra.Framework.Logging.SerilogELK;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace Deploy.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDeployDependencies(this IServiceCollection services, IConfiguration configuration, string environmentName, Assembly assembly)
        {
            var configurationRoot = GetConfigurationRoot(environmentName, assembly);

            services.AddHttpContextAccessor();

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<IConfiguration>(configurationRoot);
            services.Configure<EthereumConfiguration>(configurationRoot.GetSection("Ethereum"));

            services.AddLogging(configure =>
            {
                configure.AddSerilog();
                configure.AddConsole();
            });

            services.AddDomainModelBaseServices();
            services.AddEthereumDomainInMemoryNonceStore();

            services.AddCoreBlockchainConfiguration(configurationRoot);
            services.AddEthereumDomainConfiguration(configurationRoot);
            services.AddSingleton<IWeb3Service, SingletonWeb3Service>();
            services.AddSingleton<INetworkWeb3Service, SingletonWeb3Service>();
            services.AddTransient<IDeployer, Deployer>();


        }

        private static IConfigurationRoot GetConfigurationRoot(string environmentName, Assembly assembly)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(assembly.Location))
                .AddJsonFile("appsettings.json", true, true);
            var appSettingsFileName = "";
            if (environmentName.StartsWith("Local"))
            {
                var developerName = Environment.GetEnvironmentVariable("DEVELOPER_NAME");
                appSettingsFileName = $"appsettings.{environmentName}{developerName}.json";
            }
            else
            {
                appSettingsFileName = $"appsettings.{environmentName}.json";
            }
            configurationBuilder.AddJsonFile(appSettingsFileName, true, true);
            Console.WriteLine($"Entorno: {appSettingsFileName}");
            configurationBuilder.AddEnvironmentVariables();
            return configurationBuilder.Build();
        }

    }
}
