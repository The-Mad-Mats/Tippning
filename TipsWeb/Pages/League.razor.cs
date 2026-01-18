namespace TipsWeb.Pages
{
    public partial class League
    {
        private List<Product> products = new();
        private List<Product> filteredProducts = new();
        private List<string> categories = new();
        private string selectedCategory = "";
        
        protected override void OnInitialized()
        {
            // Initialize sample data
            products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Laptop Pro",
                    Category = "Electronics",
                    Price = 1299.99m,
                    Stock = 15,
                    Description = "High-performance laptop for professionals"
                },
                new Product 
                {
                    Id = 2,
                    Name = "Wireless Mouse",
                    Category = "Electronics",
                    Price = 29.99m,
                    Stock = 50,
                    Description = "Ergonomic wireless mouse with precision tracking"
                },
                new Product
                {
                    Id = 3,
                    Name = "Office Chair",
                    Category = "Furniture",
                    Price = 249.99m,
                    Stock = 20,
                    Description = "Comfortable ergonomic office chair"
                },
                new Product
                {
                    Id = 4,
                    Name = "Desk Lamp",
                    Category = "Furniture",
                    Price = 45.99m,
                    Stock = 35,
                    Description = "Adjustable LED desk lamp"
                },
                new Product
                {
                    Id = 5,
                    Name = "Notebook Set",
                    Category = "Stationery",
                    Price = 12.99m,
                    Stock = 100,
                    Description = "Premium notebook set with 3 pieces"
                },
                new Product
                {
                    Id = 6,
                    Name = "Pen Collection",
                    Category = "Stationery",
                    Price = 8.99m,
                    Stock = 75,
                    Description = "Set of 10 ballpoint pens"
                },
                new Product
                {
                    Id = 7,
                    Name = "Monitor 27\"",
                    Category = "Electronics",
                    Price = 399.99m,
                    Stock = 25,
                    Description = "4K UHD monitor with HDR support"
                },
                new Product
                {
                    Id = 8,
                    Name = "Bookshelf",
                    Category = "Furniture",
                    Price = 159.99m,
                    Stock = 12,
                    Description = "Modern wooden bookshelf with 5 shelves"
                },
                new Product
                {
                    Id = 9,
                    Name = "Sticky Notes",
                    Category = "Stationery",
                    Price = 5.99m,
                    Stock = 200,
                    Description = "Colorful sticky notes pack"
                },
                new Product
                {
                    Id = 10,
                    Name = "Keyboard",
                    Category = "Electronics",
                    Price = 79.99m,
                    Stock = 40,
                    Description ="Mechanical keyboard with RGB lighting"
                }
            };
            // Get unique categories
            categories = products.Select(p => p.Category).Distinct().OrderBy(c => c).ToList();
            // Initially show all products
            filteredProducts = products;
        }
        private void OnCategoryChanged()
        {
            if(string.IsNullOrEmpty(selectedCategory))
            {
                filteredProducts = products;
            }
            else
            {
                filteredProducts = products.Where(p => p.Category == selectedCategory).ToList();
            }
        }
        public class Product
        {
            public int Id {get; set; }
            public string Name {get; set; } = string.Empty;
            public string Category {get; set; } = string.Empty;
            public decimal Price { get; set; } 
            public int Stock { get; set; }
            public string Description { get; set; } = string.Empty;
        }
    }
}