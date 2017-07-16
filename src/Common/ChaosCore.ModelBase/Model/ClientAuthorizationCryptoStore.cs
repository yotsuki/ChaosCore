using System;

namespace ChaosCore.ModelBase
{
    public class ClientAuthorizationCryptoStore
    {
        public Guid ClientAuthorizationId { get; set; }
        public Guid CryptoStoreId { get; set; }

        public virtual CryptoStore CryptoStore { get; set; }
        public virtual ClientAuthorization ClientAuthorization { get; set; }
    }
}
