using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChaosCore.ModelBase
{
    public enum AuthorizationType
    {
        //
        // 摘要:
        //     An access token should be returned immediately.
        AccessToken = 0,
        //
        // 摘要:
        //     An authorization code should be returned, which can later be exchanged for refresh
        //     and access tokens.
        AuthorizationCode = 1
    }
    public class Client: BaseGuidEntity, IBaseGuidEntity
    {
        [StringLength(64)]
        public string ClientIdentifier { get; set; }
        [StringLength(64)]
        public string ClientSecret { get; set; }
        [StringLength(64)]
        public string Callback { get; set; }
        [StringLength(512)]
        [RegularExpression(@"^http[s]?://[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]+)+(:\d+)?$")]
        public string DomainName { get; set; }

        [StringLength(64)]
        public string Name { get; set; }
        public AuthorizationType ClientType { get; set; }

        public virtual ICollection<ClientAuthorization> Authorizations { get; set; }
    }
}
