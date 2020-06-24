using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VerveGroupTask.Web.Services
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Avatar_Url { get; set; }
        public string Repos_Url { get; set; }
    }
}
