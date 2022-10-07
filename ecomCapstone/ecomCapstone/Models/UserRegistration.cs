namespace ecomCapstone.Models
{
    public class UserRegistration
    {
        public int ID { get; set; }

    }

    public class Registration
    {
        public int ID { get; set; } = 0;

        public string name { get; set; } = string.Empty;

        public string email { get; set; } = string.Empty;

        public string password { get; set; } = string.Empty;

        public bool isActive { get; set; } = true;
    }
}
