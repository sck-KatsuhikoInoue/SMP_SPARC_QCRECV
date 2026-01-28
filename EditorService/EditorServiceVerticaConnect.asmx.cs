using Newtonsoft.Json; // Newtonsoft.Json の名前空間をインポート
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;

namespace EditorService
{
    /// <summary>
    /// EditorService の概要の説明です
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class EditorService : System.Web.Services.WebService
    {
        [WebMethod]
        public DataTable TecKindList()
        {
            var creatorVertica = new CreatorVertica();

            // 検索実行
            var resultTask = creatorVertica.TecKindList();
            resultTask.Wait(); // 非同期タスクを同期的に待機
            var result = resultTask.Result;

            return ConvertToDataTable(result);
            //// IEnumerable<SpcMasterResult> を DataTable に変換
            //var dataTable = ConvertToDataTable(result);

            //// DataTableをJSONに変換して返す
            //string json;
            //using (var sw = new System.IO.StringWriter())
            //{
            //    dataTable.WriteXml(sw, XmlWriteMode.WriteSchema, false);
            //    json = sw.ToString();
            //}
            //return json;
        }

        [WebMethod]
        public string SearchSpcChart(string searchCondition)
        {
            try
            {
                var creatorVertica = new CreatorVertica();

                var param = ConvertToSpcMasterParameter(searchCondition);

                // 検索実行
                var resultTask = creatorVertica.SpcChartSearch(param);
                resultTask.Wait(); // 非同期タスクを同期的に待機
                var result = resultTask.Result;

                // IEnumerable<SpcMasterResult> を DataTable に変換
                var dataTable = ConvertToDataTable(result);

                // DataTableをJSONに変換して返す
                string json;
                using (var sw = new System.IO.StringWriter())
                {
                    dataTable.WriteXml(sw, XmlWriteMode.WriteSchema, false);
                    json = sw.ToString();
                }
                return json;
            }
            catch (Exception ex)
            {
                return $"{{\"error\":\"{ex.Message}\"}}";
            }
        }

        // searchCondition を SpcMasterParameter に変換するヘルパーメソッド
        private SpcMasterParameter ConvertToSpcMasterParameter(string searchCondition)
        {
            // searchCondition を解析して SpcMasterParameter を生成するロジックを実装
            // ここでは仮の例として、searchCondition を JSON としてデシリアライズする処理を記述
            return JsonConvert.DeserializeObject<SpcMasterParameter>(searchCondition);
        }

        // IEnumerable<string> を DataTable に変換するヘルパーメソッド
        private DataTable ConvertToDataTable(IEnumerable<string> data)
        {
            // DataTable の列を定義
            var dataTable = new DataTable();

            dataTable.TableName = "TecKindList";
            dataTable.Columns.Add("DataItem", typeof(string));


            // DataTable にデータを追加
            foreach (var item in data)
            {
                var row = dataTable.NewRow();
                row["DataItem"] = item;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        
        // IEnumerable<SpcMasterResult> を DataTable に変換するヘルパーメソッド
        private DataTable ConvertToDataTable(IEnumerable<SpcMasterResult> data)
        {
            var dataTable = new DataTable();

            // DataTable の列を定義
            foreach (var prop in typeof(SpcMasterResult).GetProperties())
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            // DataTable にデータを追加
            foreach (var item in data)
            {
                var row = dataTable.NewRow();
                foreach (var prop in typeof(SpcMasterResult).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}
