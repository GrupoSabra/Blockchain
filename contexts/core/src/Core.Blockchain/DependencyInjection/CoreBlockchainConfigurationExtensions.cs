using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sabra.Framework.Ethereum.DependencyInjection;

namespace Core.Blockchain.DependencyInjection
{
    public static class CoreBlockchainConfigurationExtensions
    {
        public static void AddCoreBlockchainConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEthereumServices();

            services.AddTransient<IEventLogSmartContractFactory, EventLogSmartContractFactory>();
            services.AddTransient<IEventLogSmartContract, EventLogSmartContract>();
        }
    }
}
