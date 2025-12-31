using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGeneral.Models;
using SistemaGeneral.Utility;
using System.Collections;
using System.Data;

namespace SistemaGeneral.Services {
    public class PermissionService {

        private DB _db;
        public PermissionService(DB db) {
            _db = db;
        }

        public async Task<IEnumerable<ModelPermission>> GetPermissionsAsync() {
            using SqlConnection? conn = await _db.GetConnectionAsync();
            string cmd = @"SELECT Id, Name, Description 
                         FROM Permissions ";

            return await conn.QueryAsync<ModelPermission>(cmd);
        }

        public async Task<IEnumerable<ModelPermission>> GetPermissionAsync(short id) {
            using SqlConnection? conn = await _db.GetConnectionAsync();
            string cmd = @"SELECT Id, Name, Description 
                           FROM Permissions 
                           WHERE Id = @Id ";

            return await conn.QueryAsync<ModelPermission>(cmd, new { Id = id });
        }

        public async Task<IEnumerable> GetPermissionsByRoleAsync(byte roleId) {
            using SqlConnection? conn = await _db.GetConnectionAsync();
            string cmd = @"SELECT P.Id, P.Name, P.Description 
                               FROM Permissions P 
                               INNER JOIN RolesPermissions RP ON RP.PermissionId = P.Id 
                               WHERE RoleId = @RoleId";

            return await conn.QueryAsync<ModelPermission>(cmd, new { RoleId = roleId });
        }

        public async Task<bool> AddPermissionAsync(ModelPermissionDto model) {
            using SqlConnection? conn = await _db.GetConnectionAsync();
            string cmd = @"INSERT INTO Permissions (Name, Description) 
                           VALUES (@Name, @Description)";

            return await conn.ExecuteAsync(cmd, model) > 0;
        }

        public async Task<bool> PatchPermissionAsync(ModelPermission model) {
            using SqlConnection? conn = await _db.GetConnectionAsync();
            string cmd = @"UPDATE Permissions 
                           SET Name = @Name, Description = @Description 
                           WHERE Id = @Id";

            return await conn.ExecuteAsync(cmd, model) > 0;
        }

        public async Task<bool> DeletePermissionAsync(short id) {
            using SqlConnection? conn = await _db.GetConnectionAsync();
            string cmd = "DELETE FROM Permissions WHERE Id = @Id";

            return await conn.ExecuteAsync(cmd, new {Id=id}) > 0;
        }


    }
}
