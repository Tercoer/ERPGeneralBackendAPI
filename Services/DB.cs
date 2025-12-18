using Microsoft.Data.SqlClient;

namespace SistemaGeneral.Services {
    public class DB {

        public IConfiguration? _configuration;

        public DB(IConfiguration configuration) {
            _configuration = configuration;            
        }

        public SqlConnection? GetConnection() {
            try {
                SqlConnection conn = new SqlConnection(_configuration?.GetConnectionString("DefaultConnection"));
                conn.Open();
                return conn;
            }
            catch(Exception ex) {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
