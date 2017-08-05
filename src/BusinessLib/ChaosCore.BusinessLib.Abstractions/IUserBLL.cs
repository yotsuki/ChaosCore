using ChaosCore.BusinessLib;
using ChaosCore.ModelBase;
using ChaosCore.ModelBase.Model;
using ChaosCore.ModelBase.Plugins;
using System;
using System.Collections.Generic;

namespace ChaosCore.BusinessLib
{
    /// <summary>
    /// 
    /// </summary>
    public class UserErrorCode : ErrorCode
    {
        public const byte UserNameOrPasswordError = USERCUSTOM + 0;
        public const byte NotFoundUser = USERCUSTOM + 1;
        public const byte NameOrEmailExist = USERCUSTOM + 2;
        public const byte OldPasswordEqualsNewPassword = USERCUSTOM + 3;
        public const byte PasswordError = USERCUSTOM + 4;
        public const byte UserLocked = USERCUSTOM + 5;
        public const byte AccountExist = USERCUSTOM + 6;
        public const byte DelelteSelfError = USERCUSTOM + 7;
        public const byte TokenErrorOrExprie = USERCUSTOM + 8;
    }
    public interface IUserBLL:IBaseBLL<User>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="password">密码(标准MD5加密)</param>
        /// <returns>大于0:用户ID ,0:用户不存在或密码错误,-1:用户已被锁定</returns>
        Result<User> Login(string name, string password);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="password">密码(标准MD5加密)</param>
        /// <returns>大于0:用户ID ,0:用户不存在或密码错误,-1:用户已被锁定</returns>
        Result<User> LoginByToken(string name, string token);
        Result<User> Update(User editedUser);

        Result UpdateFace(long id, string face);

        Result<User> SignIn(User user);
        Result ChangePassword(long userid, string oldPassword, string newPassword);
        Result ChangePasswordByName(string username, string oldPassword, string newPassword);

        Result<User> GetUser(string nameOrEmail, bool? bConfirm = null);
        Result<User> GetUserFromId(long userid);
        Result LockUser(long userid, bool isLock);
        Result DeleteUser(long userid);
        /// <summary>
        /// 设置用户所在用户组
        /// </summary>
        /// <param name="userid">用户编号</param>
        /// <param name="usergroupid">用户组编号</param>
        /// <returns>设置结果</returns>
        Result SetUserGroup(long userid, long usergroupid);
        /// <summary>
        /// 确认账户
        /// </summary>
        /// <param name="accountConfirmationToken"></param>
        /// <returns></returns>
        Result ConfirmAccount(string accountConfirmationToken);
        /// <summary>
        /// 创建账户
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Result<Account> CreateAccount(Account account);

        Result HasAccount(long userid);

        Result HasName(string name);

        Result HasEmail(string email);

        Result<FunctionTreeNode[]> GetFunctionTree(long userid);
        Result<FunctionTreeNode[]> GetFunctionTree(string username);
        Result<IEnumerable<User>> GetUsers(QuickQueryModel model);
        Result BatchUpdate(IEnumerable<UpdateModel<long>> updatemodels);
        Result BatchRemove(IEnumerable<long> idlist);
    }
}
