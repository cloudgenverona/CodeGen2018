using ChatApp.Backend.Models.Groups;
using ChatApp.Backend.Stores;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Backend.Models.JoinGroups
{
    public class JoinGroupModels : GroupModel
    {
        [Required]
        public string Username { get; set; }
    }

    public class JoinGroupNotifyModel : GroupModel
    {
        public UserSignalR User { get; set; }
    }
}
