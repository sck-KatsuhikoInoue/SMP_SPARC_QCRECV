using Vertica.Data.VerticaClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EditorService
{
    public interface IVerticaDb
    {
        string ConnectionString { get; }
        int ConnectionTimeout { get; }
        int ExecuteTimeout { get; }

        VerticaDataReader Execute(string sql, Dictionary<string, object> parameters = null);
        Task<VerticaDataReader> ExecuteAsync(string sql, Dictionary<string, object> parameters = null);
    }
}
