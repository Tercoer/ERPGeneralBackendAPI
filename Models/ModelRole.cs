namespace SistemaGeneral.Models {
    public class ModelRole {
        public byte Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsEnabled { get; set; } = false;
    }

    public class ModelRoleDto {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
    }

}
