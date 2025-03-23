namespace BoardService.Core.Common
{
    public class Result<TObject>
    {
        public TObject? Value { get; }
        public List<string> Errors { get;} = new List<string>();
        public bool IsSuccess => Errors.Count == 0;
        public bool IsError => !IsSuccess;
        public string StringErrors => string.Join(". ", Errors);

        public Result(TObject? value, List<string> errors) { 
            Value = value;
            Errors = errors;
        }
    }


    public class Result
    {
        public List<string> Errors { get; } = new List<string>();
        public bool IsSuccess => Errors.Count == 0;
        public bool IsError => !IsSuccess;

        public Result(List<string> errors)
        {
            Errors = errors;
        }

        public Result(){}

    }

}
