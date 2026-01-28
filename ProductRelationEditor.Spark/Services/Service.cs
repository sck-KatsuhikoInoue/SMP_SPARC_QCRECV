using ProductRelationEditor.Spark.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProductRelationEditor.Spark.Services
{
    public class Service
    {
        public string ServiceUrl { get; private set; }

        public Service(string hostName)
        {
            //ServiceUrl = $@"{hostName.TrimEnd('/')}/tempurl";
            ServiceUrl=$@"https://localhost:44347/";
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

        public async Task<IEnumerable<string>> EditorServiceTecKindList()
        {
            using var client = new HttpClient();
            var url = $"{ServiceUrl}/EditorServiceVerticaConnect.asmx/TecKindList";

            var response = await client.PostAsync(url, content: null);
            response.EnsureSuccessStatusCode();

            var xml = await response.Content.ReadAsStringAsync();

            //// XMLから必要なデータを抽出（例: DataTable形式のXMLをパース）
            var doc = XDocument.Parse(xml);
            var items = new List<string>();

            foreach (var row in doc.Descendants("TecKindList"))
            {
                items.Add((string)row.Element("DataItem"));
            }
            return items;
        }


        public async Task<IEnumerable<ItemModel>> EditorServiceSearchSpcChart(string searchConditionJson)
        {
            using var client = new HttpClient();
            var url = $"{ServiceUrl}/EditorServiceVerticaConnect.asmx/SearchSpcChart";
            var content = new StringContent($"searchCondition={System.Web.HttpUtility.UrlEncode(searchConditionJson)}", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var xml = await response.Content.ReadAsStringAsync();

            // XMLから必要なデータを抽出（例: DataTable形式のXMLをパース）
            var doc = XDocument.Parse(xml);
            var items = new List<ItemModel>();

            foreach (var row in doc.Descendants("row"))
            {
                var item1 = (string)row.Element("TEC_KIND");
                var item2 = (string)row.Element("GNAME");
                items.Add(new ItemModel(item1, item2));
            }
            return items;
        }
    }
}
