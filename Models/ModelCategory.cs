namespace SistemaGeneral.Models {
    public class ModelCategory {
        public short ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool Enabled { get; set; } = false;
    }

    public class ModelCategoryDTO {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool Enabled { get; set; } = false;
    }
}
