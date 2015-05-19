using Common.types.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.userManagement
{
	public sealed class UserManager
	{
		private static readonly Lazy<UserManager> _lazy =
			new Lazy<UserManager>(() => new UserManager());
	    
		public static UserManager Instance { get { return _lazy.Value; } }
		
		private List<UserInfo> userList;

		public List<UserInfo> UserList {
			get {
				return userList;
			}
		}
	
		private UserManager()
		{
			userList = new List<UserInfo>();
		}
		
		public void AddUser(UserInfo userInfo)
		{
			if (UserHashIsPersistend(userInfo)) {
				var tmpUserInfo = GetUserInfoByHash(userInfo.SessionHash);               
			}else{
                if (!UserNameAvailable(userInfo.UserName))
                {
                    throw new BusinessException(String.Format("UserNamer already taken: {}", userInfo.UserName));
                }
				userList.Add(userInfo);
			}
		}

        private bool UserNameAvailable(string userName)
        {
            foreach (UserInfo info in userList)
            {
                if (info.UserName.Equals(userName))
                {
                    return false;
                }
            }
            return true;
        }

		private bool UserHashIsPersistend(UserInfo userInfo)
		{
			if (userInfo.SessionHash == null) {
				return false;
			}
			
			var tmpUserInfo = GetUserInfoByHash(userInfo.SessionHash);
			
			if(tmpUserInfo == null){
				return false;
			}
			
			return true;					
		}
		
		public UserInfo GetUserInfoByHash(string userHash){
            var userInfos = (from info in userList
                             where info.SessionHash.Equals(userHash)
                             select info).ToList<UserInfo>();
            if (userInfos.Count != 1)
            {
                return null;
                
            }
            return userInfos[0];
		}
	}   
}
