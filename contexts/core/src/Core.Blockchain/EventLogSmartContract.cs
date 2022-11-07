using Nethereum.RPC.Eth.DTOs;
using Sabra.Framework.Ethereum;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Blockchain
{
    public interface IEventLogSmartContract : ISmartContract
    {
        Task<string> SetLogContentAsync(long signatureId, long documentId, string log, string from, byte[] signature, byte[] friendlyContentHash);
        Task<string> SetLogAsync(long signatureId, long documentId, string log);
        Task<BigInteger> Nonce(string from, long documentId);
        Task<TransactionReceipt> GetTransactionReceipt(string transactionHash);
    }

    public class EventLogSmartContract : SmartContract, IEventLogSmartContract
    {
        public EventLogSmartContract(IEthNetworkProvider ethNetworkProvider, IEthAccountingService ethAccountingService, IDistributedNonceProvider distributedNonceProvider)
            : base(ethNetworkProvider, ethAccountingService, distributedNonceProvider)
        {  
        }

        public Task<string> SetLogContentAsync(long signatureId, long documentId, string log, string from, byte[] signature, byte[] friendlyContentHash)
             => SendTransactionAsync("setLogContent", 500000, signatureId, documentId, log, from, signature, friendlyContentHash);

        public Task<string> SetLogAsync(long signatureId, long documentId, string log)
             => SendTransactionAsync("setLog", 500000, signatureId, documentId, log);

        public async Task<BigInteger> Nonce(string from, long documentId)
        {
            return await InvokeCallAsync<BigInteger>("_nonce", from, documentId);
        }

        public async Task<TransactionReceipt> GetTransactionReceipt(string transactionHash)
        {
            var transactionReceipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            while (transactionReceipt == null)
            {
                Thread.Sleep(5000);
                transactionReceipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            }
            return transactionReceipt;
        }
    }
}
