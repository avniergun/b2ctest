using AutoMapper;
using B2C.Core.Repositories;
using B2C.DataAccess.Models;
using B2C.WepApi.Helper;
using B2C.WepApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace B2C.WepApi.Controllers
{
    /// <summary>
    /// Müşteri authenticate (JWT Token), Müşteri Listesi, Bilgisi, Müşteri Ekleme ve Müşteri Adresi Güncelleme işlemlerini içerir. 
    ///  Sisteme erişim  API_KEY ile  kısıtlanmıştır. API_KEY = B2CJWTSECRET
    /// </summary>
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private JwtSettings _jwtSettings;
        private readonly ILogger<Customer> _logger;
        ICustomerReadRepository _customerRead;
        ICustomerWriteRepository _customerWrite;
        IMapper _mapper;
        public CustomerController(ILogger<Customer> logger, IMapper mapper, ICustomerReadRepository customerRead, ICustomerWriteRepository customerWrite, IOptions<JwtSettings> jwtSettings)
        {
            _logger = logger;
            _customerRead = customerRead;
            _customerWrite = customerWrite;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }


        /// <summary>
		/// Customer CRUD işlemleri yapması için JWt Bearer Token için  Authenticate olması gerekmektedir. 
        /// Sisteme erişim  API_KEY ile  kısıtlanmıştır. ApiKey = B2CJWTSECRET
		/// </summary>
        [HttpPost]
        [Route("customers/auth")]
        public async Task<IActionResult> AuthenticateCustomer([FromHeader] string ApiKey, [FromBody] LoginModel loginModel)
        {
            var customer = await _customerRead.GetSingleAsync(x => x.UserName == loginModel.UserName && x.Password == loginModel.Password, false);
            if (customer == null)
                return BadRequest(new { message = "Kullanıcı adı veya şifre yanlış" });

            var claims = new List<Claim>();
            claims.Add(new Claim("username", customer.UserName));
            claims.Add(new Claim("displayname", customer.Name));
            var token = JwtHelper.GetJwtToken(customer.UserName, _jwtSettings, new TimeSpan(0, 15, 0), claims.ToArray());
            return Ok(token);
        }


        /// <summary>
        /// Tüm Customer Listesini getirir. 
        /// ApiKey = B2CJWTSECRET
        /// </summary>
        [HttpGet]
        [Route("customers")]
        public IActionResult GetAllCutomer([FromHeader] string ApiKey)
        {
            ResponseModel response = new();
            try
            {
                response.Data = _customerRead.GetAll(false);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Hata: {ex.Message}";
            }

            return Ok(response);
        }


        /// <summary>
        /// Id'si verilen Customer bilgisini getirir. 
        /// ApiKey = B2CJWTSECRET
        /// </summary>
        [HttpGet]
        [Route("customers/{id}")]
        public async Task<IActionResult> GetSingleCutomer([FromHeader] string ApiKey, int id)
        {
            ResponseModel response = new();
            try
            {
                response.Data = await _customerRead.GetByIdAsync(id, false);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Hata: {ex.Message}";
            }

            return Ok(response);
        }


        /// <summary>
        /// Yeni Customer oluşturur. 
        /// ApiKey = B2CJWTSECRET
        /// </summary>
        [HttpPost]
        [Route("customers/new")]
        public async Task<IActionResult> CreateCustomer([FromHeader] string ApiKey, [FromBody] CustomerModel customerM)
        {
            ResponseModel response = new();
            try
            {

                Customer customer = _mapper.Map<Customer>(customerM);
                await _customerWrite.AddAsync(customer);
                response.Data = await _customerWrite.SaveAsync();
                response.IsSuccess = true;


            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Hata: {ex.Message}";
            }

            return Ok(response);
        }


        /// <summary>
        /// Authenticate olmuş bir Customer adres bilgisini günceller. 
        /// ApiKey = B2CJWTSECRET
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("customers/adress")]
        public async Task<IActionResult> UpdateCustomer([FromHeader] string ApiKey, [FromBody] CustomerUpdateModel customerUpdate)
        {
            ResponseModel response = new();
            try
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                string userName = Helper.JwtHelper.GetUserNameByToken(accessToken);
                if (!string.IsNullOrEmpty(userName))
                {
                    var customer = await _customerRead.GetSingleAsync(x => x.UserName == userName);
                    if (customer != null)
                    {
                        customer.Address = customerUpdate.Address;
                        _customerWrite.Update(customer);
                        response.Data = await _customerWrite.SaveAsync();
                        response.IsSuccess = true;
                    }
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
