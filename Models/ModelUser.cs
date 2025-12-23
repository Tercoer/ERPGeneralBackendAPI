namespace SistemaGeneral.Models {

    // SIRVE PARA HACER POST
    public class ModelUser {
        public int Id { get; set; } = 0;
        public byte RoleId { get; set; } = 0;
        public string Username { get; set; } = "";
        public string InputPassword { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Salt { get; set; } = "";
        public DateTime CreatedAt { get; set;}
        public bool IsEnabled { get; set; } = false;
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
    }

    public class PatchModelUserRoleDto {
        public int Id { get; set; } = 0;
        public byte RoleId { get; set; } = 0;
    }

    public class ModelUserAddDto {
        public byte RoleId { get; set; } = 0;
        public string Username { get; set; } = "";
        public string InputPassword { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
    }

    // SIRVE PARA MOSTRAR EN: PATCH
    public class ModelUserUpdateResponseDto {
        public int Id { get; set; } = 0;
        public bool IsEnabled { get; set; } = false;
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
    }

    public class ModelUserUpdateInputDto {
        public int Id { get; set; } = 0;
        public bool IsEnabled { get; set; } = false;
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
    }


    // LISTA DE USUARIOS SIN EXPONER INFORMACION SENSIBLE
    // SIRVE PARA MOSTRAR EN: GET, POST (SOLO COMO RESULTADO)
    public class ModelUserInfoDto {
        public int Id { get; set; } = 0;
        public byte RoleId { get; set; } = 0;
        public string Username { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public bool IsEnabled { get; set; } = false;
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
    }


}
