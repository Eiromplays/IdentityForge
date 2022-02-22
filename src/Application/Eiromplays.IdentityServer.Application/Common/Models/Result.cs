namespace Eiromplays.IdentityServer.Application.Common.Models
{
    public class Result
    {
        private Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        private bool Succeeded { get; }

        private string[] Errors { get; }

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
