using System.ComponentModel.DataAnnotations;

namespace JobFinder.Areas.User_Index.Models
{
    public class User_IndexModel
    {
        public int StateID { get; set; }

        public int CountryID { get; set; }


        public string CountryName { get; set; }

        [Required]
        public string StateName { get; set; }
        [Required]
        public string StateCode { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

    }
    public class LOC_StateDropDownModel
    {
        public int StateID { get; set; }

        public string StateName { get; set; }
    }
}
