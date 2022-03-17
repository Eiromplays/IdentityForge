namespace Eiromplays.IdentityServer.Application.Common.Models
{
    public class Result<T> : Result
    {
        public Result(T? item, bool succeeded, IEnumerable<string> errors) : base(succeeded, errors)
        {
            Item = item;
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }
        
        public T? Item { get; }
    }
    
    public class Result
    {
        public Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }
        
        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }

        public static Result Success()
        {
            return new Result(true, Array.Empty<string>());
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }
    }
}
