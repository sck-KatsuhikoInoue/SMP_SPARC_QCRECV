using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ProductRelationEditor.Spark
{
    public class ProductRelationEditorModule : IModule
    {
        [Unity.Dependency]
        public IRegionManager RegionManager { get; set; }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            try
            {
                RegionManager.RegisterViewWithRegion(typeof(Views.MainWindow).FullName, typeof(Views.MainWindow));
            }
            catch
            {
                throw;
            }
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            try
            {
                containerRegistry.RegisterForNavigation<Views.MainWindow>(typeof(Views.MainWindow).FullName);
            }
            catch
            {
                throw;
            }
        }
    }
}