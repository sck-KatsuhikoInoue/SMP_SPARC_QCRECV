using DXC.EngPF.Framework.Plugins;
using System;

namespace ProductRelationEditor.Spark
{
    public class Plugin : PluginBase
    {
        public override string Id => "ProductRelationEditor";

        public override string Name => "製品紐付け定義アプリ(仮)";

        public override string Description => string.Empty;

        public override Type ViewType => typeof(Views.MainWindow);

        static Plugin() { }
    }
}
