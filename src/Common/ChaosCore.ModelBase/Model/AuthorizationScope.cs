using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ChaosCore.ModelBase
{
    public class AuthorizationScope:BaseGuidEntity, IBaseGuidEntity
    {
        [StringLength(128)]
        public string Key { get; set; }
        [StringLength(128)]
        public string Description { get; set; }

        public virtual ICollection<ClientAuthorization> ClientAuthorizations { get; set; }
        public virtual ICollection<AuthorizationToken> AuthorizationTokens { get; set; }
    }
}
