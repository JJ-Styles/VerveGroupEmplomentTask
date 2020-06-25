using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VerveGroupTask.Models
{
    public class Repos
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Full_Name { get; set; }
        public string Description { get; set; }
        public int Stargazers_Count { get; set; }
        public string Svn_Url { get; set; }
        public string UserLogin { get; set; }
        public User Owner { get; set; }
    }
}
