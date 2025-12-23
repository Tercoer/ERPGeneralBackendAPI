
using Dapper;
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

        public async Task<bool> AddUserAsync(ModelUserAddDto model) {
            ModelCrypt crypt = Crypt.HashPassword(model.InputPassword);
            object objParam = new {
                model.RoleId,
                model.Username,
                PasswordHash = crypt.Hash,
                crypt.Salt,
                model.Email,
                model.Phone
            };
            using SqlConnection? conn = await _db.GetConnectionAsync();
            string cmd = @"INSERT INTO Users ( RoleId, Username, PasswordHash, Salt, CreatedAt, IsEnabled, Email, Phone) 
                           VALUES (@RoleId, @Username, @PasswordHash, @Salt, GETDATE(), 1, @Email, @Phone)";

            return await conn.ExecuteAsync(cmd, objParam) > 0;
        }

        public async Task<ModelUserInfoDto?> GetUserAsync(int id) {
            using SqlConnection? conn = await _db.GetConnectionAsync();
            string cmd = @"SELECT Id, RoleId, Username, CreatedAt, IsEnabled, Email, Phone 
                           FROM Users 
                           WHERE Id = @Id ";

            return await conn.QueryFirstOrDefaultAsync<ModelUserInfoDto>(cmd, new { Id = id });
        }

        public async Task<IEnumerable<ModelUserInfoDto>> GetUsersAsync() {
            using SqlConnection? conn = await _db.GetConnectionAsync();
            string cmd = @"SELECT Id, RoleId, Username, CreatedAt, IsEnabled, Email, Phone 
                           FROM Users ";

            return await conn.QueryAsync<ModelUserInfoDto>(cmd);
        }

        public async Task<bool> PatchUserAsync(ModelUserUpdateInputDto model) {
            using SqlConnection? conn = await _db.GetConnectionAsync();
            string cmd = @"UPDATE Users 
                           SET IsEnabled = @IsEnabled, Email = @Email, Phone = @Phone 
                           WHERE Id = @Id";

            return await conn.ExecuteAsync(cmd, model) > 0;
        }

        public async Task<bool> PatchUserRoleAsync(PatchModelUserRoleDto model) {
            using SqlConnection? conn = await _db.GetConnectionAsync();
            string cmd = @"UPDATE Users 
                           SET RoleId = @RoleId 
                           WHERE Id = @Id";

            return await conn.ExecuteAsync(cmd, model) > 0;
        }

        public async Task<bool> DeleteUserAsync(int id) {
            using SqlConnection? conn = await _db.GetConnectionAsync();
            string cmd = "DELETE FROM Users WHERE Id = @Id";

            return await conn.ExecuteAsync(cmd, new {Id=id}) > 0;
        }

    }
}
