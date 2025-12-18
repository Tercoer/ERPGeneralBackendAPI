using Microsoft.Data.SqlClient;
using SistemaGeneral.Models;
using SistemaGeneral.Utility;
using System.Data;

namespace SistemaGeneral.Services {
    public class PermissionService {

        private DB _db;
        public PermissionService(DB db) {
            _db = db;
        }

        public Task<List<ModelPermission>?> GetPermissionsAsync() {
            List<ModelPermission>? list = null;
            SqlDataReader? reader = null;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT Id, Name, Description " +
                                          "FROM Permissions ";
                        reader = cmd.ExecuteReader();
                        if(reader != null) {
                            list = new List<ModelPermission>();
                            while(reader.Read()) {
                                ModelPermission model = new ModelPermission();
                                model.Id = reader.SafeShort(0);
                                model.Name = reader.SafeString(1);
                                model.Description = reader.SafeString(2);
                                list.Add(model);
                            }
                        }
                    }
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex);
                list = null;
            }

            return Task.FromResult(list);
        }

        public Task<ModelPermission?> GetPermissionAsync(short id) {
            ModelPermission? model = null;
            SqlDataReader? reader = null;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT Id, Name, Description " +
                                          "FROM Permissions " +
                                          "WHERE Id = @Id ";
                        cmd.Parameters.Add("@Id", SqlDbType.SmallInt).Value = id;
                        reader = cmd.ExecuteReader();
                        if(reader != null && reader.Read()) {
                            model = new ModelPermission();
                            model.Id = reader.SafeShort(0);
                            model.Name = reader.SafeString(1);
                            model.Description = reader.SafeString(2);
                        }
                    }
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex);
                model = null;
            }

            return Task.FromResult(model);
        }

        public Task<List<ModelPermission>?> GetPermissionsByRoleAsync(byte roleId) {
            List<ModelPermission>? list = null;
            SqlDataReader? reader = null;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT P.Id, P.Name, P.Description " +
                                          "FROM Permissions P " +
                                          "INNER JOIN RolesPermissions RP ON RP.PermissionId = P.Id " +
                                          "WHERE RoleId = @RoleId";
                        cmd.Parameters.Add("@RoleId", SqlDbType.TinyInt).Value = roleId;
                        reader = cmd.ExecuteReader();
                        if(reader != null) {
                            list = new List<ModelPermission>();
                            while(reader.Read()) {
                                ModelPermission model = new ModelPermission();
                                model.Id = reader.SafeShort(0);
                                model.Name = reader.SafeString(1);
                                model.Description = reader.SafeString(2);
                                list.Add(model);
                            }
                        }
                    }
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex);
                list = null;
            }

            return Task.FromResult(list);
        }

        public Task<object?> AddPermissionAsync(ModelPermissionDto model) {
            object? result = null;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT ISNULL(MAX(Id), 0) + 1 FROM Permissions";
                        int id = (int)cmd.ExecuteScalar();

                        cmd.CommandText = "INSERT INTO Permissions (Id, Name, Description) " +
                                          "VALUES (@Id, @Name, @Description)";
                        cmd.Parameters.Add("@Id", SqlDbType.SmallInt).Value = id;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 20).Value = model.Name;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = model.Description;
                        cmd.ExecuteNonQuery();

                        result = new {
                            Id = id,
                            model.Name,
                            model.Description
                        };

                    }
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
                result = null;
            }
            return Task.FromResult(result);
        }

        public Task<object?> PatchPermissionAsync(ModelPermission model) {
            object? result = null;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE Roles " +
                                          "SET Name = @Name, Description = @Description " +
                                          "WHERE Id = @Id";

                        cmd.Parameters.Add("@Id", SqlDbType.SmallInt).Value = model.Id;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 20).Value = model.Name;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = model.Description;

                        if(cmd.ExecuteNonQuery() > 0) {
                            result = new {
                                model.Id,
                                model.Name,
                                model.Description
                            };
                        }
                    }
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
                result = null;
            }
            return Task.FromResult(result);
        }

        public Task<bool> DeletePermissionAsync(short id) {
            bool result = false;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "DELETE FROM Permissions WHERE Id = @Id";
                        cmd.Parameters.Add("@Id", SqlDbType.SmallInt).Value = id;

                        result = cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
                result = false;
            }
            return Task.FromResult(result);
        }


    }
}
