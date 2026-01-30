using System.Data;
using System.Text;
using System.Threading.Tasks; // この行を追加
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization; // この行を追加
using EditorService.Common.Dto; // この行を追加



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

        internal async Task<IEnumerable<string>> TecKindList()
        {
            try
            {
                var sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine("SELECT distinct TEC_KIND FROM stg_tseries.SPARC_SPC_CHART_MAST ORDER BY TEC_KIND");

                var sql = sqlBuilder.ToString();

                var results = new List<string>();
                using (var reader = await _db.ExecuteAsync(sql))
                {
                    if (reader.HasRows)
                    {
                        var idxTEC_KIND = reader.GetOrdinal("TEC_KIND");
                        while (reader.Read())
                        {
                            results.Add(reader.GetStringSafe(idxTEC_KIND));
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
        

        internal async Task<IEnumerable<string>> CategoryList(SpcMasterParameter param)
        {
            try
            {
                var sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine("SELECT distinct CCATEGORY FROM stg_tseries.SPARC_SPC_CHART_MAST");
                sqlBuilder.AppendLine("WHERE 1=1");

                var parameters = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(param.TEC_KIND))
                {
                    sqlBuilder.AppendLine("AND TEC_KIND = :TEC_KIND");
                    parameters.Add("TEC_KIND", param.TEC_KIND);
                }

                // ORDER BY句を追加
                sqlBuilder.AppendLine("ORDER BY CCATEGORY");

                var sql = sqlBuilder.ToString();

                var results = new List<string>();
                using (var reader = await _db.ExecuteAsync(sql, parameters))
                {
                    if (reader.HasRows)
                    {
                        var idxCCATEGORY = reader.GetOrdinal("CCATEGORY");
                        while (reader.Read())
                        {
                            results.Add(reader.GetStringSafe(idxCCATEGORY));
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

        internal async Task<IEnumerable<string>> EquipmentList(SpcMasterParameter param)
        {
            try
            {
                var sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine("SELECT distinct EQP_ID FROM stg_tseries.SPARC_SPC_CHART_MAST");
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

                // ORDER BY句を追加
                sqlBuilder.AppendLine("ORDER BY EQP_ID");

                var sql = sqlBuilder.ToString();

                var results = new List<string>();
                using (var reader = await _db.ExecuteAsync(sql, parameters))
                {
                    if (reader.HasRows)
                    {
                        var idxEQP_ID = reader.GetOrdinal("EQP_ID");
                        while (reader.Read())
                        {
                            results.Add(reader.GetStringSafe(idxEQP_ID));
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


        internal async Task<IEnumerable<string>>GroupNameList(SpcMasterParameter param)
        {
            try
            {
                var sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine("SELECT distinct GNAME FROM stg_tseries.SPARC_SPC_CHART_MAST");
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

                // ORDER BY句を追加
                sqlBuilder.AppendLine("ORDER BY GNAME");

                var sql = sqlBuilder.ToString();

                var results = new List<string>();
                using (var reader = await _db.ExecuteAsync(sql, parameters))
                {
                    if (reader.HasRows)
                    {
                        var idxGNAME = reader.GetOrdinal("GNAME");
                        while (reader.Read())
                        {
                            results.Add(reader.GetStringSafe(idxGNAME));
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

        internal async Task<IEnumerable<SpcMasterResult>> SpcChartSearch(SpcMasterParameter param)
        {
            try
            {
                var sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine("SELECT *");
                sqlBuilder.AppendLine(",1 as SENDFLG,null as LARGE_GROUP,null as SMALL_GROUP,null as DISPLAY_NAME,null as CHAMBER_NAME,0 as POINTFLG");
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

                // ORDER BY句を追加
                sqlBuilder.AppendLine("ORDER BY TEC_KIND,CCATEGORY,EQP_ID,GNAME,DCITEM_NM");

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
                        var idxM_PROCESS_NAME = reader.GetOrdinal("M_PROCESS_NAME");
                        var idxM_WORK_NAME = reader.GetOrdinal("M_WORK_NAME");
                        var idxM_FLD_ID = reader.GetOrdinal("M_FLD_ID");
                        var idxM_WORK_CODE = reader.GetOrdinal("M_WORK_CODE");
                        var idxTIMESERIES_SEQ_NO = reader.GetOrdinal("TIMESERIES_SEQ_NO");
                        var idxDCITEM_NM = reader.GetOrdinal("DCITEM_NM");
                        var idxDCITEM_UNIT = reader.GetOrdinal("DCITEM_UNIT");
                        var idxCTITLE = reader.GetOrdinal("CTITLE");
                        var idxSENDFLG = reader.GetOrdinal("SENDFLG");
                        var idxLARGE_GROUP = reader.GetOrdinal("LARGE_GROUP");
                        var idxSMALL_GROUP = reader.GetOrdinal("SMALL_GROUP");
                        var idxDISPLAY_NAME = reader.GetOrdinal("DISPLAY_NAME");
                        var idxCHAMBER_NAME = reader.GetOrdinal("CHAMBER_NAME");
                        var idxPOINTFLG = reader.GetOrdinal("POINTFLG");

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
                                M_PROCESS_NAME = reader.GetStringSafe(idxM_PROCESS_NAME),
                                M_WORK_NAME = reader.GetStringSafe(idxM_WORK_NAME),
                                M_FLD_ID = reader.GetStringSafe(idxM_FLD_ID),
                                M_WORK_CODE = reader.GetStringSafe(idxM_WORK_CODE),
                                TIMESERIES_SEQ_NO = reader.GetStringSafe(idxTIMESERIES_SEQ_NO),
                                DCITEM_NM = reader.GetStringSafe(idxDCITEM_NM),
                                DCITEM_UNIT = reader.GetStringSafe(idxDCITEM_UNIT),
                                CTITLE = reader.GetStringSafe(idxCTITLE),
                                SENDFLG = reader.GetIntSafe(idxSENDFLG),
                                LARGE_GROUP = reader.GetStringSafe(idxLARGE_GROUP),
                                SMALL_GROUP = reader.GetStringSafe(idxSMALL_GROUP),
                                DISPLAY_NAME = reader.GetStringSafe(idxDISPLAY_NAME),
                                CHAMBER_NAME = reader.GetStringSafe(idxCHAMBER_NAME),
                                POINTFLG = reader.GetIntSafe(idxPOINTFLG),
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

        internal async Task<bool> RegisterIstarMaster(IEnumerable<ItemModel> items)
        {
            // 例: トランザクションで一括登録
            //try
            //{
            //    foreach (var item in items)
            //    {
            //        // 必要なSQLとパラメータを作成
            //        var sql = @"INSERT INTO ISTAR_MASTER (LARGE_GROUP, SMALL_GROUP, DISPLAY_NAME, ...) VALUES (@LARGE_GROUP, @SMALL_GROUP, @DISPLAY_NAME, ...)";
            //        var parameters = new Dictionary<string, object>
            //        {
            //            { "@LARGE_GROUP", item.LARGE_GROUP },
            //            { "@SMALL_GROUP", item.SMALL_GROUP },
            //            { "@DISPLAY_NAME", item.DISPLAY_NAME },
            //            // ...他のカラムも追加
            //        };
            //        await _db.ExecuteAsync(sql, parameters);
            //    }
            //    return true;
            //}
            //catch
            //{
                // ログ出力など必要に応じて
                return false;
            //}
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
