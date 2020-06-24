namespace VerveGroupTask.Web.Services
{
    public class RepoDTO
    { 
        public string Name { get; set; }
        public string Full_Name { get; set; }
        public string Description { get; set; }
        public int Stargazers_Count { get; set; }
        public string SVN_Url { get; set; }
        public int UserId { get; set; }
        public UserDTO Owner { get; set; }
    }
}