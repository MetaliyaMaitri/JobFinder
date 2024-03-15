using System.ComponentModel.DataAnnotations;

namespace JobFinder.Areas.Login.Models
{
    public class LoginModel
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

		[Required]
		public string Email { get; set; }

       
    }
}
