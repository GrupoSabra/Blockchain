//const PrivateKeyProvider = require("truffle-privatekey-provider");
const HDWalletProvider = require("truffle-hdwallet-provider");

module.exports = {
    networks: {
        development: {
            host: "127.0.0.1",
            //host: "23.96.86.15",
            //from: "CA1C16d7d19d1823c8aF4796A993459a7FDBEF6E",
            port: 8545,
            network_id: "*",
            gas: 5000000,
                //websockets: true
        },
        ropsten: {
            provider: () => new HDWalletProvider("material fuel faith imitate antenna sure recycle enter clump firm wedding unfair spider advance tonight", "https://ropsten.infura.io/v3/acaeecb0717941a5b4beac9b366d2087"),
            gasPrice: 2000000000,
            gas: 15000000,
            network_id: "3"
                //websockets: true
        }
    },
    compilers: {
        solc: {
            version: "0.5.11", // Fetch exact version from solc-bin (default: truffle's version)
            optimizer: {
                enabled: true,
                runs: 200
            }
        }
    },
    plugins: [
        'truffle-plugin-verify'
    ],
    api_keys: {
        etherscan: '9JRZX8AUEX1RD8X5DA2RCASHW2668D2466'
    }
};