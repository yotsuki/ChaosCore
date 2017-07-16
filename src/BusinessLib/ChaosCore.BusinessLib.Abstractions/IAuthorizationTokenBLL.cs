using ChaosCore.ModelBase;
using System;

namespace ChaosCore.BusinessLib
{
    public interface IAuthorizationTokenBLL : IBaseBLL<AuthorizationToken>
    {
        Result<AuthorizationToken> CreateToken(long userid,string token=null);

        Result<AuthorizationToken> GetOrCreateToken(long userid, string token = null);

        Result<User> CheckToken(string token);

        Result RemoveToken(string token);
    }
}
