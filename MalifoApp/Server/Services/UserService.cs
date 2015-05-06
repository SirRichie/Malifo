using Server.userManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class UserService
    {
        private UserManager _userManager;
        private ClientManager _clientManager;
        public UserService(UserManager userManager, ClientManager clientManager)
        {
            _userManager = userManager;
            _clientManager = clientManager;
        }

        public void UserLogin(string clientHash, string userName)
        {
            UserInfo userInfo = new UserInfo() { UserName = userName, SessionHash = clientHash };
            _userManager.AddUser(userInfo);
        }

        public void SendMessage()
        {

        }
    }
}
