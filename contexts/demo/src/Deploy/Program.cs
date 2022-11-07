using Deploy.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Sabra.Framework.ServiceProviderBuilder;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Deploy
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var environmentName = args.Length > 0 ? args[0] : Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                var serviceProvider = CreateServiceProvider(environmentName);
                var deployer = serviceProvider.GetService<IDeployer>();
                var eventLogSmartContract = await deployer.Deploy("0xb44564ecaa8889afb943c47feeb87473c36edcbeab9498426b9e0ac171b7ef66", "0xb44564ecaa8889afb943c47feeb87473c36edcbeab9498426b9e0ac171b7ef66");
                Console.WriteLine($"El contrato fue deployado en {eventLogSmartContract.ContractAddress}");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.Message);
            }
        }

        private static ServiceProvider CreateServiceProvider(string environmentName)
        {
            return ServiceProviderBootstrap.Build(environmentName, (services, configuration) =>
            {
                services.AddDeployDependencies(configuration, environmentName, Assembly.GetExecutingAssembly());
            });
        }
    }
}
