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

        public Task<object> AddRolePermissionAsync(ModelRolePermission model) {
            object result;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO RolesPermissions (RoleId, PermissionId) " +
                                          "VALUES (@RoleId, @PermissionId)";
                        cmd.Parameters.Add("@RoleId", SqlDbType.TinyInt).Value = model.RoleId;
                        cmd.Parameters.Add("@PermissionId", SqlDbType.SmallInt).Value = model.PermisisonId;
                        cmd.ExecuteNonQuery();

                        result = new {
                            model.RoleId,
                            model.PermisisonId
                        };

                    }
                }
            }
            catch(SqlException ex) {
                result = new {
                    ErrorNumber = ex.Number,
                    Error = "A SqlException has occurred"
                };
            }
            catch(Exception ex) {
                result = new {
                    ErrorNumber = ex.HResult,
                    Error = "A non controlled exception by the system has occurred"
                };
            }
            return Task.FromResult(result);
        }

        public Task<bool> DeleteRolePermissionAsync(ModelRolePermission model) {
            bool result = false;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "DELETE FROM RolesPermissions " +
                                          "WHERE RoleId = @RoleId AND PermissionId = @PermissionId";
                        cmd.Parameters.Add("@RoleId", SqlDbType.TinyInt).Value = model.RoleId;
                        cmd.Parameters.Add("@PermissionId", SqlDbType.SmallInt).Value = model.PermisisonId;
                        result = cmd.ExecuteNonQuery() > 0;
                    }
                }

            }
            catch(Exception ex) {
                Console.WriteLine(ex);
            }

            return Task.FromResult(result);
        }


    }
}
