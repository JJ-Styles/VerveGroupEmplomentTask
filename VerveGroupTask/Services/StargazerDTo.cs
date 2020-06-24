namespace VerveGroupTask.Web.Services
{
    public class StargazerDTo
    {
        public string Username { get; set; }
        public int RepoID { get; set; }
        public RepoDTO Repo { get; set; }
    }
}