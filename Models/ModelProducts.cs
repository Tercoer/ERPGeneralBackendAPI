namespace SistemaGeneral.Models {
    public class ModelProducts {
        public int ID { get; set; } = 0;
        public short CategoryID { get; set; } = 0;
        public int BrandID { get; set; } = 0;
        public string ProductSKU { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; } = 0;
        public decimal Cost { get; set; } = 0;
        public bool Enabled { get; set; } = false;
        public DateTime CreationDate { get; set; } 
        public DateTime LastModified { get; set; }        
    }

    public class ModelProductsUpdate {
        public int ID { get; set; } = 0;
        public short CategoryID { get; set; } = 0;
        public int BrandID { get; set; } = 0;
        public string ProductSKU { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; } = 0;
        public decimal Cost { get; set; } = 0;
        public bool Enabled { get; set; } = false;
    }

    public class ModelProductsDto {
        public short CategoryID { get; set; } = 0;
        public int BrandID { get; set; } = 0;
        public string ProductSKU { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; } = 0;
        public decimal Cost { get; set; } = 0;
        public bool Enabled { get; set; } = false;
    }
}
