using ChaosCore.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ChaosCore.BusinessLib
{
    public class RoleBLL : BaseBLL<Role>, IRoleBLL
    {
        public RoleBLL() { }
        public RoleBLL(IServiceProvider provider) : base(provider){}

        public virtual Result AddRole(Role role)
        {
            BllRepository.Add(role);
            return SaveChanges();
        }

        public virtual Result<IEnumerable<Role>> GetRoles()
        {
            return BllRepository.GetQuery().ToList();
        }
    }
}
