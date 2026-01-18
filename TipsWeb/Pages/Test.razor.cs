//using Microsoft.AspNetCore.Components;

//namespace YourApp.Pages
//{
//    public partial class Test : ComponentBase
//    {
//        // ========================================
//        // PARAMETERS
//        // ========================================

//        [Parameter]
//        public string? Category { get; set; }

//        // ========================================
//        // FIELDS
//        // ========================================

//        private List<Product> products = new();
//        private List<Product> filteredProducts = new();

//        // ========================================
//        // LIFECYCLE METHODS
//        // ========================================

//        protected override void OnInitialized()
//        {
//            LoadProducts();
//        }

//        protected override void OnParametersSet()
//        {
//            FilterProducts();
//        }

//        // ========================================
//        // DATA METHODS
//        // ========================================

//        private void LoadProducts()
//        {
//            products = new List<Product>
//            {
//                new() { Id = 1, Name = "Gaming Laptop", Category = "electronics", Price = 1499.99m, Description = "High-performance gaming laptop with RTX graphics" },
//                new() { Id = 2, Name = "Wireless Keyboard", Category = "electronics", Price = 79.99m, Description = "Mechanical keyboard with RGB lighting" },
//                new() { Id = 3, Name = "Office Desk", Category = "furniture", Price = 299.99m, Description = "Spacious L-shaped office desk" },
//                new() { Id = 4, Name = "Ergonomic Chair", Category = "furniture", Price = 349.99m, Description = "Premium ergonomic office chair" },
//                new() { Id = 5, Name = "Notebook Bundle", Category = "stationery", Price = 24.99m, Description = "Set of 5 premium notebooks" },
//                new() { Id = 6, Name = "Pen Set", Category = "stationery", Price = 15.99m, Description = "Professional fountain pen collection" },
//                new() { Id = 7, Name = "T-Shirt", Category = "clothing", Price = 29.99m, Description = "Premium cotton t-shirt" },
//                new() { Id = 8, Name = "Jeans", Category = "clothing", Price = 59.99m, Description = "Classic fit denim jeans" },
//                new() { Id = 9, Name = "Programming Book", Category = "books", Price = 49.99m, Description = "Complete guide to C# development" },
//                new() { Id = 10, Name = "Design Patterns", Category = "books", Price = 44.99m, Description = "Software design patterns explained" },
//                new() { Id = 11, Name = "4K Monitor", Category = "electronics", Price = 449.99m, Description = "27-inch 4K UHD monitor" },
//                new() { Id = 12, Name = "Bookshelf", Category = "furniture", Price = 189.99m, Description = "Modern 5-tier bookshelf" }
//            };

//            FilterProducts();
//        }

//        private void FilterProducts()
//        {
//            if (string.IsNullOrEmpty(Category))
//                filteredProducts = products;
//            else
//                filteredProducts = products
//                    .Where(p => p.Category.Equals(Category, StringComparison.OrdinalIgnoreCase))
//                    .ToList();
//        }

//        // ========================================
//        // HELPER METHODS
//        // ========================================

//        private string GetPageTitle()
//        {
//            if (string.IsNullOrEmpty(Category))
//                return "All Products";

//            return $"{char.ToUpper(Category[0])}{Category.Substring(1)} Products";
//        }

//        private int GetUniqueCategories()
//        {
//            return products
//                .Select(p => p.Category)
//                .Distinct()
//                .Count();
//        }

//        private string GetCategoryIcon(string category)
//        {
//            return category.ToLower() switch
//            {
//                "electronics" => "💻",
//                "furniture" => "🪑",
//                "stationery" => "✏️",
//                "clothing" => "👕",
//                "books" => "📚",
//                _ => "📦"
//            };
//        }

//        // ========================================
//        // EVENT HANDLERS
//        // ========================================

//        private void ViewProduct(int productId)
//        {
//            // Navigate to product detail page with route parameter
//            // NavigationManager.NavigateTo($"/product/{productId}");
//            Console.WriteLine($"Viewing product {productId}");
//        }

//        // ========================================
//        // MODELS
//        // ========================================

//        public class Product
//        {
//            public int Id { get; set; }
//            public string Name { get; set; } = "";
//            public string Category { get; set; } = "";
//            public decimal Price { get; set; }
//            public string Description { get; set; } = "";
//        }
//    }
//}