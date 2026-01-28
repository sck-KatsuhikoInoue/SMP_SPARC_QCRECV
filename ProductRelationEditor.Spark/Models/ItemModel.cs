namespace ProductRelationEditor.Spark.Models
{
    public class ItemModel
    {
        public string Item1 { get; set; }
        public string Item2 { get; set; }

        public ItemModel(string item1, string item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
}
