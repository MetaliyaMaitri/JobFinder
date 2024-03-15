namespace JobFinder.DAL
{
    public class DAL_Helper
    {
        public static string constring = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("myConnectionString");
    }
}
