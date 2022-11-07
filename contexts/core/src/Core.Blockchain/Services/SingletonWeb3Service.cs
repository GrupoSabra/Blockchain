using Configuration;
using Microsoft.Extensions.Options;
using Nethereum.JsonRpc.WebSocketClient;
using Nethereum.RPC.Accounts;
using Nethereum.Web3;
using Sabra.Framework.Ethereum.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Blockchain
{
    public class SingletonWeb3Service : IWeb3Service, INetworkWeb3Service
    {
        private const int SECONDS_TO_CLEAR_CONNECTION = 97;

        
        private ConcurrentDictionary<string, WebSocketWeb3Mapper> _clientMappers;

        private WebSocketWeb3Mapper _web3Mapper;

        private readonly IOptions<EthereumConfiguration> _options;

        public SingletonWeb3Service(
            IOptions<EthereumConfiguration> options)
        {
            _options = options;
            _clientMappers = new ConcurrentDictionary<string, WebSocketWeb3Mapper>();
        }

        public Web3 GetWeb3(IAccount senderAccount)
        {
            //ClearTimeoutConnections();

            var web3WithSender = _clientMappers.GetValueOrDefault(senderAccount.Address);

            if (web3WithSender == null)
            {
                WebSocketClient.ConnectionTimeout = TimeSpan.FromMilliseconds(30000);
                WebSocketClient.ForceCompleteReadTotalMilliseconds = 400000;
                var webSocketClient = new WebSocketClient(_options.Value.NetworkUrlWS);
                var web3 = new Web3(senderAccount, webSocketClient);
                //Para polygon 
                //web3.TransactionManager.UseLegacyAsDefault = true;


                web3WithSender = new WebSocketWeb3Mapper
                {
                    Web3 = web3,
                    WebSocket = webSocketClient,
                    CreatedDate = DateTime.Now
                };

                _clientMappers.AddOrUpdate(senderAccount.Address, web3WithSender, (x, wsw3) => wsw3);
            }

            return web3WithSender.Web3;
        }

        public Web3 GetWeb3()
        {
            if (_web3Mapper != null)
            {
                if (_web3Mapper.CreatedDate.AddSeconds(SECONDS_TO_CLEAR_CONNECTION) < DateTime.Now)
                {
                    _web3Mapper.WebSocket.Dispose();
                    _web3Mapper = null;
                }
            }

            if (_web3Mapper == null)
            {
                var webSocketClient = new WebSocketClient(_options.Value.NetworkUrlWS);
                var web3 = new Web3(webSocketClient);
                //Para polygon 
                //web3.TransactionManager.UseLegacyAsDefault = true;

                _web3Mapper = new WebSocketWeb3Mapper
                {
                    Web3 = web3,
                    WebSocket = webSocketClient,
                    CreatedDate = DateTime.Now
                };
            }

            return _web3Mapper.Web3;
        }

        public void ClearTimeoutConnections()
        {
            foreach (var wsw3 in _clientMappers.Where(x => x.Value.CreatedDate.AddSeconds(SECONDS_TO_CLEAR_CONNECTION) < DateTime.Now).ToArray())
            {
                wsw3.Value.WebSocket.Dispose();
                _clientMappers.Remove(wsw3.Key, out WebSocketWeb3Mapper webSocketWeb3Mapper);
            }
        }

        public async Task<long> GetNetWorkIdAsync()
        {
            var web3 = GetWeb3();
            var netWorkId = await web3.Net.Version.SendRequestAsync();
            return Convert.ToInt64(netWorkId);
        }

        private class WebSocketWeb3Mapper
        {
            public WebSocketClient WebSocket { get; set; }
            public Web3 Web3 { get; set; }
            public DateTime CreatedDate { get; set; }
        }
    }
}
