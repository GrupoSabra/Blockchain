using System;
using Configuration;
using Ethereum.Domain.Services;
using Ethereum.Domain.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sabra.Framework.Ethereum;
using Sabra.Framework.Ethereum.DependencyInjection;

namespace Ethereum.Domain.DependencyInjection
{
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Logging;
    using StackExchange.Redis;

    public static class EthereumDomainConfigurationExtensions
    {
        private const string REDIS_NONCE_STORE_CX_NAME = "NonceStoreDB";

        public static void AddEthereumDomainConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EthereumConfiguration>(configuration.GetSection("Ethereum"));

            services.AddEthereumServices();
            services.AddSingleton<ILoggerFactory, NullLoggerFactory>();

            services.AddSingleton<IGasPriceService, GasPriceService>();

            services.AddSingleton<IEthNetworkProvider, EthNetworkProvider>();
            services.AddSingleton<IEthGasPriceProvider, EthGasPriceProvider>();
        }

        public static void AddEthereumDomainInMemoryNonceStore(this IServiceCollection services)
        {
            services.AddInMemoryNonceStore(setup =>
            {
                setup.LockPoolingDelay = TimeSpan.FromMilliseconds(100);
                setup.GetTransactionCountRetryDelay = TimeSpan.FromMilliseconds(500);
            });
        }

        public static void AddEthereumDomainRedisNonceStore(this IServiceCollection services, IConfiguration configuration)
        {
            var redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString(REDIS_NONCE_STORE_CX_NAME));
            services.AddRedisNonceStore(redis);
        }
    }
}
