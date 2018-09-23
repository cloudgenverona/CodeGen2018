using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Backend.Models.UserStats;
using ChatApp.Backend.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserStatsController : ControllerBase
    {
        public IActionResult GetStatistics(UserStatsRequestModels statsRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            else
            {
                switch (statsRequest.Type)
                {
                    case StatType.User:
                        var result = new UserStatsResponseModels<UserSignalR>(
                            ChatStore.UsersOnline.Where(x => x.Username != User.Identity.Name).Count(), 
                            ChatStore.UsersOnline.Where(x => x.Username != User.Identity.Name));
                        return Ok(result);
                    case StatType.Group:
                        return Ok(new UserStatsResponseModels<string>(
                            ChatStore.UsersByGroups.Count, 
                            ChatStore.UsersByGroups.Select(x => x.GroupName)));
                    case StatType.UserInGroup:
                        var group = ChatStore.UsersByGroups.FirstOrDefault(x => x.GroupName == statsRequest.Group);
                        if (group == null)
                            return NotFound();
                        else
                        {
                            var users = group.Users.Where(x => x.Username != User.Identity.Name);
                            return Ok(new UserStatsResponseModels<UserSignalR>(
                               users.Count(),
                               users.ToList()));
                        }
                           
                    default:
                        return BadRequest();
                }
            }
        }
    }
}