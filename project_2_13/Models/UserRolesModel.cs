using System.Collections.Generic;

namespace project_2_13.Models
{
    public class UserRolesModel
    {
        public UserRolesModel()
        {
            Roles = new List<string>();
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
