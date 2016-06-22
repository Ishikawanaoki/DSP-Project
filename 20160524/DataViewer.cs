using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace _20160524
{
    public partial class DataViewer : Form
    {
        int[] y;
        int[] xValues;
        public DataViewer()
        {
            InitializeComponent();
        }
        public DataViewer(int[] y)
        {
            this.y = y;
            InitializeComponent();

            String gname = "窓関数の効果";

            chart1.Series.Clear();  //グラフ初期化
            chart1.Series.Add(gname);
            xValues = new int[this.y.Length];
            chart1.Series[gname].ChartType = SeriesChartType.Line;

            for (int i = 0; i < this.y.Length; i++)
            {
                xValues[i] = i;

                //グラフに追加するデータクラスを生成
                DataPoint dp = new DataPoint();
                dp.SetValueXY(xValues[i], this.y[i]);  //XとYの値を設定
                dp.IsValueShownAsLabel = false;  //グラフに値を表示するように指定
                chart1.Series[gname].Points.Add(dp);   //グラフにデータ追加
            }
        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
