namespace SoilParamsAPI.Models
{
    public class Result<T>
    {
        public bool Success { get; set; } = true;
        public T    Value   { get; set;  }
    }
}