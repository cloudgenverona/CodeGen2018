using ChatApp.Backend.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Backend.Models
{
    public abstract class Message
    {
        public UserSignalR From { get; set; }
        public string TextMessage { get; set; }
    }
    public class PrivateMessageModel : Message
    {
        public UserSignalR To { get; set; }
    }

    public class GroupMessageModel : Message
    {
        public string Group { get; set; }
    }
}
