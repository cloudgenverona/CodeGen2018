using ChatApp.Backend.Hubs;
using ChatApp.Backend.Models.Groups;
using ChatApp.Backend.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace ChatApp.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private IHubContext<ChatHub> _chatHubContext;
        public GroupsController(IHubContext<ChatHub> chatHubContext)
        {
            _chatHubContext = chatHubContext;
        }
        [HttpPost]
        public IActionResult AddGroup(GroupModel group)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if(ChatStore.UsersByGroups.Find(x => x.GroupName == group.Group) == null)
            {
                var newGroup = new UserInGroup()
                {
                    GroupName = group.Group
                };
                ChatStore.UsersByGroups.Add(newGroup);
                var currentUser = ChatStore.UsersOnline.Find(x => x.Username == User.Identity.Name);
                if (currentUser != null)
                {
                    _chatHubContext.Clients.AllExcept(currentUser.ConnectionId).SendAsync("NewGroup", group);
                }
                
            }
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateGroup(GroupUpdateModel group)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var oldGroup = ChatStore.UsersByGroups.Find(x => x.GroupName == group.OldGroup);
            if (oldGroup == null)
                return NotFound();
            else
            {
                oldGroup.GroupName = group.Group;
                var currentUser = ChatStore.UsersOnline.Find(x => x.Username == User.Identity.Name);
                if (currentUser != null)
                {
                    _chatHubContext.Clients.AllExcept(currentUser.ConnectionId).SendAsync("UpdateGroup", group);
                }
            }
            return Ok();
        }

        [HttpDelete("{group}")]
        public IActionResult DeleteGroup(string group)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var groupToDelete = ChatStore.UsersByGroups.Find(x => x.GroupName == group);
            if (!groupToDelete.Users.Any())
            {
                ChatStore.UsersByGroups.Remove(groupToDelete);
                var currentUser = ChatStore.UsersOnline.Find(x => x.Username == User.Identity.Name);
                if (currentUser != null)
                {
                    _chatHubContext.Clients.AllExcept(currentUser.ConnectionId).SendAsync("DeleteGroup", new GroupModel() { Group = group });
                }
                return Ok();
            }
            return Conflict();
        }
    }
}