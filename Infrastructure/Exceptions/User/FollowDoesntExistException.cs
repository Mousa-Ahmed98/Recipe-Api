namespace Infrastructure.Exceptions.User
{
    public class FollowDoesntExistException : BadRequestException
    {
        public FollowDoesntExistException(string username)
            :base($"User with username {username} is not followed")
        {

        }
    }
}
