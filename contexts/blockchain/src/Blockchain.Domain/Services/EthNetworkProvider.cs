using Configuration;
using Microsoft.Extensions.Options;
using Sabra.Framework.Ethereum;

namespace Ethereum.Domain.Services
{
    public class EthNetworkProvider : IEthNetworkProvider
    {
        private readonly IOptions<EthereumConfiguration> _ethereumConfiguration;

        public EthNetworkProvider(IOptions<EthereumConfiguration> ethereumConfiguration)
        {
            _ethereumConfiguration = ethereumConfiguration;
        }

        public string NetworkUrl => _ethereumConfiguration.Value.NetworkUrl;

        string IEthNetworkProvider.AuthenticationScheme => throw new System.NotImplementedException();

        string IEthNetworkProvider.AuthenticationParameter => throw new System.NotImplementedException();

    }
}
