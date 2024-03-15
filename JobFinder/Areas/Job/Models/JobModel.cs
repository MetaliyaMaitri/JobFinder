using System.ComponentModel.DataAnnotations;

namespace JobFinder.Areas.Job.Models
{
    public class JobModel
    {
        public int JobID { get; set; }
        [Required]
        public string JobType { get; set; }

        public int CompanyID { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Requirements { get; set; }
        [Required]
        public string Salary { get; set; }
        [Required]
        public string EmploymentType { get; set; }
        [Required]
        public string ExperienceLeval { get; set; }
        [Required]
        public string EducationLeval { get; set;}
        [Required]
     

        public DateTime CreatedDate  { get;set; }

        public DateTime ModifiedDate { get;set;}
    }
    public class CityDropDownModel
    {
        public int CityID { get; set; }

        public string CityName { get; set; }
    }
}
