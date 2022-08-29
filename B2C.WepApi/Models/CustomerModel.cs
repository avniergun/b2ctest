namespace B2C.WepApi.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
    }
    public class CustomerUpdateModel { 
    public string? Address { get; set; }
    }
}
