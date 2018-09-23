using ChatApp.Backend.Models;
using ChatApp.Backend.Models.JoinGroups;
using ChatApp.Backend.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Backend.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var currentUser = new UserSignalR(Context.User.Identity.Name, Context.ConnectionId);
            ChatStore.UsersOnline.Add(currentUser);
            NewConnectedUser(currentUser);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var currentUser = ChatStore.UsersOnline.Find(x => x.Username == Context.User.Identity.Name);
            if(currentUser != null)
            {
                ChatStore.UsersOnline.Remove(currentUser);
                RemoveFromGroup();
            }
            NewDisconnectedUser(currentUser);
            return base.OnDisconnectedAsync(exception);
        }

        #region SignalR
        public void AddPrivateMessage(PrivateMessageModel message)
        {
            Clients.Client(GetConnectionId(message.To.Username)).SendAsync("ReceivePrivateMessage", message);
        }

        public void AddGroupMessage(GroupMessageModel message)
        {
            var groupExist = ChatStore.UsersByGroups.FirstOrDefault(x => x.GroupName == message.Group);
            if(groupExist != null)
            {
                //Clients.Group(message.Group).SendAsync("ReceiveGroupMessage", message);
                Clients.Clients(groupExist.Users
                                          .Where(x => x.ConnectionId != Context.ConnectionId)
                                          .Select(x => x.ConnectionId)
                                          .ToList())
                       .SendAsync("ReceiveGroupMessage", message);
            }
        }

        private void NewConnectedUser(UserSignalR currentUser)
        {
            Clients.Others.SendAsync("NewConnectedUser", currentUser);
        }

        private void NewDisconnectedUser(UserSignalR currentUser)
        {
            Clients.Others.SendAsync("NewDisconnectedUser", currentUser);
        }
        #endregion

        #region Utility
        public UserSignalR GetUserContext()
        {
            return new UserSignalR(Context.User.Identity.Name, Context.ConnectionId);
        }

        private string GetConnectionId(string username)
        {
            return ChatStore.UsersOnline.FirstOrDefault(x => x.Username == username).ConnectionId;
        }

        private void RemoveFromGroup()
        {
            // Remove Disconnected User
            for (int i = 0; i < ChatStore.UsersByGroups.Count; i++)
            {
                var currentGroup = ChatStore.UsersByGroups.ElementAt(i);
                var userToRemove = currentGroup.Users.FirstOrDefault(t => t.Username == Context.User.Identity.Name);
                if (userToRemove != null)
                {
                    currentGroup.Users.Remove(userToRemove);
                }
            }

            // Remove Empty Group
            var groupsToRemove = ChatStore.UsersByGroups.Where(x => !x.Users.Any());
            for (int i = 0; i < groupsToRemove.Count(); i++)
            {
                ChatStore.UsersByGroups.Remove(groupsToRemove.ElementAt(i));
            }
        }
        #endregion
    }
}
