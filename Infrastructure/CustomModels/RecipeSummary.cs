namespace Infrastructure.CustomModels
{
    public record RecipeSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool InFavourites { get; set; }
    }
}
