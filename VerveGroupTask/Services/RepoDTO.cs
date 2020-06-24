namespace VerveGroupTask.Web.Services
{
    public class RepoDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Fullname { get; set; }
        public string Description { get; set; }
        public int StargazersCount { get; set; }
        public string Url { get; set; }
        public int UserId { get; set; }
        public UserDTO Owner { get; set; }
    }
}