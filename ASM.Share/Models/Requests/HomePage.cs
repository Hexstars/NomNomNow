namespace ASM.Share.Models.Requests
{
    public class HomePage
    {
        public List<Product> Products { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
    }
}
