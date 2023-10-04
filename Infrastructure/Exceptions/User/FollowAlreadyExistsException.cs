namespace Infrastructure.Exceptions.User
{
    public class FollowAlreadyExistsException : BadRequestException
    {
        public FollowAlreadyExistsException(string username)
            :base($"User with username {username} is already followed")
        {

        }
    }
}
