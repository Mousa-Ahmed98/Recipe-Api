namespace Infrastructure.CustomModels
{
    public record RecipeSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public bool InFavourites { get; set; }
    }
}
