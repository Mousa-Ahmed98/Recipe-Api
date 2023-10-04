namespace Infrastructure.Exceptions.Plan
{
    public class InvalidDateFormatException : BadRequestException
    {
        public InvalidDateFormatException(string date)
            : base($"Invalid date format. date: {date}")
        {

        }
    }
}
