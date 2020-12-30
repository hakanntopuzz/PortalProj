namespace DevPortal.Web.Library.Model
{
    public class BaseResult
    {
        public string Message { get; set; }

        public bool IsSuccess { get; set; }

        public static BaseResult Create(bool isSuccess, string message = null)
        {
            return new BaseResult
            {
                IsSuccess = isSuccess,
                Message = message
            };
        }
    }
}