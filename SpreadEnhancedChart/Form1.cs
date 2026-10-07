using FarPoint.Win.Spread;
using GrapeCity.Spreadsheet.Charts;
using GrapeCity.Spreadsheet.Drawing;

namespace SpreadEnhancedChart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // 拡張シェイプエンジンと拡張チャートエンジンの有効化
            fpSpread1.Features.EnhancedShapeEngine = true;
            fpSpread1.Features.EnhancedChartEngine = true;

            // ヘッダのハイライト表示の抑止とフォーカス枠の非表示
            fpSpread1.FocusRenderer = new DefaultFocusIndicatorRenderer(0);
            fpSpread1.PaintSelectionHeader = false;

            // データ用シートの設定
            var sheet1 = fpSpread1.Sheets[0].AsWorksheet();
            sheet1.Name = "データ";
            sheet1.Cells.Font.Name = "メイリオ";
            sheet1.Cells.Font.Size = 11;
            sheet1.Cells.EntireColumn.AutoFit();
            sheet1.SetValue(0, 0, new object[,]
            {
                { "月", "仙台", "デリー" },
                { "1月",1.8, 13.5 },
                { "2月", 4.7, 19.3 },
                { "3月", 6.8, 25.1},
                { "4月", 12.7, 28.1 },
                { "5月",17.4, 32.6 },
                { "6月", 19.6, 33.3 },
                { "7月", 24.6, 31.6 },
                { "8月", 24.6, 29.5 },
            });

            // 拡張チャート用シートの設定
            var sheet2 = fpSpread1.AsWorkbook().Worksheets.Add("拡張チャート");
            sheet2.Activate();

            // 拡張チャートの作成
            sheet2.Shapes.AddChart(ChartType.LineMarkers, 0, 0, 1100, 684);
            sheet2.ChartObjects[0].Chart.SetSourceData(sheet1.ActiveCell["A1:C9"]);
            var chart = sheet2.ChartObjects[0].Chart;

            // チャートスタイル
            chart.ChartStyle = BuiltInChartStyles.Line9;

            // データテーブル
            chart.ShowDataTable();
            var dataTable = chart.DataTable;
            dataTable.Font.Name = "メイリオ";
            dataTable.Font.Size = 11;

            // 第2系列の折れ線チャートのスムージング
            chart.Series[1].Smooth = true;

            // チャートタイトル
            var title = chart.ChartTitle;
            title.Text = "2026年の月平均気温";
            title.Format.TextFrame.TextRange.Font.Name = "メイリオ";
            title.Format.TextFrame.TextRange.Font.Size = 20;

            // X軸
            var categoryAxis = chart.Axes[AxisType.Category];
            categoryAxis.Format.Line.ForeColor.ARGB = Color.Silver.ToArgb();
            categoryAxis.Format.Line.Visible = true;
            categoryAxis.Format.Line.Weight = 2;
            categoryAxis.TickLabels.Font.Name = "メイリオ";
            categoryAxis.TickLabels.Font.Size = 11;

            // Y軸
            var valueAxis = chart.Axes[AxisType.Value];
            valueAxis.DisplayUnitCustom = 1;
            valueAxis.Format.Line.ForeColor.ARGB = Color.Silver.ToArgb();
            valueAxis.Format.Line.Visible = true;
            valueAxis.Format.Line.Weight = 2;
            valueAxis.TickLabels.Font.Name = "メイリオ";
            valueAxis.TickLabels.Font.Size = 11;
            var unit = valueAxis.DisplayUnitLabel;
            unit.Format.TextFrame.TextRange.Font.Name = "メイリオ";
            unit.Format.TextFrame.TextRange.Font.Size = 11;
            unit.Orientation = 0;
            unit.Text = "[℃]";

            // データ系列
            chart.ApplyDataLabels(DataLabelVisibilities.Value);
            for (var i = 0; i < chart.Series.Count; i++)
            {
                // ChartStyle設定により変更されたチャートタイプの再設定
                chart.Series[i].ChartType = ChartType.LineMarkers;
                chart.Series[i].MarkerSize = 12;

                // データラベル
                var labels = chart.Series[i].DataLabels;
                labels.Format.TextFrame.TextRange.Font.Name = "メイリオ";
                labels.Format.TextFrame.TextRange.Font.Size = 11;
                if (i == 0)
                {
                    // 系列のデータラベル位置の指定
                    labels.Position = DataLabelPosition.Bottom;

                    // データ点のデータラベル位置の指定
                    labels[0].Position = DataLabelPosition.Top;
                }
                else
                {
                    // 系列のデータラベル位置の指定
                    labels.Position = DataLabelPosition.Top;

                    // データ点のデータラベルの向きと値の変更
                    labels[1].TextOrientation = TextOrientation.Downward;
                    labels[1].AutoText = false;
                    labels[1].Text = $"テキストの向きと値の変更 ({chart.Series[i].Values[0]})";

                    // データ点のデータラベルの書式変更
                    labels[1].Format.Fill.BackColor.ARGB = Color.Lavender.ToArgb();
                    labels[1].Format.TextFrame.TextRange.Font.Bold = true;
                    labels[1].Format.TextFrame.TextRange.Font.Fill.ForeColor.ARGB = Color.Blue.ToArgb();
                }
            }

            // 凡例
            chart.ShowLegend();
            var legend = chart.Legend;
            legend.Format.TextFrame.TextRange.Font.Name = "@メイリオ";
            legend.Format.TextFrame.TextRange.Font.Size = 12;
            legend.LegendEntries[0].Delete();
            legend.Position = LegendPosition.Right;
            legend.TextOrientation = TextOrientation.Downward;
        }
    }
}
