using DAL;
public static class SeedDataProvider
{
    private static readonly DateTime CreatedDate = new DateTime(2026, 3, 1, 10, 30, 0);

    /*------------------------------------------------------------------*/
    public static List<Categorie> GetCategories()
    {
        return new List<Categorie>()
        {
            new Categorie { Name = "Electronics",   Description = "Devices, gadgets, and accessories",       CreatedAt = CreatedDate },
            new Categorie { Name = "Clothing",      Description = "Men's, women's, and kids' apparel",        CreatedAt = CreatedDate },
            new Categorie { Name = "Books",         Description = "Fiction, non-fiction, and educational",    CreatedAt = CreatedDate },
            new Categorie { Name = "Home & Garden", Description = "Furniture, tools, and home decor",         CreatedAt = CreatedDate },
            new Categorie { Name = "Sports",        Description = "Equipment, clothing, and accessories",     CreatedAt = CreatedDate },
        };
    }

    /*------------------------------------------------------------------*/
    public static List<Product> GetProducts()
    {
        return new List<Product>()
        {
            new Product {  Name = "Laptop Pro 15",       Description = "High-performance laptop",          Price = 1299.99m, StockQuantity = 50,  CategorieId = 1, CreatedAt = CreatedDate },
            new Product {  Name = "Wireless Headphones", Description = "Noise-cancelling headphones",      Price = 199.99m,  StockQuantity = 120, CategorieId = 1, CreatedAt = CreatedDate },
            new Product {  Name = "Smartphone X12",      Description = "Latest flagship smartphone",       Price = 899.99m,  StockQuantity = 75,  CategorieId = 1, CreatedAt = CreatedDate },
            new Product {  Name = "Men's Casual Jacket", Description = "Slim-fit lightweight jacket",      Price = 89.99m,   StockQuantity = 200, CategorieId = 2, CreatedAt = CreatedDate },
            new Product {  Name = "Women's Running Shoes", Description = "Breathable sport shoes",         Price = 69.99m,   StockQuantity = 150, CategorieId = 2, CreatedAt = CreatedDate },
            new Product {  Name = "Clean Code",          Description = "By Robert C. Martin",              Price = 34.99m,   StockQuantity = 300, CategorieId = 3, CreatedAt = CreatedDate },
            new Product {  Name = "The Pragmatic Programmer", Description = "By David Thomas & Andy Hunt", Price = 39.99m,   StockQuantity = 250, CategorieId = 3, CreatedAt = CreatedDate },
            new Product {  Name = "Garden Hose 50ft",    Description = "Flexible and durable hose",        Price = 49.99m,   StockQuantity = 90,  CategorieId = 4, CreatedAt = CreatedDate },
            new Product {  Name = "Yoga Mat",            Description = "Non-slip premium yoga mat",        Price = 29.99m,   StockQuantity = 180, CategorieId = 5, CreatedAt = CreatedDate },
            new Product {  Name = "Dumbbell Set 20kg",   Description = "Adjustable weight dumbbell set",   Price = 119.99m,  StockQuantity = 60,  CategorieId = 5, CreatedAt = CreatedDate },
        };
    }
}