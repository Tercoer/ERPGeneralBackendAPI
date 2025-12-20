using Microsoft.Data.SqlClient;

namespace SistemaGeneral.Services {
    public class DB {

        public IConfiguration? _configuration;

        public DB(IConfiguration configuration) {
            _configuration = configuration;
        }

        public async Task<SqlConnection> GetConnectionAsync() {
            SqlConnection conn = new SqlConnection(_configuration?.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();
            return conn;
        }

        public SqlConnection GetConnection() {
            SqlConnection conn = new SqlConnection(_configuration?.GetConnectionString("DefaultConnection"));
            conn.Open();
            return conn;
        }
    }
}
