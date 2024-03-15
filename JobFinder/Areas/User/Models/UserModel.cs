namespace JobFinder.Areas.User.Models
{
    public class UserModel
    {
        public int UserID { get; set; }    

        public string UserName { get; set; } 

        public string Password { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public int CityID { get; set; }

        public string CityName { get; set; }

        public  string LinkedInProfile {  get; set; }

        public string Address { get; set; }

        public  string Resume { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime Modified { get; set;}


    }
}
