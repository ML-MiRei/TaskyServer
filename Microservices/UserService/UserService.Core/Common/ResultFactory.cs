namespace UserService.Core.Common
{
    public class ResultFactory<T>
    {

        private List<string> _errors { get;} = new List<string>();
        private T? _value = default(T);
        

        public void AddError(string error)
        {
            _errors.Add(error);
        }

        public void AddError(string[] errors)
        {
            _errors.AddRange(errors);
        }

        public void SetResult(T result)
        {
            if (_errors.Count > 0)
                _value = default(T);
            else
                _value = result;
        }

        public Result<T> Create()
        {
            return new Result<T>(_value, _errors);
        }

    }
}
