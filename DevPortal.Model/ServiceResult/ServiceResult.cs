namespace DevPortal.Model
{
    /// <summary>
    /// ServiceResult sınıfı, factory metotlarının alt sınıflara tezahürünün engellenmesi amacıyla mühürlüdür.
    /// <br></br>
    /// İçerisinde tek bir Data özelliği taşıyacak yeni bir ServiceResult, <see cref="GenericServiceResult{T}"/> sınıfından türetilir.
    /// <br></br>
    /// Birden fazla Özellik bulunduracak bir ServiceResult alt sınıfı, <see cref="BaseServiceResult"/> sınıfından türetilir.
    /// </summary>
    public sealed class ServiceResult : BaseServiceResult
    {
        #region ctor

        ServiceResult(bool isSuccess, string message)
            : base(isSuccess, message)
        {
        }

        #endregion

        #region factory methods

        public static ServiceResult Success()
        {
            return new ServiceResult(true, null);
        }

        public static ServiceResult Success(string message)
        {
            return new ServiceResult(true, message);
        }

        public static ServiceResult Error()
        {
            return new ServiceResult(false, null);
        }

        public static ServiceResult Error(string message)
        {
            return new ServiceResult(false, message);
        }

        #endregion
    }
}