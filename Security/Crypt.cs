using SistemaGeneral.Models;
using System.Security.Cryptography;

namespace SistemaGeneral.Security {
    public class Crypt {

        private const int SaltSize = 16; // 128 bits
        private const int KeySize = 32;  // 256 bits
        private const int Iterations = 100_000;

        public static ModelCrypt HashPassword(string password) {
            // Crear salt aleatorio
            using var rng = RandomNumberGenerator.Create();
            byte[] saltBytes = new byte[SaltSize];
            rng.GetBytes(saltBytes);

            // Derivar la clave
            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);
            byte[] hashBytes = pbkdf2.GetBytes(KeySize);
            ModelCrypt model = new ModelCrypt();

            // Codificar a Base64 para almacenar en SQL
            model.Hash = Convert.ToBase64String(hashBytes);
            model.Salt = Convert.ToBase64String(saltBytes);
            return model;
        }

        public static bool VerifyPassword(ModelCrypt? model, string password) {
            if(model == null)
                return false;
            byte[] saltBytes = Convert.FromBase64String(model.Salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);
            byte[] hashBytes = pbkdf2.GetBytes(KeySize);

            string computedHash = Convert.ToBase64String(hashBytes);
            return computedHash == model.Hash;
        }

    }
}
