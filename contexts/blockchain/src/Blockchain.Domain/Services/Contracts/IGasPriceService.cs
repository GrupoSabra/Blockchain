
using System.Threading.Tasks;

namespace Ethereum.Domain.Services.Contracts
{
    public interface IGasPriceService
    {
        Task<long> GetGasPriceWei();
    }
}
