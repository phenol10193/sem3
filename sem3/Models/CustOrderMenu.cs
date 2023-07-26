namespace sem3.Models
{
    public class CustOrderMenu
    {
        public int CustOrderMenuId { get; set; }
        public int MenuItemId { get; set; }
        public int CustOrderSuppId { get; set; }
        public bool Flag { get; set; }
    }
}
