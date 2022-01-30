using Chklstr.Core.Utils;

namespace Chklstr.Core.Services;

public class ParseResult<T>
{
    public T? Result { get; init; }
    
    public List<String> Errors { get; init; }= new ();

    public bool IsSuccess()
    {
        return Errors.IsEmpty() && Result != null;
    }

    protected ParseResult(T? result)
    {
        this.Result = result;
    }

    public static ParseResult<T> Failed(string error)
    {
        return Failed(new List<string>() {error});
    }

    public static ParseResult<T> Failed(List<String> errors)
    {
        var result = new ParseResult<T>(default);
        result.Errors.AddAll(errors);
        return result;
    }

    public static ParseResult<T> Success(T result)
    {
        return new ParseResult<T>(result);
    }
}