using Microsoft.AspNetCore.Http;
using System.Net;

namespace Infrastructure.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message)
        : base(message)
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}
