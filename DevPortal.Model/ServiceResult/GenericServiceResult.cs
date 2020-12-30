namespace DevPortal.Model
{
    public abstract class GenericServiceResult<T> : BaseServiceResult
    {
        //TODO: Value için yeni bir isim tartışılabilir.
        //Yeni isim önerisi: Item
        //Yeni isim önerisi: Data

        public T Value { get; set; }

        protected GenericServiceResult(bool isSuccess, string message, T value)
            : base(isSuccess, message)
        {
            Value = value;
        }
    }
}