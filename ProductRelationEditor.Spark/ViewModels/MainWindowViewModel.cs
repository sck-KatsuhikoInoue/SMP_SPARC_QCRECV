using DXC.EngPF.Framework.Common;
using Prism.Commands;
using Prism.Services.Dialogs;
using ProductRelationEditor.Spark.Models;
using ProductRelationEditor.Spark.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace ProductRelationEditor.Spark.ViewModels
{
    public class MainWindowViewModel : PortalViewModelBase
    {
        // API呼び出し用のServiceクラス
        private Service _service;

        // 画面表示用のデータリスト
        public ObservableCollection<ItemModel> Items => _items;
        private readonly ObservableCollection<ItemModel> _items;

        // データ取得用のコマンド ButtonとBinding
        public DelegateCommand FetchDataCommand { get; }
        // データ登録用のコマンド ButtonとBinding
        public DelegateCommand RegisterDataCommand { get; }

        public MainWindowViewModel(IDialogService dialogService, PortalContext context) : base(dialogService)
        {
            // SMP上で起動した場合のみ取得可能。デバッグ起動の場合はnullになる。
            // ex. https://iskmtap01v.sck.sony.co.jp/
            var connectedServer = context?.ActiveServer?.ServerAddress;

            // SMP上で起動した場合のみ取得可能。デバッグ起動の場合はnullになる。
            var sessionId = context?.SessionId;

            _service = new Service(connectedServer ?? "https://defaultserver.co.jp/");
            _items = new ObservableCollection<ItemModel>();

            FetchDataCommand = new DelegateCommand(async () => await FetchDataAsync());
            RegisterDataCommand = new DelegateCommand(async () => await Register());
        }

        public async Task FetchDataAsync()
        {
            // 検索条件をJSONで作成
            var searchCondition = new {
                TEC_KIND = "6",
                CCATEGORY = "EQC",
                EQP_ID = "L101Y2",
                GNAME = "L101Y2"
            };
            var searchConditionJson = System.Text.Json.JsonSerializer.Serialize(searchCondition);

            var fetchedItems = await _service.EditorServiceSearchSpcChart(searchConditionJson);

            Items.Clear();
            foreach (var item in fetchedItems)
            {
                Items.Add(item);
            }
        }

        public async Task Register()
        {
            var result = await _service.RegisterDataAsync();
            if (result)
            {
                MessageBox.Show("登録に成功しました");
            }
            else
            {
                MessageBox.Show("登録に失敗しました");
            }
        }
    }
}
