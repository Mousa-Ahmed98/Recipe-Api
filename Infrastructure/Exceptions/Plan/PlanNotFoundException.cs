namespace Infrastructure.Exceptions.Plan
{
    public class PlanNotFoundException : NotFoundException
    {
        public PlanNotFoundException(int id)
            : base($"Plan with ${id} was not found.")
        {

        }
    }
}
