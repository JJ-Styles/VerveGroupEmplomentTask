using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VerveGroupTask.Models
{
    public class Stargazers
    {
        public string Login { get; set; }
        public int RepoID { get; set; }
        public Repos Repo { get; set; }
    }
}
