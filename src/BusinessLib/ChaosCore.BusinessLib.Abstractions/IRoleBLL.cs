using System.Collections.Generic;
using ChaosCore.ModelBase;

namespace ChaosCore.BusinessLib
{
    public interface IRoleBLL :IBaseBLL<Role>
    {
        Result<IEnumerable<Role>> GetRoles();
        Result AddRole(Role role);
    }
}
