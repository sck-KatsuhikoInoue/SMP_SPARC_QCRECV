using System;
using System.Configuration;
using System.Web;

public class Global : HttpApplication
{
    protected void Application_Start(object sender, EventArgs e)
    {
        // Web.config のパスを取得
        var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

        // Settings.settings から値を取得
        var verticaDb = EditorService.Properties.Settings.Default.VerticaDb;
        var connectionTimeout = EditorService.Properties.Settings.Default.VerticaConnectionTimeoutSeconds.ToString();
        var executeTimeout = EditorService.Properties.Settings.Default.VerticaExecuteTimeoutSeconds.ToString();

        // appSettings の値を更新
        config.AppSettings.Settings["VerticaDb"].Value = verticaDb;
        config.AppSettings.Settings["VerticaConnectionTimeoutSeconds"].Value = connectionTimeout;
        config.AppSettings.Settings["VerticaExecuteTimeoutSeconds"].Value = executeTimeout;

        // Web.config を保存
        config.Save();
    }
}