using System.ComponentModel.DataAnnotations;

namespace JobFinder.Areas.City.Models
{
    public class CityModel
    {
        public int CityID { get; set; }

        public int StateID { get; set; }
        
        public string StateName { get; set; }
        
        public int CountryID { get; set; }

        
        public string CountryName { get; set; }
        
        public string CityName { get; set; }
        
        public string CityCode { get; set; }

        public DateTime CreationDate {  get; set; }

        public DateTime Modified { get; set; }

    }
    public class CityDropDownModel
    {
        public int CityID { get; set; }

        public string CityName { get; set; }
    }
}
