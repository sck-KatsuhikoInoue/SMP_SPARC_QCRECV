using System.Windows.Controls;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Text;
using System;
using System.Collections.Generic;
using ProductRelationEditor.Spark.ViewModels;
using EditorService.Common.Dto;

namespace ProductRelationEditor.Spark.Views
{
    /// <summary>
    /// Interaction logic for PrismUserControl1
    /// </summary>
    public partial class MainWindow : UserControl
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnDisplay_Click(object sender, RoutedEventArgs e)
        {
            // 表示/非表示をトグルしたいカラム名リスト
            var targetHeaders = new[] { "TEC_KIND", "TIMESERIES_SEQ_NO", "M_PROCESS_NAME", "M_WORK_NAME", "M_FLD_ID", "M_WORK_CODE", "DCITEM_UNIT", "CTITLE" };

            foreach (var col in GrMaster.Columns)
            {
                if (targetHeaders.Contains(col.Header?.ToString()))
                {
                    col.Visibility = col.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }

        // DataGridのコピー・ペースト・削除処理
        private void GrMaster_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is not DataGrid grid) return;

            // --- コピー処理 ---
            if (e.Key == Key.C && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                var selectedCells = grid.SelectedCells;
                if (selectedCells.Count == 0) return;

                var rowIndices = selectedCells.Select(c => grid.Items.IndexOf(c.Item)).Distinct().OrderBy(i => i).ToArray();
                var colIndices = selectedCells.Select(c => grid.Columns.IndexOf(c.Column)).Distinct().OrderBy(i => i).ToArray();

                var sb = new StringBuilder();

                // ⑥ 1行全ての項目（または複数行全ての項目）選択時はヘッダ付きでコピー
                bool isFullRowSelection = colIndices.Length == grid.Columns.Count;
                if (isFullRowSelection)
                {
                    // ヘッダ行
                    foreach (var col in grid.Columns)
                    {
                        sb.Append(col.Header?.ToString());
                        sb.Append('\t');
                    }
                    sb.Length--;
                    sb.AppendLine();
                }

                // データ行
                foreach (var rowIdx in rowIndices)
                {
                    var rowItem = grid.Items[rowIdx];
                    foreach (var colIdx in (isFullRowSelection ? Enumerable.Range(0, grid.Columns.Count) : colIndices))
                    {
                        var col = grid.Columns[colIdx];
                        var prop = col.SortMemberPath;
                        if (!string.IsNullOrEmpty(prop))
                        {
                            var pi = rowItem.GetType().GetProperty(prop);
                            if (pi != null)
                            {
                                var value = pi.GetValue(rowItem)?.ToString() ?? "";
                                sb.Append(value);
                            }
                        }
                        sb.Append('\t');
                    }
                    sb.Length--;
                    sb.AppendLine();
                }
                Clipboard.SetText(sb.ToString().TrimEnd('\r', '\n'));
                e.Handled = true;
                return;
            }

            // --- ペースト処理 ---
            if (e.Key == Key.V && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                var pasteText = Clipboard.GetText();
                if (string.IsNullOrEmpty(pasteText)) return;

                var lines = pasteText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(l => l.Split('\t')).ToArray();
                var selectedCells = grid.SelectedCells;
                if (selectedCells.Count == 0) return;

                int nRows = lines.Length;
                int nCols = lines[0].Length;

                // ⑪ コピー元N×M → 1セル選択時、N×M範囲に貼り付け
                if (selectedCells.Count == 1 && (nRows > 1 || nCols > 1))
                {
                    var firstCell = selectedCells[0];
                    int startRow = grid.Items.IndexOf(firstCell.Item);
                    int startCol = grid.Columns.IndexOf(firstCell.Column);

                    for (int i = 0; i < nRows; i++)
                    {
                        int rowIdx = startRow + i;
                        if (rowIdx >= grid.Items.Count) break;
                        var rowItem = grid.Items[rowIdx];
                        for (int j = 0; j < nCols; j++)
                        {
                            int colIdx = startCol + j;
                            if (colIdx >= grid.Columns.Count) break;
                            var col = grid.Columns[colIdx];
                            if (col.IsReadOnly) continue; // ⑬ ReadOnly列は編集しない
                            var prop = col.SortMemberPath;
                            if (!string.IsNullOrEmpty(prop))
                            {
                                var pi = rowItem.GetType().GetProperty(prop);
                                if (pi != null && pi.CanWrite)
                                {
                                    var cellValue = lines[i][j];
                                    if (pi.PropertyType == typeof(bool))
                                    {
                                        bool boolValue = cellValue switch
                                        {
                                            "1" or "○" or "true" or "True" or "TRUE" => true,
                                            "0" or "×" or "false" or "False" or "FALSE" => false,
                                            _ => false
                                        };
                                        pi.SetValue(rowItem, boolValue);
                                    }
                                    else
                                    {
                                        pi.SetValue(rowItem, cellValue);
                                    }
                                }
                            }
                        }
                    }
                    grid.Items.Refresh();
                    e.Handled = true;
                    return;
                }

                // ⑫ コピー元N×M → 複数セル選択時、選択セル分ループしてN×M範囲内のみ貼り付け
                if ((nRows > 1 || nCols > 1) && selectedCells.Count > 1)
                {
                    int idx = 0;
                    for (int i = 0; i < nRows; i++)
                    {
                        for (int j = 0; j < nCols; j++)
                        {
                            if (idx >= selectedCells.Count) break;
                            var cell = selectedCells[idx++];
                            if (cell.Column.IsReadOnly) continue; // ⑬ ReadOnly列は編集しない
                            var prop = cell.Column.SortMemberPath;
                            if (!string.IsNullOrEmpty(prop))
                            {
                                var item = cell.Item;
                                var pi = item.GetType().GetProperty(prop);
                                if (pi != null && pi.CanWrite)
                                {
                                    var cellValue = lines[i][j];
                                    if (pi.PropertyType == typeof(bool))
                                    {
                                        bool boolValue = cellValue switch
                                        {
                                            "1" or "○" or "true" or "True" or "TRUE" => true,
                                            "0" or "×" or "false" or "False" or "FALSE" => false,
                                            _ => false
                                        };
                                        pi.SetValue(item, boolValue);
                                    }
                                    else
                                    {
                                        pi.SetValue(item, cellValue);
                                    }
                                }
                            }
                        }
                    }
                    grid.Items.Refresh();
                    e.Handled = true;
                    return;
                }

                // ① コピー元1セル → 貼り付け先の選択セル分、縦横ループ
                if (nRows == 1 && nCols == 1)
                {
                    string value = lines[0][0];
                    foreach (var cell in selectedCells)
                    {
                        if (cell.Column.IsReadOnly) continue; // ⑬ ReadOnly列は編集しない
                        var prop = cell.Column.SortMemberPath;
                        if (!string.IsNullOrEmpty(prop))
                        {
                            var item = cell.Item;
                            var pi = item.GetType().GetProperty(prop);
                            if (pi != null && pi.CanWrite)
                            {
                                if (pi.PropertyType == typeof(bool))
                                {
                                    bool boolValue = value switch
                                    {
                                        "1" or "○" or "true" or "True" or "TRUE" => true,
                                        "0" or "×" or "false" or "False" or "FALSE" => false,
                                        _ => false
                                    };
                                    pi.SetValue(item, boolValue);
                                }
                                else
                                {
                                    pi.SetValue(item, value);
                                }
                            }
                        }
                    }
                    grid.Items.Refresh();
                    e.Handled = true;
                    return;
                }

                // ② コピー元1列複数行 → 貼り付け先1セルなら縦方向に展開
                if (nCols == 1 && selectedCells.Count == 1)
                {
                    var firstCell = selectedCells[0];
                    int startRow = grid.Items.IndexOf(firstCell.Item);
                    int startCol = grid.Columns.IndexOf(firstCell.Column);
                    for (int i = 0; i < nRows; i++)
                    {
                        int rowIdx = startRow + i;
                        if (rowIdx >= grid.Items.Count) break;
                        var rowItem = grid.Items[rowIdx];
                        var col = grid.Columns[startCol];
                        if (col.IsReadOnly) continue; // ⑬ ReadOnly列は編集しない
                        var prop = col.SortMemberPath;
                        if (!string.IsNullOrEmpty(prop))
                        {
                            var pi = rowItem.GetType().GetProperty(prop);
                            if (pi != null && pi.CanWrite)
                            {
                                var cellValue = lines[i][0];
                                if (pi.PropertyType == typeof(bool))
                                {
                                    bool boolValue = cellValue switch
                                    {
                                        "1" or "○" or "true" or "True" or "TRUE" => true,
                                        "0" or "×" or "false" or "False" or "FALSE" => false,
                                        _ => false
                                    };
                                    pi.SetValue(rowItem, boolValue);
                                }
                                else
                                {
                                    pi.SetValue(rowItem, cellValue);
                                }
                            }
                        }
                    }
                    grid.Items.Refresh();
                    e.Handled = true;
                    return;
                }

                // ③ コピー元1列複数行 → 貼り付け先1列複数セルなら、上から順に貼り付け
                if (nCols == 1 && selectedCells.Select(c => grid.Columns.IndexOf(c.Column)).Distinct().Count() == 1 && selectedCells.Count == nRows)
                {
                    var orderedCells = selectedCells.OrderBy(c => grid.Items.IndexOf(c.Item)).ToArray();
                    for (int i = 0; i < nRows; i++)
                    {
                        var cell = orderedCells[i];
                        if (cell.Column.IsReadOnly) continue; // ⑬ ReadOnly列は編集しない
                        var prop = cell.Column.SortMemberPath;
                        if (!string.IsNullOrEmpty(prop))
                        {
                            var item = cell.Item;
                            var pi = item.GetType().GetProperty(prop);
                            if (pi != null && pi.CanWrite)
                            {
                                var cellValue = lines[i][0];
                                if (pi.PropertyType == typeof(bool))
                                {
                                    bool boolValue = cellValue switch
                                    {
                                        "1" or "○" or "true" or "True" or "TRUE" => true,
                                        "0" or "×" or "false" or "False" or "FALSE" => false,
                                        _ => false
                                    };
                                    pi.SetValue(item, boolValue);
                                }
                                else
                                {
                                    pi.SetValue(item, cellValue);
                                }
                            }
                        }
                    }
                    grid.Items.Refresh();
                    e.Handled = true;
                    return;
                }

                // ⑤ コピー元複数列1行 → 貼り付け先の選択セル分横に展開
                if (nRows == 1 && nCols > 1 && selectedCells.Count == nCols)
                {
                    var orderedCells = selectedCells.OrderBy(c => grid.Columns.IndexOf(c.Column)).ToArray();
                    for (int j = 0; j < nCols; j++)
                    {
                        var cell = orderedCells[j];
                        if (cell.Column.IsReadOnly) continue; // ⑬ ReadOnly列は編集しない
                        var prop = cell.Column.SortMemberPath;
                        if (!string.IsNullOrEmpty(prop))
                        {
                            var item = cell.Item;
                            var pi = item.GetType().GetProperty(prop);
                            if (pi != null && pi.CanWrite)
                            {
                                var cellValue = lines[0][j];
                                if (pi.PropertyType == typeof(bool))
                                {
                                    bool boolValue = cellValue switch
                                    {
                                        "1" or "○" or "true" or "True" or "TRUE" => true,
                                        "0" or "×" or "false" or "False" or "FALSE" => false,
                                        _ => false
                                    };
                                    pi.SetValue(item, boolValue);
                                }
                                else
                                {
                                    pi.SetValue(item, cellValue);
                                }
                            }
                        }
                    }
                    grid.Items.Refresh();
                    e.Handled = true;
                    return;
                }

                // ⑨ コピー元複数列1行 → 貼り付け先が1セルの場合は横に展開
                if (nRows == 1 && nCols > 1 && selectedCells.Count == 1)
                {
                    var firstCell = selectedCells[0];
                    int startRow = grid.Items.IndexOf(firstCell.Item);
                    int startCol = grid.Columns.IndexOf(firstCell.Column);
                    var rowItem = grid.Items[startRow];
                    for (int j = 0; j < nCols; j++)
                    {
                        int colIdx = startCol + j;
                        if (colIdx >= grid.Columns.Count) break;
                        var col = grid.Columns[colIdx];
                        if (col.IsReadOnly) continue; // ⑬ ReadOnly列は編集しない
                        var prop = col.SortMemberPath;
                        if (!string.IsNullOrEmpty(prop))
                        {
                            var pi = rowItem.GetType().GetProperty(prop);
                            if (pi != null && pi.CanWrite)
                            {
                                var cellValue = lines[0][j];
                                if (pi.PropertyType == typeof(bool))
                                {
                                    bool boolValue = cellValue switch
                                    {
                                        "1" or "○" or "true" or "True" or "TRUE" => true,
                                        "0" or "×" or "false" or "False" or "FALSE" => false,
                                        _ => false
                                    };
                                    pi.SetValue(rowItem, boolValue);
                                }
                                else
                                {
                                    pi.SetValue(rowItem, cellValue);
                                }
                            }
                        }
                    }
                    grid.Items.Refresh();
                    e.Handled = true;
                    return;
                }

                // ⑦⑧ 1行全て貼り付け時は先頭列のみ許可、ヘッダ除去
                if (nRows == 1 && nCols == grid.Columns.Count)
                {
                    var firstCell = selectedCells[0];
                    int startCol = grid.Columns.IndexOf(firstCell.Column);
                    if (startCol != 0) return; // 先頭列以外は貼り付け禁止

                    var rowItem = firstCell.Item;
                    for (int j = 0; j < grid.Columns.Count; j++)
                    {
                        var col = grid.Columns[j];
                        if (col.IsReadOnly) continue; // ⑬ ReadOnly列は編集しない
                        var prop = col.SortMemberPath;
                        if (!string.IsNullOrEmpty(prop))
                        {
                            var pi = rowItem.GetType().GetProperty(prop);
                            if (pi != null && pi.CanWrite)
                            {
                                var cellValue = lines[0][j];
                                if (pi.PropertyType == typeof(bool))
                                {
                                    bool boolValue = cellValue switch
                                    {
                                        "1" or "○" or "true" or "True" or "TRUE" => true,
                                        "0" or "×" or "false" or "False" or "FALSE" => false,
                                        _ => false
                                    };
                                    pi.SetValue(rowItem, boolValue);
                                }
                                else
                                {
                                    pi.SetValue(rowItem, cellValue);
                                }
                            }
                        }
                    }
                    grid.Items.Refresh();
                    e.Handled = true;
                    return;
                }

                // それ以外は何もしない
            }

            // --- 削除処理 ---
            if (e.Key == Key.Delete)
            {
                var selectedCells = grid.SelectedCells;
                if (selectedCells.Count == 0) return;

                foreach (var cell in selectedCells)
                {
                    if (cell.Column.IsReadOnly) continue; // ⑬ ReadOnly列は編集しない
                    var prop = cell.Column.SortMemberPath;
                    if (!string.IsNullOrEmpty(prop))
                    {
                        var item = cell.Item;
                        var pi = item.GetType().GetProperty(prop);
                        if (pi != null && pi.CanWrite)
                        {
                            if (pi.PropertyType == typeof(bool))
                            {
                                pi.SetValue(item, false);
                            }
                            else if (pi.PropertyType == typeof(string))
                            {
                                pi.SetValue(item, "");
                            }
                            else if (pi.PropertyType.IsValueType)
                            {
                                pi.SetValue(item, Activator.CreateInstance(pi.PropertyType));
                            }
                            else
                            {
                                pi.SetValue(item, null);
                            }
                        }
                    }
                }
                grid.Items.Refresh();
                e.Handled = true;
                return;
            }
        }
    }
}
