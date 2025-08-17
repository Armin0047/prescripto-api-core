using System.Data.SqlClient;

namespace Prescripto.Models
{
    public class DbConnectionInfo
    {
        private readonly IConfiguration _configuration;

        public DbConnectionInfo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool DynamicConnection(string server = @"192.168.1.98\sql2019", string database = "emamreza", string username = "sa", string password = "1")
            {
                var template = _configuration.GetConnectionString("DefaultConnectionTemplate");

                string connectionString = template
                    .Replace("{SERVER}", server)
                    .Replace("{DATABASE}", database)
                    .Replace("{USERNAME}", username)
                    .Replace("{PASSWORD}", password);

                try
                {
                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Connection failed: {ex.Message}");
                    return false;
                }
            }

            public string Build(string server = @"192.168.1.98\sql2019", string database = "emamreza", string username = "sa", string password = "1")
            {
                var template = _configuration.GetConnectionString("DefaultConnectionTemplate");

                if (string.IsNullOrEmpty(template))
                    throw new Exception("Connection template not found.");

                return template
                    .Replace("{SERVER}", server)
                    .Replace("{DATABASE}", database)
                    .Replace("{USERNAME}", username)
                    .Replace("{PASSWORD}", password);
        }
        
    }
}
