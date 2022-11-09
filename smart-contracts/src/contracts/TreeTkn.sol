/* SPDX-License-Identifier: MIT */
pragma solidity ^0.8.17;


import "@openzeppelin/contracts/token/ERC20/extensions/ERC20Pausable.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
uint256 constant _BIP = 10000;
uint256 constant _USDfit = 2; // v.064 -> Decimales de la stablecoin
uint256 constant _ETHfit = 18; // v.064 

contract TreeERC20 is ERC20Pausable, Ownable {

uint8 _decimals;

/* 
Parámetros del constructor:
name_ > El nombre del token que se pasa a la extensión Detailed. Viene de #Project.FNFT_Name
symbol_ > El símbolo del token que se pasa a la extensión Detailed. Viene de #Project.FNFT_Symbol
mintage_ > Cantidad de tokens a crear en el momento de la construcción. En ForestMaker, esta cantidad NO se puede modificar en ningún momento de la vida del token, porque está atado a la superficie del bosque. Por eso se pasa al constructor de la extensión Capped. Viene de #Project.FNFT_MAX
decimals_ > ¿Puede valer 0? Se pasa a la extensión Detailed. Deberíamos definir una constante para esto. SI no, hay que agregarlo en #Project

Address de prueba (symbol -> fmk_000) = 0x5f6DF6aCA2B1D660f87413AC45791Aed16B2Ed90
*/
    constructor(string memory name_, string memory symbol_, uint256 mintage_, uint8 decimals_ ) 
    ERC20(name_, symbol_) {
    
    _decimals = decimals_;
    _mint(msg.sender, fitpot( mintage_, decimals())); 

    }

    // funciones para ajustar los decimales de las variables relacionadas con el TreeERC20 - v.064
    function fitpot( uint256 amt_, uint256 dec_) private pure returns (uint256) {
        return( amt_ * 10 ** dec_);
    }

    function fitsqr( uint256 amt_, uint256 dec_) private pure returns (uint256) {
        return( amt_ / 10 ** dec_);
    }

    function decimals() override public view returns (uint8) {
        return(_decimals);
    }

  
}
