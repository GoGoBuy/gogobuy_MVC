using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gogobuy
{
    
    public class ChatHub : Hub
    {
        
        
        
        private static readonly Dictionary<string, string> users = new Dictionary<string, string>();

       
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            string username;
            //取得使用者名稱
            if (users.TryGetValue(Context.ConnectionId, out username))
            {
                //顯示使用者離開
                Clients.All.UserLeft(username);
                //Remove user from the server list
                users.Remove(Context.ConnectionId);

                //使用者列表
                List<string> userList = new List<string>();
                foreach (KeyValuePair<string, string> user in users)
                    userList.Add(user.Value);

                //更新使用者列表 
                Clients.All.UserList(userList, users.Count);

            }
            return base.OnDisconnected(stopCalled);
        }

        
        public void UserInformation(string username)
        {
            //確定使用者名稱
            bool results = (!users.ContainsValue(username));

            //更新結果給新使用者
            Clients.Caller.UserInfoResults(results);

            if (results)
            {
                //加入新使用者
                users.Add(Context.ConnectionId, username);

                //使用者列表
                List<string> userList = new List<string>();
                foreach (KeyValuePair<string, string> user in users)
                    userList.Add(user.Value);

                //更新使用者列表
                Clients.All.UserList(userList, users.Count);
                //廣播新使用者加入
                Clients.All.NewUser(username);
            }
        }

        
        public void MessageFromUser(string message)
        {
            string username;
            if (!users.TryGetValue(Context.ConnectionId, out username))
                username = "Unknown";
            Clients.All.MessageToUsers(username, message);
        }


        //附加訂單同步功能


        public class Product
        {
            public int ProductKey { get; set; }
            public String EnglishProductName { get; set; }
            public String ProductAlternatekey { get; set; }
            public String Color { get; set; }
        }


        public void updatedata(Product data)
        {
            Clients.All.update(data.ProductKey, data.ProductAlternatekey, data.EnglishProductName, data.Color);
        }

    }
}
   