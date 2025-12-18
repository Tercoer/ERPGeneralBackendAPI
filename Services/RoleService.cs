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

        public Task<object?> AddRoleAsync(ModelRoleDto model) {
            object? result = null;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT ISNULL(MAX(Id), 0) + 1 FROM Roles";
                        int id = (int)cmd.ExecuteScalar();

                        cmd.CommandText = "INSERT INTO Roles (Id, Name, Description, IsEnabled) " +
                                          "VALUES (@Id, @Name, @Description, 'True')";
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 20).Value = model.Name;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = model.Description;
                        cmd.ExecuteNonQuery();

                        result = new {
                            Id = id,
                            model.Name,
                            model.Description,
                            IsEnabled = true,
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

        public Task<ModelRole?> GetRoleAsync(byte id) {
            ModelRole? model = null;
            SqlDataReader? reader = null;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT Name, Description, IsEnabled " +
                                          "FROM Roles " +
                                          "WHERE Id = @Id ";
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                        reader = cmd.ExecuteReader();
                        if(reader != null && reader.Read()) {
                            model = new ModelRole();
                            model.Id = id;
                            model.Name = reader.SafeString(0);
                            model.Description = reader.SafeString(1);
                            model.IsEnabled = reader.SafeBool(2);
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

        public async Task<IResult?> GetRolesAsync() {
            List<ModelRole>? list = null;
            SqlDataReader? reader = null;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT Id, Name, Description, IsEnabled " +
                                          "FROM Roles ";
                        reader = cmd.ExecuteReader();
                        if(reader != null) {
                            list = new List<ModelRole>();
                            while(reader.Read()) {
                                ModelRole model = new ModelRole();
                                model.Id = reader.SafeByte(0);
                                model.Name = reader.SafeString(1);
                                model.Description = reader.SafeString(2);
                                model.IsEnabled = reader.SafeBool(3);
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
            IResult res = Results.Ok(list);
            return await Task.FromResult(res);
        }

        public Task<object?> PatchRoleAsync(ModelRole model) {
            object? result = null;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE Roles " +
                                          "SET Name = @Name, Description = @Description " +
                                          "IsEnabled = @IsEnabled " +
                                          "WHERE Id = @Id";

                        cmd.Parameters.Add("@Id", SqlDbType.TinyInt).Value = model.Id;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 20).Value = model.Name;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = model.Description;
                        cmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = model.IsEnabled;
                        
                        if(cmd.ExecuteNonQuery() > 0) {
                            result = new {
                                model.Id,
                                model.Name,
                                model.Description,
                                model.IsEnabled
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

        public Task<bool> DeleteRoleAsync(byte id) {
            bool result = false;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "DELETE FROM Roles WHERE Id = @Id";
                        cmd.Parameters.Add("@Id", SqlDbType.TinyInt).Value = id;

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
