using ProductRelationEditor.Spark.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductRelationEditor.Spark.Services
{
    public class Service
    {
        public string ServiceUrl { get; private set; }

        public Service(string hostName)
        {
            ServiceUrl = $@"{hostName.TrimEnd('/')}/tempurl";
        }

        public async Task<IEnumerable<ItemModel>> FetchDataAsync()
        {
            await Task.Delay(1000);
            return new List<ItemModel>
            {
                new ItemModel("ItemA1", "ItemB1"),
                new ItemModel("ItemA2", "ItemB2"),
                new ItemModel("ItemA3", "ItemB3"),
                new ItemModel("ItemA4", "ItemB4"),
                new ItemModel("ItemA5", "ItemB5"),
            };
        }

        public async Task<bool> RegisterDataAsync()
        {
            await Task.Delay(1000);
            return true;
        }
    }
}
