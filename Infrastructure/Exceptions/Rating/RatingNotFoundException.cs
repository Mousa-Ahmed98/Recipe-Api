namespace Infrastructure.Exceptions.User
{
    public class RatingNotFoundException : NotFoundException
    {
        public RatingNotFoundException(int id)
            : base($"Rating with id {id} was not found.")
        {
        }
    }
}
