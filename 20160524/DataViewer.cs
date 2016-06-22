using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20160524
{
    public partial class DataViewer : Form
    {
        int[] y;
        public DataViewer()
        {
            InitializeComponent();
        }
        public DataViewer(int[] y)
        {
            this.y = y;
            String gname = "窓関数の効果";

            chart1.Series.Clear();  //グラフ初期化

            chart1.Series.Add(gname);

            int[] xValues = new int[y.Length];

            chart1.Series[gname].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            for (int i = 0; i < y.Length; i++)
            {
                //グラフに追加するデータクラスを生成
                System.Windows.Forms.DataVisualization.Charting.DataPoint dp = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                dp.SetValueXY(xValues[i], y[i]);  //XとYの値を設定
                dp.IsValueShownAsLabel = false;  //グラフに値を表示するように指定
                chart1.Series[gname].Points.Add(dp);   //グラフにデータ追加
            }
            InitializeComponent();
        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
