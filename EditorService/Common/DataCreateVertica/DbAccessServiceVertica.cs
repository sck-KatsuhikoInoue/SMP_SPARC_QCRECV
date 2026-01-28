using System.Data;
using System.Text;
using System.Threading.Tasks; // この行を追加
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization; // この行を追加



namespace EditorService
{
    internal class DbAccessServiceVertica
    {
        private IVerticaDb _db;
        private JsonSerializerOptions _jsonSerializerOptions;

        internal DbAccessServiceVertica(IVerticaDb db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
            };

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        internal string GetConnectionString() => _db?.ConnectionString ?? string.Empty;

        internal async Task<IEnumerable<SpcMasterResult>> SpcChartSearch(SpcMasterParameter param)
        {
            try
            {
                var sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine("SELECT *");
                sqlBuilder.AppendLine("FROM stg_tseries.SPARC_SPC_CHART_MAST");
                sqlBuilder.AppendLine("WHERE 1=1");

                var parameters = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(param.TEC_KIND))
                {
                    sqlBuilder.AppendLine("AND TEC_KIND = :TEC_KIND");
                    parameters.Add("TEC_KIND", param.TEC_KIND);
                }
                if (!string.IsNullOrEmpty(param.CCATEGORY))
                {
                    sqlBuilder.AppendLine("AND CCATEGORY = :CCATEGORY");
                    parameters.Add("CCATEGORY", param.CCATEGORY);
                }
                if (!string.IsNullOrEmpty(param.EQP_ID))
                {
                    sqlBuilder.AppendLine("AND EQP_ID = :EQP_ID");
                    parameters.Add("EQP_ID", param.EQP_ID);
                }
                if (!string.IsNullOrEmpty(param.GNAME))
                {
                    sqlBuilder.AppendLine("AND GNAME = :GNAME");
                    parameters.Add("GNAME", param.GNAME);
                }

                var sql = sqlBuilder.ToString();

                var results = new List<SpcMasterResult>();
                using (var reader = await _db.ExecuteAsync(sql, parameters))
                {
                    if (reader.HasRows)
                    {
                        var idxTEC_KIND = reader.GetOrdinal("TEC_KIND");
                        var idxCCATEGORY = reader.GetOrdinal("CCATEGORY");
                        var idxEQP_ID = reader.GetOrdinal("EQP_ID");
                        var idxGNAME = reader.GetOrdinal("GNAME");
                        var idxPRODUCT = reader.GetOrdinal("PRODUCT");
                        var idxP_RECIPE = reader.GetOrdinal("P_RECIPE");
                        var idxTIMESERIES_SEQ_NO = reader.GetOrdinal("TIMESERIES_SEQ_NO");
                        var idxDCITEM_NM = reader.GetOrdinal("DCITEM_NM");
                        var idxDCITEM_UNIT = reader.GetOrdinal("DCITEM_UNIT");
                        var idxCTITLE = reader.GetOrdinal("CTITLE");

                        while (reader.Read())
                        {
                            results.Add(new SpcMasterResult()
                            {
                                TEC_KIND = reader.GetStringSafe(idxTEC_KIND),
                                CCATEGORY = reader.GetStringSafe(idxCCATEGORY),
                                EQP_ID = reader.GetStringSafe(idxEQP_ID),
                                GNAME = reader.GetStringSafe(idxGNAME),
                                PRODUCT = reader.GetStringSafe(idxPRODUCT),
                                P_RECIPE = reader.GetStringSafe(idxP_RECIPE),
                                TIMESERIES_SEQ_NO = reader.GetStringSafe(idxTIMESERIES_SEQ_NO),
                                DCITEM_NM = reader.GetStringSafe(idxDCITEM_NM),
                                DCITEM_UNIT = reader.GetStringSafe(idxDCITEM_UNIT),
                                CTITLE = reader.GetStringSafe(idxCTITLE),
                            });
                        }
                    }
                }

                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    /// <summary>
    /// IDataReaderの拡張メソッド
    /// </summary>
    internal static class DataReaderExtensions
    {
        /// <summary>SELECTしたカラムのデータを取得（string）</summary>
        /// <param name="idx">インデックス</param>
        /// <param name="defaultValue">取得失敗時のデフォルト値（規定値：string.Empty）</param>
        /// <returns></returns>
        internal static string GetStringSafe(this IDataReader reader, int idx, string defaultValue = "")
        {
            return reader.IsDBNull(idx) ? defaultValue : reader.GetString(idx);
        }

        /// <summary>SELECTしたカラムのデータを取得（string）</summary>
        /// <param name="columnName">カラム名</param>
        /// <param name="defaultValue">取得失敗時のデフォルト値（規定値：string.Empty）</param>
        internal static string GetStringSafe(this IDataReader reader, string columnName, string defaultValue = "")
        {
            var idx = reader.GetOrdinal(columnName);
            return reader.GetStringSafe(idx, defaultValue);
        }

        /// <summary>SELECTしたカラムのデータを取得（int）</summary>
        /// <param name="idx">インデックス</param>
        /// <param name="defaultValue">取得失敗時のデフォルト値（規定値：0）</param>
        internal static int GetIntSafe(this IDataReader reader, int idx, int defaultValue = default)
        {
            return reader.IsDBNull(idx) ? defaultValue : reader.GetInt32(idx);
        }

        /// <summary>SELECTしたカラムのデータを取得（int）</summary>
        /// <param name="columnName">カラム名</param>
        /// <param name="defaultValue">取得失敗時のデフォルト値（規定値：0）</param>
        internal static int GetIntSafe(this IDataReader reader, string columnName, int defaultValue = default)
        {
            var idx = reader.GetOrdinal(columnName);
            return reader.GetIntSafe(idx, defaultValue);
        }

        /// <summary>SELECTしたカラムのデータを取得（double）</summary>
        /// <param name="idx">インデックス</param>
        /// <param name="defaultValue">取得失敗時のデフォルト値（規定値：0D）</param>
        internal static double GetDoubleSafe(this IDataReader reader, int idx, double defaultValue = default)
        {
            return reader.IsDBNull(idx) ? defaultValue : reader.GetDouble(idx);
        }

        /// <summary>SELECTしたカラムのデータを取得（double）</summary>
        /// <param name="columnName">カラム名</param>
        /// <param name="defaultValue">取得失敗時のデフォルト値（規定値：0D）</param>
        internal static double GetDoubleSafe(this IDataReader reader, string columnName, double defaultValue = default)
        {
            var idx = reader.GetOrdinal(columnName);
            return reader.GetDoubleSafe(idx, defaultValue);
        }

        /// <summary>SELECTしたカラムのデータを取得（DateTime）</summary>
        /// <param name="idx">インデックス</param>
        /// <param name="defaultValue">取得失敗時のデフォルト値（規定値：0001/01/01 0:00:00）</param>
        internal static DateTime GetDateTimeSafe(this IDataReader reader, int idx, DateTime defaultValue = default)
        {
            return reader.IsDBNull(idx) ? defaultValue : reader.GetDateTime(idx);
        }

        /// <summary>SELECTしたカラムのデータを取得（DateTime）</summary>
        /// <param name="columnName">カラム名</param>
        /// <param name="defaultValue">取得失敗時のデフォルト値（規定値：0001/01/01 0:00:00）</param>
        internal static DateTime GetDateTimeSafe(this IDataReader reader, string columnName, DateTime defaultValue = default)
        {
            var idx = reader.GetOrdinal(columnName);
            return reader.GetDateTimeSafe(idx, defaultValue);
        }
    }

}
