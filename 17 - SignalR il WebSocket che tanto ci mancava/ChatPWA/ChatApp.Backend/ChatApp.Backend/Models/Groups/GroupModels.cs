using System.ComponentModel.DataAnnotations;

namespace ChatApp.Backend.Models.Groups
{
    public class GroupModel
    {
        [Required]
        public string Group { get; set; }
    }

    public class GroupUpdateModel : GroupModel
    {
        [Required]
        public string OldGroup { get; set; }
    }
}
