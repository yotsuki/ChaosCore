using ChaosCore.CommonLib;
using ChaosCore.ModelBase;
using ChaosCore.ModelBase.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ChaosCore.Interception.Attributes;
using Dora.Interception;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ChaosCore.ModelBase.Model;
using ChaosCore.RepositoryLib;
using ChaosCore.ModelBase.Plugins;
using System.Linq.Expressions;

namespace ChaosCore.BusinessLib
{

    /// <summary>
    /// 用户业务类
    /// </summary>
    public class UserBLL : BaseBLL<User>, IUserBLL
    {
        public UserBLL(){}
        public UserBLL(IServiceProvider provider):base(provider){
        }
        #region 用户侧操作
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="password">密码(标准MD5加密)</param>
        /// <returns>1:用户不存在或密码错误,2:用户已被锁定,0:用户登陆成功</returns>
        public virtual Result<User> Login(string name, string password)
        {
            var result = new Result();
            var passwordEncrypt = MD5Ex.GetMd5Hash(password);
            var query = BllRepository.GetQuery(u => u.Name == name && u.Password == passwordEncrypt && !u.IsDeleted).Take(1).ToArray();
            if (query.Count() == 0) {
                //var query2 = from a in GetRep<Account>().GetQuery()
                //        where a.User.Name == name
                //        select a;
                //if (query2.Any()) {
                //    var account = query2.FirstOrDefault();
                //    account.PasswordFailuresSinceLastSuccess++;
                //    account.LastPasswordFailureDate = DateTime.UtcNow;
                //    SaveChanges();
                //}
                result.Code = UserErrorCode.UserNameOrPasswordError;
                result.Message = Res("UserNameOrPasswordError");
                return result.New<User>();
            }
            //var query3 = from a in GetRep<Account>().GetQuery()
            //            where a.User.Name == name && a.PasswordFailuresSinceLastSuccess != 0
            //            select a;
            //if (query3.Any()) {
            //    var account = query3.FirstOrDefault();
            //    account.PasswordFailuresSinceLastSuccess = 0;
            //    SaveChanges();
            //}
            var user = query.First();
            if (user.Lock){
                result.Code = UserErrorCode.UserLocked;
                return result.New<User>();
            }
            var token = ServiceProvider.GetService<IAuthorizationTokenBLL>().CreateToken(user.ID, Guid.NewGuid().ToString("N"));
            var newuser = new User() {
                Name = user.Name,
                ID = user.ID,
                Face = user.Face,
                Email = user.Email,
                Birthday = user.Birthday,
                Alias = user.Alias,
                Memo = user.Memo,
                Sex = user.Sex,
                Tel = user.Tel,
                Roles = BllRepository.GetQuery(u => u.ID == user.ID).Select(u => u.Roles).FirstOrDefault(),
                Password = $"Basic {token.Value.Token}",
            };

            return result.New(newuser);
        }
        public virtual Result<User> LoginByToken(string name, string token)
        {
            var result = new Result();
            var userresult = ServiceProvider.GetService<IAuthorizationTokenBLL>().CheckToken(token);
            if(userresult == null || userresult.Value == null || userresult.Value.Name != name) {
                result.Code = UserErrorCode.TokenErrorOrExprie;
                result.Message = Res("TokenErrorOrExprie");
                return result.New<User>();
            }
            var user = userresult.Value;
            var newuser = new User(){
                Name = user.Name,
                ID = user.ID,
                Face = user.Face,
                Email = user.Email,
                Birthday = user.Birthday,
                Alias = user.Alias,
                Memo = user.Memo,
                Sex = user.Sex,
                Tel = user.Tel,
                Roles = BllRepository.GetQuery(u => u.ID == user.ID).Select(u => u.Roles).FirstOrDefault(),
            };
            return result.New<User>(newuser);
        }
        public virtual Result<User> Update(User editedUser)
        {
            if (editedUser.ID == 0) {
                return new Result<User>(UserErrorCode.ID_ERROR, Res("IDisZero"));
            }
            var user = GetUser(editedUser.ID);
            if(user == null) {
                return new Result<User>(UserErrorCode.NotFoundUser, Res("NotFoundUser"));
            }
            user.CopyFrom(new { editedUser.Email, editedUser.Alias, editedUser.Birthday, editedUser.Memo, editedUser.Sex, editedUser.Tel });
            var result = SaveChanges();
            if(result){
                return user;
            }else{
                return result.New<User>();
            }
        }

