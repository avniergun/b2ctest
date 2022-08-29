using AutoMapper;
using B2C.Core.Repositories;
using B2C.DataAccess.Models;
using B2C.WepApi.Helper;
using B2C.WepApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace B2C.WepApi.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        
        private readonly ILogger<Product> _logger;
        IProductReadRepository _productRead;
        IProductWriteRepository _productWrite;
        IMapper _mapper;
        public ProductController(ILogger<Product> logger, IMapper mapper, IProductReadRepository productRead, IProductWriteRepository productWrite)
        {
            _logger = logger;
            _productRead = productRead;
            _productWrite = productWrite;
            _mapper = mapper;
        }


        /// <summary>
        /// Tüm Product listesini getirir. 
        /// ApiKey = B2CJWTSECRET
        /// </summary>
        [HttpGet]
        [Route("products")]
        public IActionResult GetAllProduct([FromHeader] string ApiKey)
        {
            ResponseModel response = new();
            try
            {
                response.Data = _productRead.GetAll(false);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Hata: {ex.Message}";
            }

            return Ok(response);
        }


        /// <summary>
        /// Tek bir Product bilgisi getirir. 
        /// ApiKey = B2CJWTSECRET
        /// </summary>
        [HttpGet]
        [Route("products/{id}")]
        public async Task<IActionResult> GetSingleProduct([FromHeader] string ApiKey, int id)
        {
            ResponseModel response = new();
            try
            {
                response.Data = await _productRead.GetByIdAsync(id, false);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Hata: {ex.Message}";
            }

            return Ok(response);
        }


        /// <summary>
        /// Yeni Product ekler. 
        /// ApiKey = B2CJWTSECRET
        /// </summary>
        [HttpPost]
        [Route("products/new")]
        public async Task<IActionResult> CreateProduct([FromHeader] string ApiKey, [FromBody] ProductModel productM)
        {
            ResponseModel response = new();
            try
            {
                Product product = _mapper.Map<Product>(productM);
                await _productWrite.AddAsync(product);
                response.Data = await _productWrite.SaveAsync();
                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Hata: {ex.Message}";
            }

            return Ok(response);
        }


        /// <summary>
        /// Mecuttaki Product bilgisi güncellenir. 
        /// ApiKey = B2CJWTSECRET
        /// </summary>
        [HttpPost]
        [Route("products/edit")]
        public async Task<IActionResult> UpdateCustomer([FromHeader] string ApiKey, [FromBody] ProductModel productM)
        {
            ResponseModel response = new();
            try
            {
                Product product = _mapper.Map<Product>(productM);
                if (_productWrite.Update(product)) { 
                response.Data = await _productWrite.SaveAsync();
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
