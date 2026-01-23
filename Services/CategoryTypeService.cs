using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGeneral.Models;
using System.Reflection;

namespace SistemaGeneral.Services {
    public class CategoryTypeService {

        private DB _db;
        public CategoryTypeService(DB db) { 
            _db = db;
        }
        public async Task<IEnumerable<ModelCategoryType>> GetCategoryTypesAsync() {
            SqlConnection conn = await _db.GetConnectionAsync();
            string cmd = "SELECT ID, CategoryId, Name, Description, Enabled FROM CategorieTypes WHERE ID=@id";
            return await conn.QueryAsync<ModelCategoryType>(cmd);
        }

        public async Task<ModelCategoryType?> GetCategoryTypeAsync(short id) {
            SqlConnection conn = await _db.GetConnectionAsync();
            string cmd = "SELECT ID, CategoryId, Name, Description, Enabled FROM CategorieTypes WHERE ID=@id";
                        
            return await conn.QuerySingleOrDefaultAsync<ModelCategoryType>(cmd, new { id });
        }

        public async Task<bool> CreateCategoryTypeAsync(ModelCategoryTypeDTO model) {
            SqlConnection conn = await _db.GetConnectionAsync();
            string cmd = "INSERT INTO CategorieTypes (CategoryId, Name, Description, Enabled) VALUES (@CategoryId, @Name, @Description, @Enabled)";
            return await conn.ExecuteAsync(cmd, model) > 0;
        }

        public async Task<bool> UpdateCategoryTypeAsync(ModelCategoryType model) {
            SqlConnection conn = await _db.GetConnectionAsync();
            string cmd = @"UPDATE CategorieTypes 
                           SET CategoryId=@CategoryId, Name=@Name, Description=@Description, Enabled=@Enabled
                           WHERE ID=@ID";
            return await conn.ExecuteAsync(cmd, model) > 0;
        }

        public async Task<bool> DeleteCategoryTypeAsync(short ID) {
            SqlConnection conn = await _db.GetConnectionAsync();
            string cmd = @"DELETE FROM CategorieTypes WHERE ID=@ID";
            return await conn.ExecuteAsync(cmd, new {ID}) > 0;
        }
    }
}
