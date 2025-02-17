namespace AuthenticationService.Core.Common
{
    public class Result<Object>
    {
        public Object? Value { get; }
        public List<string> Errors { get;} = new List<string>();
        public bool IsSuccess => Errors.Count == 0;
        public bool IsError => !IsSuccess;

        public Result(Object? value, List<string> errors) { 
            Value = value;
            Errors = errors;
        }
    }
}
