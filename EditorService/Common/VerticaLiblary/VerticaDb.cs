using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vertica.Data.VerticaClient;

namespace EditorService
{
    public class VerticaDb : IVerticaDb
    {
        public string ConnectionString { get; }
        public int ConnectionTimeout { get; }
        public int ExecuteTimeout { get; }
        public int SqlInClauseLimit { get; }
        public int ChipMeasureDataChunkSize { get; }

        public VerticaDb(VerticaDbOption option)
        {
            ConnectionString = option.ConnectionString;
            ConnectionTimeout = option.ConnectionTimeoutSeconds;
            ExecuteTimeout = option.ExecuteTimeoutSeconds;
        }

        public VerticaDataReader Execute(string sql, Dictionary<string, object> parameters = null)
        {
            VerticaConnection connection = null;
            try
            {
                var builder = new VerticaConnectionStringBuilder(ConnectionString)
                {
                    ConnectionTimeout = ConnectionTimeout
                };
                connection = new VerticaConnection(builder.ToString());
                connection.Open();

                using (var cmd = new VerticaCommand(sql, connection)
                {
                    CommandTimeout = ExecuteTimeout
                })
                {
                    cmd.Parameters.Clear();
                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> param in parameters)
                        {
                            cmd.Parameters.Add(new VerticaParameter(param.Key, param.Value));
                        }
                    }

                    return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                if (connection?.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                throw new VerticaDbException("VerticaDbでエラーが発生しました", ex);
            }
        }

        // 修正: VerticaConnection に CloseAsync メソッドが存在しないため、代わりに Close メソッドを使用します。
        public async Task<VerticaDataReader> ExecuteAsync(string sql, Dictionary<string, object> parameters = null)
        {
            VerticaConnection connection = null;
            try
            {
                var builder = new VerticaConnectionStringBuilder(ConnectionString)
                {
                    ConnectionTimeout = ConnectionTimeout
                };
                connection = new VerticaConnection(builder.ToString());
                await Task.Run(() => connection.Open()); // 非同期的に接続を開く

                using (var cmd = new VerticaCommand(sql, connection)
                {
                    CommandTimeout = ExecuteTimeout
                })
                {
                    cmd.Parameters.Clear();
                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> param in parameters)
                        {
                            cmd.Parameters.Add(new VerticaParameter(param.Key, param.Value));
                        }
                    }

                    return (VerticaDataReader)await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                if (connection?.State == System.Data.ConnectionState.Open)
                {
                    connection.Close(); // CloseAsync の代わりに Close を使用
                }
                throw new VerticaDbException("VerticaDbでエラーが発生しました", ex);
            }
        }
    }
}
