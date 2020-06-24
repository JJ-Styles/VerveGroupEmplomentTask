using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VerveGroupTask.Models
{
    public class Stargazers
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public int RepoID { get; set; }
        public Repos Repo { get; set; }
    }
}
