namespace DevPortal.Model
{
    public class BuildScriptServiceResult : GenericServiceResult<string>
    {
        #region ctor

        BuildScriptServiceResult(bool isSuccess, string message, string value)
            : base(isSuccess, message, value)
        {
        }

        #endregion

        #region factory methods

        public static BuildScriptServiceResult Success(string value)
        {
            return new BuildScriptServiceResult(true, null, value);
        }

        public static BuildScriptServiceResult Error(string message)
        {
            return new BuildScriptServiceResult(false, message, null);
        }

        #endregion
    }
}