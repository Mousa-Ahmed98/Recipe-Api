using Microsoft.AspNetCore.Http;
using System.Net;

namespace Application.Exceptions
{
    public abstract class NotFoundException : CustomException
    {
        protected NotFoundException(string message)
        : base(message)
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}
