namespace R1API.Models
{
    public class GeneralResponse
    {
        public dynamic Data { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
    }
}
