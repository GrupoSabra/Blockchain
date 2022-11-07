using Nethereum.RPC.Accounts;
using Sabra.Framework.Ethereum;
using Sabra.Framework.Ethereum.Services;
using System;
using System.Threading.Tasks;

namespace Core.Blockchain
{
    public interface IEventLogSmartContractFactory : ISmartContractFactory<IEventLogSmartContract>
    {
        Task<IEventLogSmartContract> CreateAsync(IAccount senderAccount, IAccount prefundedAccount);
    }

    public class EventLogSmartContractFactory : SmartContractFactory<IEventLogSmartContract, IEventLogSmartContract>, IEventLogSmartContractFactory
    {
        public EventLogSmartContractFactory(IWeb3Service web3Service, IServiceProvider serviceProvider, IEthNetworkProvider ethNetworkProvider, IEthAccountingService ethAccountingService, IDistributedNonceProvider distributedNonceProvider)
            : base(web3Service, serviceProvider, ethNetworkProvider, ethAccountingService, distributedNonceProvider)
        { }

        protected override ulong RequiredGasUnits => 6800000;

        public Task<IEventLogSmartContract> CreateAsync(IAccount senderAccount, IAccount prefundedAccount)
            => base.CreateAsync(senderAccount, prefundedAccount);

        protected override string AbiResourceName => $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.contracts.EventLog.abi";
        protected override string BinResourceName => $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.contracts.EventLog.bin";
    }
}
