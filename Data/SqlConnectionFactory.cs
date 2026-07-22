using System.Configuration;
using System.Data.SqlClient;

namespace InventoryManagement.Data
{
    public static class SqlConnectionFactory
    {
        public static SqlConnection Create()
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString;

            return new SqlConnection(connectionString);
        }
    }
}