        public virtual Result UpdateFace(long id, string face)
        {
            if(id == 0) {
                return new Result(UserErrorCode.ID_ERROR, Res("IDisZero"));
            }
            var user = GetUser(id);
            if(user == null) {
                return new Result(UserErrorCode.NotFoundUser, Res("NotFoundUser"));
           }
            user.Face = face;
            return SaveChanges();
        }
        [ErrorLogger("BLL")]
        [Transaction]
        public virtual Result<User> SignIn(User user)
        {
            var newUser = new User();
            if (BllRepository.GetQuery(u => u.Name == user.Name).Any()) {
                //邮件地址或用户名已存在
                return new Result<User>(UserErrorCode.NameOrEmailExist, Res("NameOrEmailExist"));
            }

            newUser.CopyFrom(new { user.Name, user.Alias, user.Birthday, user.Email, user.Memo, user.Sex, user.Tel });
            newUser.Password = MD5Ex.GetMd5Hash(user.Password);
            newUser.Lock = false;
            if (user.Roles == null) {
                return new Result<User>(UserErrorCode.PARAM_NULL, Res("ParamNull"));
            }
            if (user.Roles.Count > 0) {
                var ids = user.Roles.Select(r => r.RoleID).ToArray();
                var roles = GetRep<Role>().GetQuery(r => ids.Any(i => i == r.ID)).ToArray().Select(r=>new UserRole() { User =newUser,Role = r });
                newUser.Roles = roles.ToArray();
            }
            BllRepository.Add(newUser);
            var result = SaveChanges();
            if (result) {
                user.ID = newUser.ID;
                //if (newUser.Roles.Any(r => r.Role.Type == RoleType.Customer)) {
                //var bll = CreateIBLL<IConfigBLL>();
                //var config = bll.GetDefaultConfig();
                //config.UserID = user.ID;
                //bll.Add(config);
                //}
                //newUser.Roles = null;
                var token = ServiceProvider.GetService<IAuthorizationTokenBLL>().CreateToken(newUser.ID, Guid.NewGuid().ToString("N"));
                user.Password = $"Basic {token.Value.Token}";
                return user;
            } else {
                return result.New<User>();
            }
        }

        public virtual Result ChangePassword(long userid, string oldPassword, string newPassword)
        {
            if(userid == 0) {
                return new Result(UserErrorCode.ID_ERROR, Res("IDisZero"));
            }
            if (oldPassword.Equals(newPassword)) {
                return new Result(UserErrorCode.OldPasswordEqualsNewPassword, Res("OldPasswordEqualsNewPassword"));
            }
            var oldEncrypt = MD5Ex.GetMd5Hash(oldPassword);
            if (BllRepository.GetQuery(u => u.ID == userid && u.Password == oldEncrypt).Any()) {
                var newEncrypt = MD5Ex.GetMd5Hash(newPassword);
                var user = GetUser(userid);
                user.Password = newEncrypt;
                var tokenentity = GetRep<AuthorizationToken>().GetQuery(a => a.UserID == userid).FirstOrDefault();
                if (tokenentity != null) {
                    GetRep<AuthorizationToken>().Delete(tokenentity);
                }
               
                return SaveChanges();
            } else {
                //密码不正确
                return new Result(UserErrorCode.PasswordError, Res("PasswordError"));
            }
        }
        #endregion

        #region 管理员侧操作
        /// <summary>
        /// 根据ID获取用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public virtual User GetUser(long userid)
        {
            var user = BllRepository.Find(userid);
            return user;
        }
        [ErrorLogger("BLL")]
        public virtual Result<User> GetUserFromId(long userid)
        {
            Logger.LogInformation("start get user from {0}", userid);
            var user = GetUser(userid);
            if (user != null && !user.IsDeleted) {
                return user;
            }
            Logger.LogInformation(Res("IdNotFound"));
            return new Result<User>(UserErrorCode.NotFoundUser,Res("IdNotFound"));
        }
        public virtual Result<User> GetUser(string nameOrEmail, bool? bConfirm = null)
        {
            var query = BllRepository.GetQuery(u => u.Name == nameOrEmail || u.Email == nameOrEmail);
            User user = null;
            if (bConfirm != null && bConfirm.HasValue){
                var userQuery = from r in RepositoryFactory.CreateGenericRepository<Account>().GetQuery()
                           from u in query
                           where r.User.ID == u.ID && r.IsConfirmed == bConfirm.Value
                           select u;
                user = userQuery.FirstOrDefault();
            }else{
                user = query.FirstOrDefault();
            }
            if (user == null) {
                return new Result<User>(UserErrorCode.NotFoundUser, Res("NotFoundUser"));
            }
            return user;
        }

        public virtual Result LockUser(long userid, bool isLock)
        {
            if (userid == 0) {
                return new Result(UserErrorCode.ID_ERROR, "BllRes.IDisZero");
            }
            var user = GetUser(userid);
            if(user == null) {
                return new Result(UserErrorCode.NotFoundUser, "BllRes.NotFoundUser");
            }

            if(user.Lock == isLock) {
                //该用户已经锁定/解锁
                return false;
            }else {
                user.Lock = isLock;
                return SaveChanges();
            }
        }

