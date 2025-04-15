namespace ASM.Share.Models.Responses
{
    public class ConfirmOrder
    {
        public Account Account { get; set; }
        public List<ASM.Share.Models.CartProduct> CartProducts { get; set; }
    }
}
