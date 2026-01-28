using DXC.EngPF.Framework.Mvvm;
using Prism.Commands;
using Prism.Regions;

namespace SMPPluginDebug.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [Unity.Dependency]
        public IRegionManager _regionManager { get; set; }
        public DelegateCommand LoadedCommand { get; }

        public MainWindowViewModel()
        {
            LoadedCommand = new DelegateCommand(Loaded);
        }

        public void Loaded()
        {
            _regionManager.RequestNavigate("ContentRegion", App.plugin.ViewType.FullName);
        }
    }
}
