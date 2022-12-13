namespace ecomCapstone.Models
{
    public class Response
    {
        public bool Success { get; set; }

        public string Message { get; set; }
    }

    public class ResponseObj<T>
    {
        public List<T> Data { get; set; } = new List<T>();

        public bool Success { get; set; }

        public string Message { get; set; }
    }

    public class useroutdata
    {
        public Boolean isAdmin { get; set; } = false;
    }
}
