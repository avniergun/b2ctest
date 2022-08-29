using AutoMapper;
using B2C.Core.Repositories;
using B2C.DataAccess.Models;
using B2C.WepApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace B2C.WepApi.Controllers
{
    [ApiController]
    public class CustomerOrderController : ControllerBase
    {
        private readonly ILogger<Product> _logger;
        IMapper _mapper;
        ICustomerReadRepository _customerRead;
        IProductReadRepository _productRead;
        ICustomerOrderReadRepository _customerOrderRead;
        ICustomerOrderWriteRepository _customerOrderWrite;
        ICustomerOrderDetailReadRepository _customerOrderDetailRead;
        ICustomerOrderDetailWriteRepository _customerOrderDetailWrite;
        public CustomerOrderController(ILogger<Product> logger, IMapper mapper, ICustomerOrderReadRepository customerOrderRead, ICustomerOrderWriteRepository customerOrderWrite,
                    ICustomerOrderDetailReadRepository customerOrderDetailRead, ICustomerOrderDetailWriteRepository customerOrderDetailWrite, IProductReadRepository productRead, ICustomerReadRepository customerRead
            )
        {
            _logger = logger;
            _mapper = mapper;
            _customerRead = customerRead;
            _productRead = productRead;
            _customerOrderRead = customerOrderRead;
            _customerOrderWrite = customerOrderWrite;
            _customerOrderDetailRead = customerOrderDetailRead;
            _customerOrderDetailWrite = customerOrderDetailWrite;
        }


        /// <summary>
        /// Tüm CustomerOrder listesini getirir. 
        /// ApiKey = B2CJWTSECRET
        /// </summary>
        [HttpGet]
        [Route("orders")]
        public IActionResult GetAllOrder([FromHeader] string ApiKey)
        {
            ResponseModel response = new();
            try
            {
                var listOrder = _customerOrderRead.GetAll(false).ToList();
                foreach (var order in listOrder)
                {
                    order.CustomerOrderDetail = _customerOrderDetailRead.GetWhere(x => x.CustomerOrderId == order.Id, false).ToList();
                }
                response.Data = listOrder;

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Hata: {ex.Message}";
            }

            return Ok(response);
        }


        /// <summary>
        /// Bir Customerın tüm Order listesini getirir. 
        /// ApiKey = B2CJWTSECRET
        /// </summary>
        [HttpGet]
        [Route("customers/{customerId}/orders")]
        public IActionResult GetOrdersForCustomer([FromHeader] string ApiKey, int customerId)
        {
            ResponseModel response = new();
            try
            {
               var listOrder = _customerOrderRead.GetWhere(x=> x.CustomerId==customerId,false).ToList();
                foreach (var order in listOrder)
                {
                    order.CustomerOrderDetail = _customerOrderDetailRead.GetWhere(x=>x.CustomerOrderId==order.Id,false).ToList();
                }
                response.Data = listOrder;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Hata: {ex.Message}";
            }

            return Ok(response);
        }



        /// <summary>
        /// Bir Customerın tüm bir Order bilgisini getirir. 
        /// ApiKey = B2CJWTSECRET
        /// </summary>
        [HttpGet]
        [Route("customers/{customerId}/orders/{id}")]
        public IActionResult GetSingleOrderForCustomer([FromHeader] string ApiKey, int customerId,int id)
        {
            ResponseModel response = new();
            try
            {
                var itemOrder = _customerOrderRead.GetSingleAsync(x => x.CustomerId == customerId && x.Id==id, false).Result;
               itemOrder.CustomerOrderDetail = _customerOrderDetailRead.GetWhere(x => x.CustomerOrderId == itemOrder.Id, false).ToList();
                
                response.Data = itemOrder;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Hata: {ex.Message}";
            }

            return Ok(response);
        }


        /// <summary>
        /// Bir Customer yeni bir Order kaydı açar. 
        /// ApiKey = B2CJWTSECRET
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("customers/{customerId}/orders/new")]
        public async Task<IActionResult> CreateCustomerOrder([FromHeader] string ApiKey, [FromBody] CustomerOrderModel customerOrderM)
        {
            ResponseModel response = new();
            try
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                string userName = Helper.JwtHelper.GetUserNameByToken(accessToken);
                if (!string.IsNullOrEmpty(userName) && _customerRead.GetSingleAsync(x => x.UserName == userName).Result.Id== customerOrderM.CustomerId)
                {
                    CustomerOrder order = new CustomerOrder()
                    {
                        CustomerId = customerOrderM.CustomerId,
                        DeliveryAddress = customerOrderM.DeliveryAddress,
                        OrderAmount = customerOrderM.CustomerOrderDetail.Sum(x=>x.Quantity*x.Price),
                        OrderDate = customerOrderM.OrderDate,
                        OrderCode =  Helper.GenerateRandomCodeHelper.GetCode(),
                        CreateDate = DateTime.Now
                    };
                    await _customerOrderWrite.AddAsync(order);
                    await _customerOrderWrite.SaveAsync();


                    foreach (var orderDetail in customerOrderM.CustomerOrderDetail)
                    {
                        orderDetail.CustomerOrderId = order.Id;
                        orderDetail.TotalAmount= orderDetail.Quantity*orderDetail.Price;

                        CustomerOrderDetail detail = _mapper.Map<CustomerOrderDetail>(orderDetail);
                        await _customerOrderDetailWrite.AddAsync(detail);
                        await _customerOrderDetailWrite.SaveAsync();
                    }

                    response.Data = order.Id;
                    response.IsSuccess = true;
                }

            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Hata: {ex.Message}";
            }

            return Ok(response);
        }


        /// <summary>
        /// Bir Customer kendine ait  bir Orderı edit eder(Ürün ekleme, çıkarma, adet güncelleme vb.). 
        /// ApiKey = B2CJWTSECRET
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("customers/{customerId}/orders/{id}/edit")]
        public async Task<IActionResult> UpdateCustomerOrder([FromHeader] string ApiKey, [FromBody] CustomerOrderUpdateModel customerOrderM)
        {
            ResponseModel response = new();
            try
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                string userName = Helper.JwtHelper.GetUserNameByToken(accessToken);
                if (!string.IsNullOrEmpty(userName))
                {
                    var customerId = _customerRead.GetSingleAsync(x => x.UserName == userName).Result.Id;
                    var order = _customerOrderRead.GetSingleAsync(x => x.CustomerId ==customerId && x.Id == customerOrderM.Id).Result;
                    order.UpdateDate = DateTime.Now;
                    order.DeliveryAddress = customerOrderM.DeliveryAddress;

                      _customerOrderWrite.Update(order);
                    await _customerOrderWrite.SaveAsync();

                    var orderDetailList = _customerOrderDetailRead.GetWhere(x=>x.CustomerOrderId==order.Id);
                    _customerOrderDetailWrite.RemoveRange(orderDetailList.ToList());

                    foreach (var orderDetail in customerOrderM.CustomerOrderDetail)
                    {
                        orderDetail.CustomerOrderId = order.Id;
                        orderDetail.TotalAmount = orderDetail.Quantity * orderDetail.Price;

                        CustomerOrderDetail detail = _mapper.Map<CustomerOrderDetail>(orderDetail);
                        await _customerOrderDetailWrite.AddAsync(detail);
                        await _customerOrderDetailWrite.SaveAsync();
                    }

                    response.Data = order.Id;
                    response.IsSuccess = true;
                }

            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Hata: {ex.Message}";
            }

            return Ok(response);
        }



        /// <summary>
        /// Bir Customer kendine ait  bir Orderı silebilir. 
        /// ApiKey = B2CJWTSECRET
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("customers/{customerId}/orders/{id}/remove")]
        public async Task<IActionResult> DeleteCustomerOrder([FromHeader] string ApiKey, int id)
        {
            ResponseModel response = new();
            try
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                string userName = Helper.JwtHelper.GetUserNameByToken(accessToken);
                if (!string.IsNullOrEmpty(userName))
                {
                    var customerId = _customerRead.GetSingleAsync(x => x.UserName == userName).Result.Id;
                    var listDetail = _customerOrderDetailRead.GetWhere(x=> x.CustomerOrderId==id);
                    _customerOrderDetailWrite.RemoveRange(listDetail.ToList());
                    _customerOrderDetailWrite.SaveAsync();

                    var order = _customerOrderRead.GetSingleAsync(x=>x.CustomerId==customerId && x.Id==id).Result;
                    _customerOrderWrite.Remove(order);
                    _customerOrderWrite.SaveAsync();

                    response.Data = null;
                    response.IsSuccess = true;
                }

            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Hata: {ex.Message}";
            }

            return Ok(response);
        }

    }
}
