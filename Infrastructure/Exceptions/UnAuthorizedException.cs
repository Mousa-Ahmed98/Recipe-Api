using Application.Exceptions;
using System.Net;

namespace Infrastructure.Exceptions
{
    public class UnAuthorizedException : CustomException
    {
        public UnAuthorizedException()
            : base("Unauthorized access!")
        {
            StatusCode = HttpStatusCode.Unauthorized;
        }
    }
}
