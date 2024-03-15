using System.ComponentModel.DataAnnotations;

namespace JobFinder.Areas.Country.Models
{
    public class CountryModel
    {
       
        public int CountryID { get; set; }

        [Required(ErrorMessage = "Please Enter Country Name")]

        public string CountryName { get; set; }

        [Required]
        public string CountryCode { get; set; }
      
        public DateTime Created  {get;set;}

        public DateTime Modified { get;set;}
    }

    public class LOC_CountryDropDownModel
    {
        public int CountryID { get; set; }

        public string CountryName { get; set; }
    }
}
