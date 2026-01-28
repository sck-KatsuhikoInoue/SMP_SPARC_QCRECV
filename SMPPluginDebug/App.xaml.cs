using DXC.EngPF.Framework.Plugins;
using Prism.Ioc;
using Prism.Modularity;
using SMPPluginDebug.Views;
using System.Windows;

namespace SMPPluginDebug
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static PluginBase plugin;

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            plugin = new ProductRelationEditor.Spark.Plugin();
            containerRegistry.RegisterForNavigation(plugin.ViewType, plugin.ViewType.FullName);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            plugin.ConfigModuleCatalog(moduleCatalog);
        }
    }
}
