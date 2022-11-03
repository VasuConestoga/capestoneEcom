namespace ecomCapstone.Models
{
    public class ProductOutput<T>
    {
        public List<T> Data { get; set; } = new List<T>();

        public bool Success { get; set; } = false;

        public string Message { get; set; } = string.Empty;

    }


    public class ProductList
    {
        public int Id { get; set; } = 0;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int quantity { get; set; } = 0;
        public decimal price { get; set; } = 0;
        public string category { get; set; } = string.Empty;
        public string imageUrl { get; set; } = string.Empty;

    }



}
