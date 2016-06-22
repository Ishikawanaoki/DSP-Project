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
    public partial class Egaku : Form
    {
        private const int CNSTMAX = 256;
        //private const int CNSTMAX = 512;
        private int Nmax;

        private double[] y = new double[CNSTMAX];
        private int[] y2 = new int[CNSTMAX];
        private int[] x2 = new int[CNSTMAX];

        public string fileName, safeFileName;
        private string fileout, yout;

        private string filename = @"C:\Users\N.Ishikawa\Desktop\data\";
        private int chart_id = 0;

        public Egaku(Form1 Form1)
        {
            InitializeComponent();
            safeFileName = Form1.safeFileName;
            fileName = Form1.fileName;

            // 画像を保存する名前を取得
            // filename : String
            filename += System.IO.Path.GetFileName(fileName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            filename += (chart_id++).ToString();
            filename += ".png";
            chart1.SaveImage(filename, System.Drawing.Imaging.ImageFormat.Png);
            chart1.Dispose();
            System.Diagnostics.Process p = System.Diagnostics.Process.Start(filename);
            button1.Enabled = false;
        }

        private void chart1_Click(object sender, EventArgs e)
        { }
        
         private void Plot()
        {
            String gname = "100Hz-2KAD";

            chart1.Series.Clear();  //グラフ初期化

            chart1.Series.Add(gname);
            
            int[] xValues = new int[y2.Length];

            chart1.Series[gname].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            for (int i = 0; i < xValues.Length; i++)
            {
                    //グラフに追加するデータクラスを生成
                    System.Windows.Forms.DataVisualization.Charting.DataPoint dp = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                    dp.SetValueXY(xValues[i], y2[i]);  //XとYの値を設定
                    dp.IsValueShownAsLabel = false;  //グラフに値を表示するように指定
                    chart1.Series[gname].Points.Add(dp);   //グラフにデータ追加
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            byosya byosya = new byosya(y);
            byosya.Show();
        }

        private void start_Click(object sender, EventArgs e)
        {
            includeFile();
            Plot();
            start.Enabled = false;
        }
        private void includeFile()
        {
            string buf;
            double aver, amax, sum, aby, seikika;
            aver = 0; amax = 0; sum = 0; aby = 0;  seikika = 0;
            Nmax = CNSTMAX;

            System.IO.StreamReader koeFile = new System.IO.StreamReader(fileName);

            for (int i = 0; i < Nmax; i++)
            {
                if (koeFile.Peek() == -1)    // ファイルの最後で有れば -1 を返す
                    break;

                buf = koeFile.ReadLine();

                y[i] = Convert.ToDouble(buf);

                sum += y[i];
                if (amax < y[i]) amax = y[i];
                
                y2[i] = Convert.ToInt32(buf);
                x2[i] = i;
            }
                koeFile.Close();
            aver = sum / Nmax; sum = 0;
            aby = amax;
            for (int i = 0; i < Nmax; i++)
            {
                y[i] -= aver;
                sum += y[i]; //　この最大値は、平均値を除去した後のもの【使わない】
                if (aby > y[i]) aby = y[i];
            }
            seikika = aby * (-1);
            if (seikika < amax) seikika = amax;
            // seikika は正規化をするために、信号値の絶対値の最大値を格納
            for (int i=0; i < Nmax; i++)
            {
                y[i] = y[i] / seikika * 100;
            }
            fileout = @"C:\Users\N.Ishikawa\Desktop\data\koeout.txt";
            System.IO.StreamWriter kekkaout = new System.IO.StreamWriter(fileout);
            for(int ii=0; ii<Nmax; ii++)
            {
                y2[ii] = Convert.ToInt32(y[ii]);
                //yout = y[ii].ToString("D10");
                yout = y2[ii].ToString("D10");
                kekkaout.WriteLine(yout);
            }
            kekkaout.Close();
            //  double aver, amax, sum, aby, seikika;
            Console.WriteLine("平均値 = {0}", aver);
            Console.WriteLine("最大値 = {0}", amax);
            Console.WriteLine("最小値 = {0}", aby);
            Console.WriteLine("正規化 = {0}", seikika);
        }

    }
}
