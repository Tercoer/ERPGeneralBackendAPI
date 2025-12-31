using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGeneral.Models;

namespace SistemaGeneral.Services {
    public class ProductsService {

        private DB _db;

        public ProductsService(DB db) {
            _db = db;
        }

        public async Task<IEnumerable<ModelProducts>> GetProductsAsync() {
            SqlConnection conn = await _db.GetConnectionAsync();
            string cmd = @"SELECT ID, CategoryID, BrandID, ProductSKU, Name, Description, 
                           Price, Cost, Enabled, CreationDate, LastModified 
                           FROM Products";                     
            return await conn.QueryAsync<ModelProducts>(cmd);
        }

        public async Task<bool> AddProductsAsync(ModelProductsDto model) {
            SqlConnection conn = await _db.GetConnectionAsync();
            string cmd = @"INSERT INTO Products 
                           (CategoryID, BrandID, ProductSKU, Name, Description, Price, Cost, Enabled) 
                           VALUES 
                           (@CategoryID, @BrandID, @ProductSKU, @Name, @Description, @Price, @Cost, @Enabled)";
            return await conn.ExecuteAsync(cmd, model) > 0;
        }
    }
}
