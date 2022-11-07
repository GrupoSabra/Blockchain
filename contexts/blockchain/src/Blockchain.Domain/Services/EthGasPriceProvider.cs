using Ethereum.Domain.Services.Contracts;
using Sabra.Framework.Ethereum;
using System.Threading.Tasks;

namespace Ethereum.Domain.Services
{
    public class EthGasPriceProvider : IEthGasPriceProvider
    {
        private const ulong GAS_UNITS_STANDARD_TRANSACTION = 21000ul;

        private readonly IGasPriceService _gasPriceService;

        public EthGasPriceProvider(IGasPriceService gasPriceService)
        {
            _gasPriceService = gasPriceService;
        }

        public ulong StandardTransactionGasUnits => GAS_UNITS_STANDARD_TRANSACTION;

        public async Task<long> GetGasPriceWei() => await _gasPriceService.GetGasPriceWei();
    }
}
