using System.ComponentModel.DataAnnotations;

namespace JobFinder.Areas.Company.Models
{
    public class CompanyModel
    {
        public int CompanyID { get; set; }

        public int CityID { get; set; }

        public string CityName { get; set; }

        

        [Required]
        public string CompanyName { get; set;}

        public int Size { get; set;}

        public string Description { get; set;}

        //public string ContactPersonName { get; set;}

        public string ContactEmail { get; set;}

        public string ContactPhone { get; set;}

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public IFormFile? ImageFile { get; set; } // For file upload
        public string ContactPersonName { get; set; } // For storing image path in the database
        public string ImageUrl { get; set; }
        
    }

    public class CompanyDropDownModel
    {
     public int CompanyID { get; set;}

public string CompanyName { get; set;}
    }

}
