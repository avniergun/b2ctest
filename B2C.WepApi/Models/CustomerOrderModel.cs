namespace B2C.WepApi.Models
{
    public class CustomerOrderModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? OrderCode { get; set; }
        public string? DeliveryAddress { get; set; }
        public decimal OrderAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public List<CustomerOrderDetailModel> CustomerOrderDetail { get; set; }
    }

    public class CustomerOrderDetailModel
    {
        public int Id { get; set; }
        public int CustomerOrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class CustomerOrderUpdateModel
    {
        public int Id { get; set; }
        public string? DeliveryAddress { get; set; }

        public List<CustomerOrderDetailModel> CustomerOrderDetail { get; set; }
    }
}
