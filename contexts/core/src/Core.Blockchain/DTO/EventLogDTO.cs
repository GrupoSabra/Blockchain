using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Core.Blockchain.DTO
{
    [FunctionOutput]
    [Event("Log")]
    public class EventLogDTO
    {
        [Parameter("uint256", "signatureId", 1, false)]
        public long SignatureId { get; set; }

        [Parameter("uint256", "id", 2, true)]
        public long Id { get; set; }

        [Parameter("string", "log",  3, false)]
        public string Log { get; set; }
        
         [Parameter("address", "sender", 4, false)]
         public string Sender { get; set; }
    }

}
