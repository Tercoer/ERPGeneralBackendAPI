using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGeneral.EndPoints;
using SistemaGeneral.Models;
using SistemaGeneral.Utility;
using System.Data;

namespace SistemaGeneral.Services {
    public class RolePermissionService {

        private DB _db;
        public RolePermissionService(DB db) {
            _db = db;
        }

        public async Task<bool> AddRolePermissionAsync(ModelRolePermission model) {                    
            using SqlConnection? conn = await _db.GetConnectionAsync();
            string cmd = @"INSERT INTO RolesPermissions (RoleId, PermissionId) 
                           VALUES (@RoleId, @PermissionId)";

            return await conn.ExecuteAsync(cmd, model) > 0;
        }

        public async Task<bool> DeleteRolePermissionAsync(ModelRolePermission model) {
            using SqlConnection? conn = await _db.GetConnectionAsync();
            string cmd  = @"DELETE FROM RolesPermissions 
                            WHERE RoleId = @RoleId AND PermissionId = @PermissionId";

            return await conn.ExecuteAsync(cmd, model) > 0;
        }


    }
}
