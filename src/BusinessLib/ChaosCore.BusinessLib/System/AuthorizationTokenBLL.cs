using ChaosCore.CommonLib;
using ChaosCore.ModelBase;
using System;
using System.Linq;

namespace ChaosCore.BusinessLib
{
    public class AuthorizationTokenBLL : BaseBLL<AuthorizationToken>, IAuthorizationTokenBLL
    {
        public AuthorizationTokenBLL() { }
        public AuthorizationTokenBLL(IServiceProvider provider):base(provider) { }
        public int Expires { get; set; } = 60*24*30;
        public virtual Result<User> CheckToken(string token)
        {
            var result = new Result<User>();
            if (string.IsNullOrEmpty(token)) {
                result.Code = -1;
                result.Message = Res("TokenIsEmpty");
                return result;
            }

            var entity = BllRepository.GetQuery(a => a.Token == token).FirstOrDefault();
            if (entity == null) {
                result.Code = -2;
                result.Message = Res("TokenNotFound");// "token not found.";
                return result;
            }

            if (entity.Expires < DateTime.UtcNow) {
                result.Code = -3;
                result.Message = Res("TokenIsExpired");//"token is expired. plase use refresh token to get new toekn.";
                return result;
            }

            var user = GetRep<User>().GetIncludeQuery("Roles.Role").FirstOrDefault(u => u.ID == entity.UserID);
            if (user == null) {
                result.Code = -4;
                result.Message = Res("UserNotFound");//"user not found.";
                return result;
            }
            user.Password = string.Empty;

            return user;
        }

        public virtual Result RemoveToken(string token)
        {
            if (string.IsNullOrEmpty(token)) {
                return null;
            }

            var entity = BllRepository.GetQuery(a => a.Token == token).FirstOrDefault();
            if (entity == null) {
                return null;
            }

            BllRepository.Delete(entity);
            SaveChanges();
            return true;
        }

        public virtual Result<AuthorizationToken> CreateToken(long userid, string token = null)
        {
            var rep = BllRepository;
            var entity = BllRepository.GetQuery(a => a.UserID == userid).FirstOrDefault();
            if (entity == null) {
                entity = new AuthorizationToken() { UserID = userid, Token = token ?? Guid.NewGuid().ToString("N"), Expires = DateTimeHelper.GetNow().AddMinutes(Expires) };
                BllRepository.Add(entity);
            } else {
                entity.Token = token;
                entity.Expires = DateTimeHelper.GetNow().AddMinutes(Expires);
            }
            var save = SaveChanges();
            return save.New(entity);
        }

        public Result<AuthorizationToken> GetOrCreateToken(long userid, string token = null)
        {
            var result = new Result<AuthorizationToken>();
            var rep = BllRepository;
            var entity = BllRepository.GetQuery(a => a.UserID == userid).FirstOrDefault();
            if (entity == null) {
                entity = new AuthorizationToken() { UserID = userid, Token = token ?? Guid.NewGuid().ToString("N"), Expires = DateTimeHelper.GetNow().AddMinutes(Expires) };
                BllRepository.Add(entity);
                result.Final(SaveChanges());
                result.Value = entity;
            } else {
                entity.Expires = DateTimeHelper.GetNow().AddMinutes(Expires);
                result.Value = entity;
            }
            return result;
        }
    }
}
