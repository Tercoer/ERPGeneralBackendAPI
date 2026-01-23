namespace SistemaGeneral.Models {
    public class ModelCategoryType {
        public short ID { get; set; } = 0;
        public short CategoryId { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool Enabled { get; set; } = false;
    }

    public class ModelCategoryTypeDTO {
        public short CategoryId { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool Enabled { get; set; } = false;
    }

}
