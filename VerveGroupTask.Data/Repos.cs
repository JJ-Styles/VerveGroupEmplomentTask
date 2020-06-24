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
        public string Fullname { get; set; }
        public string Description { get; set; }
        public int StargazersCount { get; set; }
        public string Url { get; set; }
        public int UserId { get; set; }
        public User Owner { get; set; }
    }
}
