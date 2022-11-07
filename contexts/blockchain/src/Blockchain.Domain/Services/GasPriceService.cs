using Ethereum.Domain.Services.Contracts;
using Sabra.Framework.Ethereum.Services;
using System.Threading.Tasks;

namespace Ethereum.Domain.Services
{
    public class GasPriceService : IGasPriceService
    {
        private readonly IWeb3Service _web3Service;

        public GasPriceService(IWeb3Service web3Service)
        {
            _web3Service = web3Service;
        }

        public async Task<long> GetGasPriceWei()
        {
            var web3 = _web3Service.GetWeb3();
            var gasPrice = await web3.Eth.GasPrice.SendRequestAsync();
            //multiplicarlo por 1.05
            return (long)(gasPrice.Value * 21 / 20);

        }
    }
}
