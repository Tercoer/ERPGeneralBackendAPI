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
            object? id = await conn.ExecuteScalarAsync(cmd, model);

            if(id == null)
                return null;

            return new ModelRole {
                Id = (byte)id,
                Name = model.Name,
                Description = model.Description,
                IsEnabled = true,
            };
        }

        public async Task<ModelRole?> GetRoleAsync(byte id) {

            using SqlConnection? conn = await _db.GetConnectionAsync();
            using SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT Name, Description, IsEnabled " +
                                "FROM Roles " +
                                "WHERE Id = @Id ";
            cmd.Parameters.Add("@Id", SqlDbType.TinyInt).Value = id;

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if(!await reader.ReadAsync())
                return null;
            return new ModelRole() {
                Id = id,
                Name = reader.SafeString(0),
                Description = reader.SafeString(1),
                IsEnabled = reader.SafeBool(2)
            };
        }

        public async Task<List<ModelRole>> GetRolesAsync() {

            using SqlConnection? conn = await _db.GetConnectionAsync();
            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT Id, Name, Description, IsEnabled " +
                              "FROM Roles ";
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            List<ModelRole> list = new List<ModelRole>();
            while(await reader.ReadAsync()) {
                ModelRole model = new ModelRole();
                model.Id = reader.SafeByte(0);
                model.Name = reader.SafeString(1);
                model.Description = reader.SafeString(2);
                model.IsEnabled = reader.SafeBool(3);
                list.Add(model);
            }
            return list;
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
