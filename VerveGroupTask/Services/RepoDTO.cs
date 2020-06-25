using System.Collections.Generic;

namespace VerveGroupTask.Web.Services
{
    public class RepoDTO
    { 
        public string Name { get; set; }
        public string Full_Name { get; set; }
        public string Description { get; set; }
        public int Stargazers_Count { get; set; }
        public string SVN_Url { get; set; }
        public string UserLogin { get; set; }
        public UserDTO Owner { get; set; }
        public IEnumerable<StargazerDTO> Stargazers { get; set;}
    }
}