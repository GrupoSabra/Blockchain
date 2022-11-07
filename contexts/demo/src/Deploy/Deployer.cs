using Core.Blockchain;
using Nethereum.Web3.Accounts;
using Sabra.Framework.Ethereum;
using Sabra.Framework.Ethereum.Services;
using System;
using System.Threading.Tasks;

namespace Deploy
{
    public interface IDeployer
    {
        Task<ISmartContract> Deploy(string prefundedAccountPrivateKey, string systemAccountPrivateKey);
    }
    public class Deployer : IDeployer
    {
        private readonly IWeb3Service _web3Service;
        private readonly IEventLogSmartContractFactory _eventLogSmartContractFactory;

        public Deployer(
            IEventLogSmartContractFactory eventLogSmartContractFactory,
            IWeb3Service web3Service
            ) 
        {
            _eventLogSmartContractFactory = eventLogSmartContractFactory;
            _web3Service = web3Service;
        }

        public async Task<ISmartContract> Deploy(string prefundedAccountPrivateKey, string systemAccountPrivateKey)
        {
            var web3 = _web3Service.GetWeb3();
            var netWorkId = await web3.Net.Version.SendRequestAsync();
            var prefundedAccount = new Account(prefundedAccountPrivateKey, Convert.ToInt64(netWorkId));
            var systemAccount = new Account(systemAccountPrivateKey, Convert.ToInt64(netWorkId));

            return  await _eventLogSmartContractFactory.CreateAsync(systemAccount, prefundedAccount);
        }

        
    }

}
