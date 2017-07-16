using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChaosCore.ModelBase
{
    public class CryptoStore:BaseGuidEntity,IBaseGuidEntity
    {
        public byte[] Key { get; set; }
        public DateTime ExpiresUtc { get; set; }

        [StringLength(128)]
        public string Bucket { get; set; }
        [StringLength(128)]
        public string Handle { get; set; }
        [StringLength(1024)]
        public string ClientIdentifier { get; set; }
        [StringLength(1024)]
        public string UserName { get; set; }

        public ICollection<ClientAuthorizationCryptoStore> ClientAuthorizations { get; set; }
    }
}
