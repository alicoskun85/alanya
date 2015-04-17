using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Alanya.Hubs
{

    public class Users
    {
        public string ConID { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; set; }
    }
    public class ChatHub : Hub
    {
        public static Dictionary<string, List<Users>> AllConnectedUsers = new Dictionary<string, List<Users>>();

        public class MyMessage
        {
            public string Msg { get; set; }
            public string GroupName { get; set; }
            public string UserName { get; set; }
        }

        public async System.Threading.Tasks.Task Connect(string userName, string groupName)
        {
            string conId = Context.ConnectionId;

            List<Users> newUser = new List<Users>();
            newUser.Add(new Users { ConID = conId, UserName = userName, GroupName = groupName });
            //grup yoksa yeni grup oluştur, kullanıcıyı ekle
            if (AllConnectedUsers.Count(t => t.Key == groupName) == 0)
            {
                AllConnectedUsers.Add(groupName, newUser);
            }
            else
            {
                //grup var kullanıcı yoksa kullanıcıyı gruba ekle
                if (AllConnectedUsers.FirstOrDefault(a => a.Key == groupName).Value.Count(x => x.ConID == conId) == 0)
                {
                    AllConnectedUsers.FirstOrDefault(a => a.Key == groupName).Value.Add(new Users { ConID = conId, UserName = userName, GroupName = groupName });
                }
            }
            Clients.Caller.onConnected(userName, conId, groupName);

            await Groups.Add(Context.ConnectionId, groupName);
            Clients.Group(groupName).showAllUsers(AllConnectedUsers.FirstOrDefault(a => a.Key == groupName).Value);
            Clients.Group(groupName).sendMessage(userName + " joined.", "system");

            //Clients.Group(groupName).hello();

        }

        public void SendMessage(MyMessage Message)
        {
            //Clients.All.sendMessage(Message.Msg, Message.UserName);
            Clients.Group(Message.GroupName).sendMessage(Message.Msg, Message.UserName);
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {

            var group = AllConnectedUsers.SelectMany(a => a.Value).Where(b => b.ConID == Context.ConnectionId).Select(v => v.GroupName).FirstOrDefault();
            var cUser = AllConnectedUsers.SelectMany(a => a.Value).Where(b => b.ConID == Context.ConnectionId).Select(v => v).FirstOrDefault();
            //var user = AllConnectedUsers.Where(a => a.Value.Where(x => x.ConID.Equals(Context.ConnectionId)).FirstOrDefault().ConID.Equals(Context.ConnectionId)).FirstOrDefault();
            if (cUser != null)
            {
                List<Users> cUserListByGroup = AllConnectedUsers[cUser.GroupName];
                cUserListByGroup.Remove(cUser);
                AllConnectedUsers[cUser.GroupName] = cUserListByGroup;
                //AllConnectedUsers..Remove(user);
                var conId = Context.ConnectionId;
                Clients.All.onUserDisconnected(conId);
                Groups.Remove(Context.ConnectionId, cUser.GroupName);
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}