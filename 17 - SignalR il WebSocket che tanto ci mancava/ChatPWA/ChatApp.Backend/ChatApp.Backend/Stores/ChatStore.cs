using System.Collections.Generic;

namespace ChatApp.Backend.Stores
{
    public static class ChatStore
    {
        public static List<UserInGroup> UsersByGroups { get; set; } = new List<UserInGroup>();
        public static List<UserSignalR> UsersOnline { get; set; } = new List<UserSignalR>();
    }

    public class UserInGroup
    {
        public string GroupName { get; set; }
        public List<UserSignalR> Users { get; set; } = new List<UserSignalR>();
    }

    public class UserSignalR
    {
        public UserSignalR() {}
        public UserSignalR(string username, string connectionId)
        {
            Username = username;
            ConnectionId = connectionId;
        }
        public string Username { get; set; }
        public string ConnectionId { get; set; }
    }
}
