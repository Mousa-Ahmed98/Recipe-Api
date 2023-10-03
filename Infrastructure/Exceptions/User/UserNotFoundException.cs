namespace Infrastructure.Exceptions.User
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string username)
            : base($"User with username {username} was not found.")
        {

        }
    }
}
