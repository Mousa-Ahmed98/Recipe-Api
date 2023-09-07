namespace Infrastructure.CustomModels
{
    public record RecipeSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
    }
}
