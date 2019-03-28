namespace TestApp.Lambda.Common.Models
{
    public class FunctionResult
    {
        public int Code { get; }
        public string CodeName { get; }
        public object Result { get; }

        public FunctionResult(int code, string name)
            : this(code, name, string.Empty)
        {            
        }

        public FunctionResult(int code, string name, object result)
        {
            this.Code = code;
            this.CodeName = name;
            this.Result = result;
        }
    }
}
