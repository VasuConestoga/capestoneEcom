namespace ecomCapstone.Models
{
    public class ProductInput
    {
        public int ID { get; set; } = 0;

        public int pagefrom { get; set; } = 0;

        public string searchtext { get; set; } = string.Empty;

        public string category { get; set; } = string.Empty;
    }
}
