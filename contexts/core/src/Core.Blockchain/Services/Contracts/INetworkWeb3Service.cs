using System.Threading.Tasks;

namespace Core.Blockchain
{
    public interface INetworkWeb3Service
    {
        Task<long> GetNetWorkIdAsync();
    }
}
