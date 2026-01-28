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
                ConnectionString = Properties.Settings.Default.VerticaDb,
                ConnectionTimeoutSeconds = Properties.Settings.Default.VerticaConnectionTimeoutSeconds,
                ExecuteTimeoutSeconds = Properties.Settings.Default.VerticaExecuteTimeoutSeconds
            };
        }
    }
}
