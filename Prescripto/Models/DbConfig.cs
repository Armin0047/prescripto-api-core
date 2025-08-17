using Microsoft.AspNetCore.Connections;
using System.Data.SqlClient;

namespace Prescripto.Models
{
    public class DbConfig
    {
        public string Username { get; set; }
        public string? Password { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
    }
}
