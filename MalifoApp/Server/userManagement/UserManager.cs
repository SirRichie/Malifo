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
				userList.Add(userInfo);
			}
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
		
		private UserInfo GetUserInfoByHash(string userHash){
			return (from info in userList
                    where info.SessionHash.Equals(userHash)
			                   select info).SingleOrDefault();
		}
	}
}
