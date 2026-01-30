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

            foreach (var row in doc.Descendants("DataItemList"))
            {
                items.Add((string)row.Element("DataItem"));
            }
            return items;
        }

        public async Task<IEnumerable<string>> EditorServiceCategoryList(string searchConditionJson)
        {
            using var client = new HttpClient();
            var url = $"{ServiceUrl}/EditorServiceVerticaConnect.asmx/CategoryList";
            var content = new StringContent($"searchCondition={System.Web.HttpUtility.UrlEncode(searchConditionJson)}", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var xml = await response.Content.ReadAsStringAsync();

            //// XMLから必要なデータを抽出（例: DataTable形式のXMLをパース）
            var doc = XDocument.Parse(xml);
            var items = new List<string>();

            foreach (var row in doc.Descendants("DataItemList"))
            {
                items.Add((string)row.Element("DataItem"));
            }
            return items;
        }        

        public async Task<IEnumerable<string>> EditorServiceEquipmentList(string searchConditionJson)
        {
            using var client = new HttpClient();
            var url = $"{ServiceUrl}/EditorServiceVerticaConnect.asmx/EquipmentList";
            var content = new StringContent($"searchCondition={System.Web.HttpUtility.UrlEncode(searchConditionJson)}", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var xml = await response.Content.ReadAsStringAsync();

            //// XMLから必要なデータを抽出（例: DataTable形式のXMLをパース）
            var doc = XDocument.Parse(xml);
            var items = new List<string>();

            foreach (var row in doc.Descendants("DataItemList"))
            {
                items.Add((string)row.Element("DataItem"));
            }
            return items;
        }
        
        public async Task<IEnumerable<string>> EditorServiceGroupNameList(string searchConditionJson)
        {
            using var client = new HttpClient();
            var url = $"{ServiceUrl}/EditorServiceVerticaConnect.asmx/GroupNameList";
            var content = new StringContent($"searchCondition={System.Web.HttpUtility.UrlEncode(searchConditionJson)}", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var xml = await response.Content.ReadAsStringAsync();

            //// XMLから必要なデータを抽出（例: DataTable形式のXMLをパース）
            var doc = XDocument.Parse(xml);
            var items = new List<string>();

            foreach (var row in doc.Descendants("DataItemList"))
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

            // XMLから必要なデータを抽出
            var doc = XDocument.Parse(xml);
            var items = new List<ItemModel>();

            foreach (var row in doc.Descendants("DataItemList"))
            {
                items.Add(new ItemModel(
                    (string)row.Element("TEC_KIND"),
                    (string)row.Element("CCATEGORY"),
                    (string)row.Element("EQP_ID"),
                    (string)row.Element("GNAME"),
                    (string)row.Element("PRODUCT"),
                    (string)row.Element("P_RECIPE"),
                    (string)row.Element("M_PROCESS_NAME"),
                    (string)row.Element("M_WORK_NAME"),
                    (string)row.Element("M_FLD_ID"),
                    (string)row.Element("M_WORK_CODE"),
                    (string)row.Element("TIMESERIES_SEQ_NO"),
                    (string)row.Element("DCITEM_NM"),
                    (string)row.Element("DCITEM_UNIT"),
                    (string)row.Element("CTITLE"),
                    (bool)row.Element("SENDFLG"),
                    (string)row.Element("LARGE_GROUP"),
                    (string)row.Element("SMALL_GROUP"),
                    (string)row.Element("DISPLATY_NAME"),
                    (string)row.Element("CHAMBER_NAME"),
                    (bool)row.Element("POINTFLG")
                ));
            }
            return items;
        }
    }
}
