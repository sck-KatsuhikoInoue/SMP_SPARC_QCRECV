using DXC.EngPF.Framework.Common;
using Prism.Commands;
using Prism.Services.Dialogs;
using ProductRelationEditor.Spark.Models;
using ProductRelationEditor.Spark.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System;
using System.Collections.Generic;

namespace ProductRelationEditor.Spark.ViewModels
{
    public class MainWindowViewModel : PortalViewModelBase
    {
        // API呼び出し用のServiceクラス
        private Service _service;

        // 画面表示用のデータリスト
        public ObservableCollection<ItemModel> Items => _items;
        private readonly ObservableCollection<ItemModel> _items;

        public string WindowsUserName { get; } = Environment.UserName;

        #region "// Prismのプロパティ変更通知実装 ///"
         // TecKindリスト表示プロパティ
        public ObservableCollection<string> TecKindList { get; } = new();

        //TecKind選択プロパティ
        private object _selectedTecKind;
        public object SelectedTecKind
        {
            get => _selectedTecKind;
            set
            {
                if (_selectedTecKind != value)
                {
                    _selectedTecKind = value;
                    RaisePropertyChanged(nameof(SelectedTecKind));

                    // ① カテゴリ再検索
                    SearchCategoryList();
                    // 装置・グループ名クリア
                    SelectedEquipment = null;
                    SelectedGroupName = null;
                    EquipmentList.Clear();
                    GroupNameList.Clear();
                }
            }
        }

        // Categoryリスト表示プロパティ
        public ObservableCollection<string> CategoryList { get; } = new();

        //Category選択プロパティ
        private object _selectedCategory;
        public object SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    RaisePropertyChanged(nameof(SelectedCategory));

                    // ② 装置再検索
                    SearchEquipmentList();
                    // グループ名クリア
                    SelectedGroupName = null;
                    GroupNameList.Clear();
                }
            }
        }

        // Equipmentリスト表示プロパティ
        public ObservableCollection<string> EquipmentList { get; } = new();

        //Equipment選択プロパティ
        private string _selectedEquipment;
        public string SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                if (_selectedEquipment != value)
                {
                    _selectedEquipment = value;
                    RaisePropertyChanged(nameof(SelectedEquipment)); // ComboBoxの選択変更を通知

                    // ③ グループ再検索
                    SearchGroupNameList();
                }
            }
        }

        // GroupNameリスト表示プロパティ
        public ObservableCollection<string> GroupNameList { get; } = new();

        //GroupName選択プロパティ
        private string _selectedGroupName;
        public string SelectedGroupName
        {
            get => _selectedGroupName;
            set
            {
                if (_selectedGroupName != value)
                {
                    _selectedGroupName = value;
                    RaisePropertyChanged(nameof(SelectedGroupName)); // ComboBoxの選択変更を通知
                }
            }
        }

        // データ取得用のコマンド ButtonとBinding
        public DelegateCommand FetchDataCommand { get; }
        // データ登録用のコマンド ButtonとBinding
        public DelegateCommand RegisterDataCommand { get; }

        #endregion

        public MainWindowViewModel(IDialogService dialogService, PortalContext context) : base(dialogService)
        {
            // SMP上で起動した場合のみ取得可能。デバッグ起動の場合はnullになる。
            // ex. https://iskmtap01v.sck.sony.co.jp/
            var connectedServer = context?.ActiveServer?.ServerAddress;

            // SMP上で起動した場合のみ取得可能。デバッグ起動の場合はnullになる。
            var sessionId = context?.SessionId;

            _service = new Service(connectedServer ?? "https://defaultserver.co.jp/");

            // 画面表示用のデータリスト（UIと連動するコレクション）を初期化
            _items = new ObservableCollection<ItemModel>();

            // データ取得ボタン押下時にFetchDataAsyncを非同期実行するコマンドを生成（UIとバインド用）
            FetchDataCommand = new DelegateCommand(async () => await FetchDataAsync());
            // RegisterDataCommandは何もしない（ViewのOnRegisterDataで処理）
            RegisterDataCommand = new DelegateCommand(() => { });

            // TecKind一覧の初期化
            _ = InitializeTecKindListAsync();
        }

        #region "ComboBoxリスト取得"
        // TecKind一覧取得処理
        private async Task InitializeTecKindListAsync()
        {
            var tecKinds = await _service.EditorServiceTecKindList();
            TecKindList.Clear();
            foreach (var kind in tecKinds)
            {
                TecKindList.Add(kind);
            }
        }

        // カテゴリリスト再検索
        private void SearchCategoryList()
        {
            CategoryList.Clear();
            // 非同期でカテゴリリストを再取得
            _ = SearchCategoryListAsync();
        }

        private async Task SearchCategoryListAsync()
        {
            // 検索条件をJSONで作成
            var searchCondition = new
            {
                TEC_KIND = _selectedTecKind,
                CCATEGORY = string.Empty,
                EQP_ID = string.Empty,
                GNAME = string.Empty
            };
            var searchConditionJson = System.Text.Json.JsonSerializer.Serialize(searchCondition);

            var categories = await _service.EditorServiceCategoryList(searchConditionJson);
            CategoryList.Clear();
            foreach (var kind in categories)
            {
                CategoryList.Add(kind);
            }
        }

        // 装置リスト再検索
        private void SearchEquipmentList()
        {
            EquipmentList.Clear();
            _ = SearchEquipmentListAsync();
        }

        private async Task SearchEquipmentListAsync()
        {
            // 検索条件をJSONで作成
            var searchCondition = new
            {
                TEC_KIND = _selectedTecKind,
                CCATEGORY = _selectedCategory,
                EQP_ID = string.Empty,
                GNAME = string.Empty
            };
            var searchConditionJson = System.Text.Json.JsonSerializer.Serialize(searchCondition);

            var equipments = await _service.EditorServiceEquipmentList(searchConditionJson);
            EquipmentList.Clear();
            foreach (var kind in equipments)
            {
                EquipmentList.Add(kind);
            }
        }

        // グループ名リスト再検索
        private void SearchGroupNameList()
        {
            GroupNameList.Clear();
            _ = SearchGroupNameListAsync();
        }

        private async Task SearchGroupNameListAsync()
        {
            var searchCondition = new
            {
                TEC_KIND = _selectedTecKind,
                CCATEGORY = _selectedCategory,
                EQP_ID = _selectedEquipment,
                GNAME = string.Empty
            };
            var searchConditionJson = System.Text.Json.JsonSerializer.Serialize(searchCondition);

            var groupNames = await _service.EditorServiceGroupNameList(searchConditionJson);
            GroupNameList.Clear();
            foreach (var kind in groupNames)
            {
                GroupNameList.Add(kind);
            }
        }

        #endregion

        #region "データ取得・登録処理"
        // データ取得処理
        public async Task FetchDataAsync()
        {
            // 検索条件をJSONで作成
            var searchCondition = new
            {
                TEC_KIND = _selectedTecKind,
                CCATEGORY = _selectedCategory,
                EQP_ID = _selectedEquipment,
                GNAME = _selectedGroupName
            };
            var searchConditionJson = System.Text.Json.JsonSerializer.Serialize(searchCondition);

            //// 検索条件をJSONで作成
            var fetchedItems = await _service.EditorServiceSearchSpcChart(searchConditionJson);

            Items.Clear();
            foreach (var item in fetchedItems)
            {
                Items.Add(item);
            }
        }

        // データ登録処理（Service経由でDB登録のみ）
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
        #endregion
    }
}
