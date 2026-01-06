using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGeneral.Models;

namespace SistemaGeneral.Services {
    public class CategoryService {

        private DB _db;
        public CategoryService(DB db) { 
            _db = db;
        }

        public async Task<IEnumerable<ModelCategory>> GetCategoriesAsync() {
            SqlConnection conn = await _db.GetConnectionAsync();
            string cmd = "SELECT ID, Name, Description, Enabled FROM Categories";
            return await conn.QueryAsync<ModelCategory>(cmd);
        }

        public async Task<ModelCategory?> GetCategoryAsync(short ID) {
            SqlConnection conn = await _db.GetConnectionAsync();
            string cmd = "SELECT ID, Name, Description, Enabled FROM Categories WHERE ID = @ID";
            return await conn.QuerySingleOrDefaultAsync<ModelCategory>(cmd,   new { ID });
        }

        public async Task<bool> CreateCategoryAsync(ModelCategoryDTO model) {
            SqlConnection conn = await _db.GetConnectionAsync();
            string cmd = "INSERT INTO Categories (Name, Description, Enabled) VALUES (@Name, @Description, @Enabled)";
            return await conn.ExecuteAsync(cmd, model) > 0;
        }

        public async Task<bool> UpdateCategoryAsync(ModelCategory model) {
            SqlConnection conn = await _db.GetConnectionAsync();
            string cmd = "UPDATE Categories SET Name=@Name, Description=@Description, Enabled=@Enabled WHERE ID=@ID";
            return await conn.ExecuteAsync(cmd, model) > 0;
        }

        public async Task<bool> DeleteCategoryAsync(short ID) {
            SqlConnection conn = await _db.GetConnectionAsync();
            string cmd = "DELETE FROM Categories WHERE ID=@ID";
            return await conn.ExecuteAsync(cmd, new { ID }) > 0;
        }
    }


}
