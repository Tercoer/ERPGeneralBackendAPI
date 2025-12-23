using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using SistemaGeneral.Models;
using SistemaGeneral.Security;
using SistemaGeneral.Utility;
using System.Data;

namespace SistemaGeneral.Services {
    public class RoleService {

        private DB _db;
        public RoleService(DB db) {
            _db = db;
        }

        public async Task<ModelRole?> AddRoleAsync(ModelRoleDto model) {
            await using SqlConnection? conn = await _db.GetConnectionAsync();

            string cmd = "INSERT INTO Roles (Name, Description, IsEnabled) " +
                         "OUTPUT INSERTED.Id " +
                         "VALUES (@Name, @Description, 1)";
            byte id = await conn.ExecuteScalarAsync<byte>(cmd, model);
            if(id < 1)
                return null;
            return new ModelRole {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                IsEnabled = true,
            };
        }

        public async Task<ModelRole?> GetRoleAsync(byte id) {

            using SqlConnection? conn = await _db.GetConnectionAsync();

            string cmd = "SELECT Id, Name, Description, IsEnabled " +
                         "FROM Roles " +
                         "WHERE Id = @Id ";

            return await conn.QuerySingleOrDefaultAsync<ModelRole>(cmd, new { Id = id });
        }

        public async Task<IEnumerable<ModelRole>> GetRolesAsync() {
            using SqlConnection? conn = await _db.GetConnectionAsync();

            string cmd = "SELECT Id, Name, Description, IsEnabled " +
                              "FROM Roles ";

            return await conn.QueryAsync<ModelRole>(cmd);
        }

        public async Task<ModelRole?> PatchRoleAsync(ModelRole model) {
            using SqlConnection? conn = await _db.GetConnectionAsync();

            string cmd = "UPDATE Roles " +
                         "SET Name = @Name, Description = @Description, " +
                         "IsEnabled = @IsEnabled " +
                         "WHERE Id = @Id";

            if(await conn.ExecuteAsync(cmd, model) < 1)
                return null;

            return model;
        }

        public async Task<bool> DeleteRoleAsync(byte id) {
            using SqlConnection? conn = _db.GetConnection();
            string cmd = "DELETE FROM Roles WHERE Id = @Id";

            return await conn.ExecuteAsync(cmd, new { Id = id }) > 0;
        }
    }
}
