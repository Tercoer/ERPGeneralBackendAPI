using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using SistemaGeneral.Services;
using SistemaGeneral.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemaGeneral.Security {
    public class JWT {

        //CONFIGURAR CREDENCIALES DE JWT
        public static string? PASSWORD_JWT = Environment.GetEnvironmentVariable("PASSWORD_JWT");
        //public static string? ISSUER_JWT = Environment.GetEnvironmentVariable("ISSUER_JWT");
        //public static string? AUDIENCE_JWT = Environment.GetEnvironmentVariable("AUDIENCE_JWT");
        private DB _db;
        public JWT(DB db) {
            _db = db;
        }


        public IResult Login(ModelLogin login) {
            //Obtener usuario de la BD
            if(PASSWORD_JWT == null)
                return Results.Problem();

            ModelCrypt? crypt = LoginUser(login);

            bool isValidLogin = Crypt.VerifyPassword(crypt, login.Password);

            if(isValidLogin) {
                // Claims que se incluirán en el JWT
                Claim[] claims = [
                    new Claim(ClaimTypes.Name, login.Username),
                    new Claim(ClaimTypes.Role, "User")  // Se puede usar para restricciones de acceso
                ];

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(PASSWORD_JWT));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: "miapi.com",
                    audience: "miapi.com",
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds
                );
                string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

                return Results.Ok(new { token = tokenStr, expiresIn = DateTime.UtcNow.AddMinutes(30) });
            }
            return Results.Unauthorized();
        }


        public ModelCrypt? LoginUser(ModelLogin model) {
            SqlDataReader? reader = null;
            ModelCrypt? crypt = null;
            try {
                using(SqlConnection? conn = _db.GetConnection()) {
                    using(SqlCommand cmd = new SqlCommand()) {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT PasswordHash, Salt " +
                                          "FROM UserServices " +
                                          "WHERE Username=@Username";
                        cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = model.Username;
                        reader = cmd.ExecuteReader();
                        if(reader != null && reader.Read()) {
                            crypt = new ModelCrypt();
                            crypt.Hash = reader.GetString(0);
                            crypt.Salt = reader.GetString(1);
                        }
                    }
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex);
            }

            return crypt;
        }
    }
}
