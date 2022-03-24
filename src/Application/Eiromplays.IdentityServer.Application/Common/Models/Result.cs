namespace Eiromplays.IdentityServer.Application.Common.Models;

public class Result<T>
{
    protected Result(T? item, bool succeeded, IEnumerable<string> errors)
    {
        Item = item;
        Succeeded = succeeded;
        Errors = errors.ToList();
    }
        
    public T? Item { get; set; }        
        
    public bool Succeeded { get; set; }

    public List<string> Errors { get; set; }

    public static Result<T> Success(T? item)
    {
        return new Result<T>(item, true, new List<string>());
    }

    public static Result<T> Failure(T? item, IEnumerable<string> errors)
    {
        return new Result<T>(item, false, errors);
    }
    
    public static Result<T> Success()
    {
        return new Result<T>(default, true, new List<string>());
    }

    public static Result<T> Failure(List<string> errors)
    {
        return new Result<T>(default, false, errors);
    }
}

public class Result : Result<DateTime>
{
    private Result(bool succeeded, IEnumerable<string> errors) : base(default, succeeded, errors) { }
    
    public new static Result Success()
    {
        return new Result(true, new List<string>());
    }

    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}