namespace WSForm.Models
{
    public class SearchModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string BookType { get; set; }
        public double? PriceFrom { get; set; }
        public double? PriceTo { get; set; }
        public string Author { get; set; }
    }
}