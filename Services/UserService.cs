
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using SistemaGeneral.Models;
using SistemaGeneral.Security;
using SistemaGeneral.Utility;
using System.Data;

namespace SistemaGeneral.Services {
    public class UserService {

        private DB _db;
        public UserService(DB db) {
            _db = db;
        }


        public bool conectarDB() {
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;

                        cmd.CommandText = "SELECT 'EXITO'";
                        string result = (string)cmd.ExecuteScalar();
                        return !string.IsNullOrEmpty(result);
                    }
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public Task<object?> AddUserAsync(ModelUserAddDto model) {
            ModelCrypt crypt = Crypt.HashPassword(model.InputPassword);
            object? result = null;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT ISNULL(MAX(Id), 0) + 1 FROM Users";
                        int id = (int)cmd.ExecuteScalar();

                        cmd.CommandText = "INSERT INTO Users (Id, RoleId, Username, PasswordHash, Salt, CreatedAt, IsEnabled, Email, Phone) " +
                                          "VALUES (@Id, @RoleId, @Username, @PasswordHash, @Salt, GETDATE(), 'True', @Email, @Phone)";
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = model.Username;
                        cmd.Parameters.Add("@PasswordHash", SqlDbType.VarChar).Value = crypt.Hash;
                        cmd.Parameters.Add("@Salt", SqlDbType.VarChar).Value = crypt.Salt;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = model.Email;
                        cmd.Parameters.Add("@Phone", SqlDbType.VarChar).Value = model.Phone;
                        cmd.Parameters.Add("@RoleId", SqlDbType.TinyInt).Value = -1; // undefined
                        cmd.ExecuteNonQuery();
                        result = new {
                            Id = id,
                            model.Username,
                            IsEnabled = true,
                            model.Email,
                            model.Phone,
                            CreatedAt = DateTime.Now
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

        public Task<ModelUserInfoDto?> GetUserAsync(int id) {
            ModelUserInfoDto? model = null;
            SqlDataReader? reader = null;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT RoleId, Username, CreatedAt, IsEnabled, Email, Phone " +
                                          "FROM Users " +
                                          "WHERE Id = @Id ";
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                        reader = cmd.ExecuteReader();
                        if(reader != null && reader.Read()) {
                            model = new ModelUserInfoDto();
                            model.Id = id;
                            model.RoleId = reader.SafeByte(0);
                            model.Username = reader.SafeString(1);
                            model.CreatedAt = reader.SafeDateTime(2);
                            model.IsEnabled = reader.SafeBool(3);
                            model.Email = reader.SafeString(4);
                            model.Phone = reader.SafeString(5);
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

        public Task<List<ModelUserInfoDto>?> GetUsersAsync() {
            List<ModelUserInfoDto>? users = null;
            SqlDataReader? reader = null;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT Id, RoleId, Username, CreatedAt, IsEnabled, Email, Phone " +
                                          "FROM Users ";
                        reader = cmd.ExecuteReader();
                        if(reader != null) {
                            users = new List<ModelUserInfoDto>();
                            while(reader.Read()) {
                                ModelUserInfoDto modelo = new ModelUserInfoDto();
                                modelo.Id = reader.SafeInt(0);
                                modelo.RoleId = reader.SafeByte(1);
                                modelo.Username = reader.SafeString(2);
                                modelo.CreatedAt = reader.SafeDateTime(3);
                                modelo.IsEnabled = reader.SafeBool(4);
                                modelo.Email = reader.SafeString(5);
                                modelo.Phone = reader.SafeString(6);
                                users.Add(modelo);
                            }
                        }
                    }
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex);
                users = null;
            }
            return Task.FromResult(users);
        }

        public Task<object?> PatchUserAsync(ModelUser model) {
            ModelCrypt crypt = Crypt.HashPassword(model.InputPassword);
            model.Salt = crypt.Salt;
            model.PasswordHash = crypt.Hash;
            object? result = null;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE Users " +
                                          "SET PasswordHash = @PasswordHash, Salt = @Salt, " +
                                          "IsEnabled = @IsEnabled, Email = @Email, Phone = @Phone " +
                                          "WHERE Id = @Id";

                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                        cmd.Parameters.Add("@PasswordHash", SqlDbType.VarChar).Value = model.PasswordHash;
                        cmd.Parameters.Add("@Salt", SqlDbType.VarChar).Value = model.Salt;
                        cmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = model.IsEnabled;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = model.Email;
                        cmd.Parameters.Add("@Phone", SqlDbType.VarChar).Value = model.Phone;

                        if(cmd.ExecuteNonQuery() > 0) {
                            result = new {
                                model.Id,
                                model.IsEnabled,
                                model.Email,
                                model.Phone
                            };
                        }
                        else
                            result = null;
                    }
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
                result = null;
            }
            return Task.FromResult(result);
        }

        public Task<object?> PatchUserRoleAsync(PatchModelUserRoleDto model) {
            object? result = null;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE Users " +
                                          "SET RoleId = @RoleId " +
                                          "WHERE Id = @Id";

                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                        cmd.Parameters.Add("@RoleId", SqlDbType.TinyInt).Value = model.RoleId;
                        
                        if(cmd.ExecuteNonQuery() > 0) {
                            result = new {
                                model.Id,
                                model.RoleId
                            };
                        }
                        else
                            result = null;
                    }
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
                result = null;
            }
            return Task.FromResult(result);
        }


        public Task<bool> DeleteUserAsync(int id) {
            bool result = false;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "DELETE FROM Users WHERE Id = @Id";
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

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
