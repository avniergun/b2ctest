using AutoMapper;
using B2C.DataAccess.Models;

namespace B2C.WepApi.Models.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerModel>();
            CreateMap<CustomerModel, Customer>();
            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();
            CreateMap<CustomerOrder, CustomerOrderModel>();
            CreateMap<CustomerOrderModel, CustomerOrder>();
            CreateMap<CustomerOrderDetail, CustomerOrderDetailModel>();
            CreateMap<CustomerOrderDetailModel, CustomerOrderDetail>();
        }
    }
}