        public virtual Result DeleteUser(long userid)
        {
            if(userid == 0) {
                return new Result(UserErrorCode.ID_ERROR, "BllRes.IDisZero");
            }
            if (userid == this.UserID) {
                return new Result(UserErrorCode.DelelteSelfError, "BllRes.DelelteSelfError");
            }
            var user = GetUser(userid);
            if (user == null) {
                return new Result(UserErrorCode.NotFoundUser, "BllRes.NotFoundUser");
            }
            user.IsDeleted = true;
            user.Name = string.Format("{0}_Removed_{1:YYMMDDhhmm}", user.Name, DateTime.UtcNow);
            return SaveChanges();
        }
#if DEBUG
        /// <summary>
        /// 真实删除用户数据
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public virtual Result DeleteUserTrue(long userid)
        {
            if (userid == 0) {
                return new Result(UserErrorCode.ID_ERROR, "BllRes.IDisZero");
            }
            if (userid == this.UserID) {
                return new Result(UserErrorCode.DelelteSelfError, "BllRes.DelelteSelfError");
            }
            var user = GetUser(userid);
            if (user == null) {
                return new Result(UserErrorCode.NotFoundUser, "BllRes.NotFoundUser");
            }
            BllRepository.Delete(user);
            return SaveChanges();
        }
#endif
        /// <summary>
        /// 设置用户所在用户组
        /// </summary>
        /// <param name="userid">用户编号</param>
        /// <param name="usergroupid">用户组编号</param>
        /// <returns>设置结果</returns>
        public virtual Result SetUserGroup(long userid, long usergroupid)
        {
            if (userid == 0 || usergroupid == 0) {
                return Result.New(UserErrorCode.ID_ERROR, "BllRes.IDisZero");
            }
            var user = GetUser(userid);
            if (user == null) {
                return Result.New(UserErrorCode.NotFoundUser, "BllRes.NotFoundUser");
            }
            var groupRep = RepositoryFactory.CreateGenericRepository<UserGroup>();
            var group = groupRep.Find(usergroupid);
            if(group == null) {
                return false;
            }
            //if(group.ID == user.UserGroup.ID)
            //user.UserGroup = group;

            return SaveChanges();
        }

        #endregion
        public virtual Result ChangePasswordByName(string username, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new Result(UserErrorCode.ID_ERROR, Res("IDisZero"));
            }
            if (oldPassword.Equals(newPassword))
            {
                return new Result(UserErrorCode.OldPasswordEqualsNewPassword, Res("OldPasswordEqualsNewPassword"));
            }
            var oldEncrypt = MD5Ex.GetMd5Hash(oldPassword);
            if (BllRepository.GetQuery(u => u.Name == username && u.Password == oldEncrypt).Any()){
                var newEncrypt = MD5Ex.GetMd5Hash(newPassword);
                var user = GetUser(username);
                user.Value.Password = newEncrypt;
                var tokenentity = GetRep<AuthorizationToken>().GetQuery(a => a.UserID == user.Value.ID).FirstOrDefault();
                if (tokenentity != null) {
                    GetRep<AuthorizationToken>().Delete(tokenentity);
                }
                //TODO: UserPasswordHistory 
                return SaveChanges();
            }
            else
            {
                //密码不正确
                return new Result(UserErrorCode.PasswordError, Res("PasswordError"));
            }
        }

        public virtual Result ConfirmAccount(string accountConfirmationToken)
        {
            throw new NotImplementedException();
        }
        public virtual Result<Account> CreateAccount(Account account)
        {
            var newAccount = new Account();
            var accountRep = GetRep<Account>();
            if (accountRep.GetQuery(a => a.User.ID == account.User.ID).Any()) {
                //Account已存在
                return new Result<Account>(UserErrorCode.AccountExist, Res("AccountExist"));
            }
            newAccount.CopyFrom(new {
                account.PasswordSalt,
                account.IsConfirmed,
                account.ConfirmationToken,
                account.CreateDate,
                account.PasswordChangedDate,
                account.PasswordFailuresSinceLastSuccess
            });

            newAccount.User = BllRepository.Find(account.User.ID);
            accountRep.Add(newAccount);
            var result = SaveChanges();
            if (result) {
                return newAccount;
            } else {
                return result.New<Account>();
            }
        }

        public virtual Result HasAccount(long userid)
        {
            return GetRep<Account>().GetQuery(a => a.User.ID == userid).Any();
        }

