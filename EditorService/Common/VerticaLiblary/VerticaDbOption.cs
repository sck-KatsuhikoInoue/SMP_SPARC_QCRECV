using System.Runtime.CompilerServices;
using System.Configuration;

namespace EditorService
{
    public sealed class VerticaDbOption
    {
        public string ConnectionString { get; set; }
        public int ConnectionTimeoutSeconds { get; set; }
        public int ExecuteTimeoutSeconds { get; set; }

        public static VerticaDbOption FromConfig()
        {
            return new VerticaDbOption
            {
                ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["VerticaDb"].ConnectionString,
                ConnectionTimeoutSeconds = int.Parse(System.Configuration.ConfigurationManager.AppSettings["VerticaConnectionTimeoutSeconds"]),
                ExecuteTimeoutSeconds = int.Parse(System.Configuration.ConfigurationManager.AppSettings["VerticaExecuteTimeoutSeconds"])
            };
        }
    }
}
