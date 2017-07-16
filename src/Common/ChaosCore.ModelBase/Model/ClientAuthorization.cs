using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChaosCore.ModelBase
{
    public class ClientAuthorization : BaseGuidEntity, IBaseGuidEntity
    {

        public virtual User User { get; set; }
        public virtual Client Client { get; set; }

        public virtual ICollection<AuthorizationScope> Scopes { get; set; }

        public DateTime ExpirationDateUtc { get; set; }

        [StringLength(1024)]
        public string AuthorizationCode { get; set; }

        public ICollection<ClientAuthorizationCryptoStore> CryptoStores { get; set; }
    }
}
