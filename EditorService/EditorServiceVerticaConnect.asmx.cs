using EditorService.Common.Dto;
using Newtonsoft.Json; // Newtonsoft.Json の名前空間をインポート
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json; // System.Text.Json の名前空間をインポート
using System.Web.Services;
using System.Web.Script.Services; // ← 追加

namespace EditorService
{
    /// <summary>
    /// EditorService の概要の説明です
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService] // ← 追加
    public class EditorService : System.Web.Services.WebService
    {
        /// <summary>
        /// TECリストを出力します
        /// </summary>
        /// <returns>TECの一覧を DataTable で返します。</returns>
        [WebMethod]
        public DataTable TecKindList()
        {
            var creatorVertica = new CreatorVertica();

            // 検索実行
            var resultTask = creatorVertica.TecKindList();
            resultTask.Wait(); // 非同期タスクを同期的に待機
            var result = resultTask.Result;

            return ConvertToDataTable(result);
        }

        /// <summary>
        /// カテゴリリストを出力します
        /// </summary>
        /// <param name="searchCondition">検索条件（JSON形式）</param>
        /// <returns>検索結果を DataTable で返します。</returns>
        [WebMethod]
        public DataTable CategoryList(string searchCondition)
        {
            var creatorVertica = new CreatorVertica();

            var param = ConvertToSpcMasterParameter(searchCondition);

            // 検索実行
            var resultTask = creatorVertica.CategoryList(param);
            resultTask.Wait(); // 非同期タスクを同期的に待機
            var result = resultTask.Result;

            // IEnumerable<SpcMasterResult> を DataTable に変換
            return ConvertToDataTable(result);

        }

        /// <summary>
        /// 装置IDリストを出力します
        /// </summary>
        /// <param name="searchCondition">検索条件（JSON形式）</param>
        /// <returns>検索結果を DataTable で返します。</returns>
        [WebMethod]
        public DataTable EquipmentList(string searchCondition)
        {
            var creatorVertica = new CreatorVertica();

            var param = ConvertToSpcMasterParameter(searchCondition);

            // 検索実行
            var resultTask = creatorVertica.EquipmentList(param);
            resultTask.Wait(); // 非同期タスクを同期的に待機
            var result = resultTask.Result;

            // IEnumerable<SpcMasterResult> を DataTable に変換
            return ConvertToDataTable(result);

        }

        /// <summary>
        /// グループ名称リストを出力します
        /// </summary>
        /// <param name="searchCondition">検索条件（JSON形式）</param>
        /// <returns>検索結果を DataTable で返します。</returns>
        [WebMethod]
        public DataTable GroupNameList(string searchCondition)
        {
            var creatorVertica = new CreatorVertica();

            var param = ConvertToSpcMasterParameter(searchCondition);

            // 検索実行
            var resultTask = creatorVertica.GroupNameList(param);
            resultTask.Wait(); // 非同期タスクを同期的に待機
            var result = resultTask.Result;

            // IEnumerable<SpcMasterResult> を DataTable に変換
            return ConvertToDataTable(result);

        }

        /// <summary>
        /// SPCチャートの検索を行います。
        /// </summary>
        /// <param name="searchCondition">検索条件（JSON形式）</param>
        /// <returns>検索結果を DataTable で返します。</returns>
        [WebMethod]
        public DataTable SearchSpcChart(string searchCondition)
        {
            var creatorVertica = new CreatorVertica();

            var param = ConvertToSpcMasterParameter(searchCondition);

            // 検索実行
            var resultTask = creatorVertica.SpcChartSearch(param);
            resultTask.Wait(); // 非同期タスクを同期的に待機
            var result = resultTask.Result;

            // IEnumerable<SpcMasterResult> を DataTable に変換
            return ConvertToDataTable(result);

        }

        /// <summary>
        /// Istar Master Registry を登録します。
        /// </summary>
        /// <param name="json">JSON形式のデータ</param>
        /// <returns>登録結果を返します。</returns>
        [WebMethod]
        public async System.Threading.Tasks.Task<bool> IstarMasterRegistry(string json)
        {
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // もしjsonがダブルクォートで囲まれていたらアンラップ
            if (json.StartsWith("\"") && json.EndsWith("\""))
            {
                json = System.Text.Json.JsonSerializer.Deserialize<string>(json);
            }

            var items = System.Text.Json.JsonSerializer.Deserialize<List<ItemModel>>(json, options);

            var creator = new CreatorVertica();
            return await creator.RegisterIstarMaster(items);
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

            dataTable.TableName = "DataItemList";
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

            dataTable.TableName = "DataItemList";
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