        public virtual Result HasName(string name)
        {
            return BllRepository.GetQuery().Any(u => u.Name == name);
        }

        public virtual Result HasEmail(string email)
        {
            return BllRepository.GetQuery().Any(u => u.Email == email);
        }

        private IEnumerable<User> ClearUserPW(IEnumerable<User> list)
        {
            foreach (var user in list) {
                user.Password = "";
            }
            return list;
        }

        private void CreateFunctionTreeNode(FunctionTreeNode parent, Function[] list)
        {
            parent.Children = list.Where(f => f.ParentFuncID == parent.ID).Select(f => new FunctionTreeNode(f)).ToArray();
            foreach (var child in parent.Children) {
                CreateFunctionTreeNode(child, list);
            }
        }
        public Result<FunctionTreeNode[]> GetFunctionTree(long userid)
        {
            var result = new Result<FunctionTreeNode[]>();
            if(userid == 0) {
                return result.Failure();
            }
            var query = from r in ServiceProvider.GetService<IRepository<Role>>().GetQuery()
                        from u in r.Users
                        from fr in r.Functions
                        where u.UserID == userid
                        select fr.Function;
            var functions = query.ToArray();
            var rootfuncs = functions.Where(f => f.ParentFuncID == 0).Select(f=> new FunctionTreeNode(f)).ToArray();

            foreach(var root in rootfuncs) {
                CreateFunctionTreeNode(root, functions);
            }

            return result.Final(rootfuncs);
        }
        public Result<FunctionTreeNode[]> GetFunctionTree(string username)
        {
            var result = new Result<FunctionTreeNode[]>();
            if (string.IsNullOrEmpty(username)) {
                return result.Failure();
            }
            var query = from r in ServiceProvider.GetService<IRepository<Role>>().GetQuery()
                        from u in r.Users
                        from fr in r.Functions
                        where u.User.Name == username && fr.Function.IsMenu
                        select fr.Function;
            var functions = query.ToArray();
            var rootfuncs = functions.Where(f => f.ParentFuncID == 0).Select(f => new FunctionTreeNode(f)).ToArray();

            foreach (var root in rootfuncs) {
                CreateFunctionTreeNode(root, functions);
            }

            return result.Final(rootfuncs);
        }
        public Result<IEnumerable<User>> GetUsers(QuickQueryModel model)
        {
            var result = new Result<IEnumerable<User>>();
            var mapping = new ResultMapping<User>();
            mapping.SetText("id", u => u.ID.ToString())
                    .SetText("username", u => u.Name)
                    .SetText("alias", u => u.Alias)
                    .SetText("email", u => u.Email)
                    .SetText("tel", u => u.Tel)
                    .SetText("workunit", u => u.WorkUnit)
                    .SetText("position", u => u.Position)
                    .SetText("sex", u => u.Sex == 0 ? "male" : "female")
                    .SetText("state", u => u.Lock ? "未激活" : "已激活")
                    .SetWhere("state",(q,v)=> {
                        if (v == "已激活") {
                            q = q.Where(u => !u.Lock);
                        } else if(v == "未激活") {
                            q = q.Where(u => u.Lock);
                        }
                        return q;
                    })
                    .SetWhere("searchkey", (q,v)=> string.IsNullOrEmpty(v)?q:q.Where(u=>u.Alias==v||u.Tel==v))
                    .MemberBaseEntity(true);
            Expression<Func<User, User>> expr = (u) => new User() { Alias = u.Alias,Name = u.Name };
            var users = mapping.Query(model, BllRepository.GetQuery()); //QueryResultModel.FormRequest(model, BllRepository.GetQuery(), mapping);
            result.Value = users;
            return result;
        }

        public virtual Result BatchUpdate(IEnumerable<UpdateModel<long>> updatemodels)
        {
            var result = new Result();
            if (updatemodels == null) {
                return result;
            }
            foreach (var model in updatemodels) {
                var entity = new User() { ID = model.ID };
                var entityEntry = BllRepository.Attach(entity);
                foreach (var column in model.Columns) {
                    switch (column.Name) {
                        case "lock":
                            bool islock = false;
                            if (bool.TryParse(column.Value, out islock)) {
                                entity.Lock = islock;
                                entityEntry.Property("Lock").IsModified = true;
                            }
                            break;
                    }
                }
            }
            result = SaveChanges();
            return result;
        }

        public Result BatchRemove(IEnumerable<long> idlist)
        {
            var result = new Result();
            if (idlist == null) {
                return result;
            }
            foreach (var id in idlist) {
                //var entity = new User() { ID = id };
                //var entityEntry = BllRepository.Attach(entity);
                BllRepository.Delete(id);
            }
            result = SaveChanges();
            return result;
        }
    }
}
