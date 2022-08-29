using B2C.DataAccess.Models;

namespace B2C.WepApi.Models
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public object Data { get; set; }

    }
}
