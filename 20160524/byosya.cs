using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace _20160524
{
    public partial class byosya : Form
    {
        private const int CNSTMAX = 256;
        //private const int CNSTMAX = 512;
        private double[] y = new double[CNSTMAX];
        double[] y_out = new double[CNSTMAX];
        int[] y2 = new int[CNSTMAX];
        private int[] x2 = new int[CNSTMAX];
        private int chart_id = 0;
        private string chart_name;
        public byosya()
        {
            InitializeComponent();
        }
        public byosya(double[] y)
        {
            this.y = y;
            InitializeComponent();
        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }
        private void test()
        {
            Complex[] sign = new Complex[CNSTMAX];
            Complex[] do_dft = new Complex[CNSTMAX];
            //Complex[] do_fft = new Complex[Nmax];
            double seikika = 0;
            for (int i = 0; i < CNSTMAX; i++)
            {
                sign[i] = new Complex(y[i], 0);
            }
            do_dft = Fourier.DFT(sign);

            for (int ii = 0; ii < CNSTMAX; ii++)
            {
                y_out[ii] = do_dft[ii].magnitude;
                if (seikika < y_out[ii]) seikika = y_out[ii];
            }
            for (int iii =0; iii < CNSTMAX; iii++)
            {
                y_out[iii] = y_out[iii] / seikika * 100;
                y2[iii] = Convert.ToInt32(y_out[iii]); //y2のdftをノルム出力したものをy2に格納します。

            }
            String fileout = @"C:\Users\N.Ishikawa\Desktop\data\dft_out.txt";
            String yout;
            System.IO.StreamWriter kekkaout = new System.IO.StreamWriter(fileout);
            for (int iii = 0; iii < CNSTMAX; iii++)
            {
                //y2[iii] = Convert.ToInt32(y[iii]);
                yout = y2[iii].ToString("D10");
                kekkaout.WriteLine(yout);
            }
            kekkaout.Close();
        }
        private void test2()
        {
            Complex[] sign = new Complex[CNSTMAX];
            Complex[] do_fft = new Complex[CNSTMAX];
            //Complex[] do_fft = new Complex[Nmax];
            double seikika = 0;
            for (int i = 0; i < CNSTMAX; i++)
            {
                sign[i] = new Complex(y[i], 0);
            }
            do_fft = Fourier.FFT(sign);

            for (int ii = 0; ii < CNSTMAX; ii++)
            {
                y_out[ii] = do_fft[ii].magnitude;
                if (seikika < y_out[ii]) seikika = y_out[ii];
            }
            for (int iii = 0; iii < CNSTMAX; iii++)
            {
                y_out[iii] = y_out[iii] / seikika * 100;
                y2[iii] = Convert.ToInt32(y_out[iii]); //y2のdftをノルム出力したものをy2に格納します。

            }
            String fileout = @"C:\Users\N.Ishikawa\Desktop\data\fft_out.txt";
            String yout;
            System.IO.StreamWriter kekkaout = new System.IO.StreamWriter(fileout);
            for (int iii = 0; iii < CNSTMAX; iii++)
            {
                //y2[iii] = Convert.ToInt32(y[iii]);
                yout = y2[iii].ToString("D10");
                kekkaout.WriteLine(yout);
            }
            kekkaout.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            test();
            Plot("dft-100Hz-2KAD");
            label();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            test2();
            Plot("fft-100Hz-2KAD");
            label();
        }
        private void label()
        {
            int max = 0;
            int max_num = 0;
            int num = 0;
            for (int j = 0; j < y2.Length; j++)
            {
                if (max < y2[j])
                {
                    max = y2[j];
                    max_num = j;
                }
                if (max == y2[j])
                {
                    num++;
                }

            }
            //Dim s As String;
            String s = "max = ";
            s += max.ToString();
            s += "\nmax_num = ";
            //s += ((double)(max_num*3.9)).ToString();
            s += max_num.ToString();
            label1.Text = s;

        }
        private void Plot(String str)
        {
            //
            // Titles,Series,ChartAreasはchartコントロール直下のメンバ
            chart1.Titles.Clear();
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            Series seriesLine = new Series();
            seriesLine.ChartType = SeriesChartType.Line; // 折れ線グラフ
            seriesLine.LegendText = "Legend:Line";       // 凡例

            chart1.ChartAreas.Add(new ChartArea("Area1"));            // ChartArea作成
            chart1.ChartAreas["Area1"].AxisX.Title = "周波数 N [Hz]";  // X軸タイトル設定
            chart1.ChartAreas["Area1"].AxisY.Title = "ノルム F [N]";  // Y軸タイトル設定
            
            // 以上、初期化処理

            String ChartName = str;
            string[] xValues = new string[(x2.Length/2)];
            int[] plot_y = new int[x2.Length / 2];
            for(int j=0; j<plot_y.Length; j++)
            {
                //plot_y[j] = y2[plot_y.Length - j];
                //y2[j] = Convert.ToInt32(Math.Log10(y2[j])*50);
                int max = 1;
                for(int i=0; i<plot_y.Length; i++)
                {
                    if (max < y2[i]) max = y2[i];
                }
                y2[j] = (int)Math.Log10(y2[j]/max) * 50;
                plot_y[j] = y2[j];
            }

            chart1.Series.Clear();  //グラフ初期化

            chart1.Series.Add(ChartName);

            chart1.Series[ChartName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            //x2[0] = x2.Length / 2 * 4; // x[0] はグラフ描写の開始点
            for (int i = 0; i < xValues.Length; i++)
            {
                //if(i+1 < x2.Length) x2[i+1] = x2[i] - 4;
                //x2[i] = i * 4;
                x2[i] = i;
                xValues[i] = x2[i].ToString();
                //グラフに追加するデータクラスを生成
                DataPoint dp = new DataPoint();
                //dp.SetValueXY(xValues[i], y2[]);  //XとYの値を設定
                dp.SetValueXY(xValues[i], plot_y[i]);
                dp.IsValueShownAsLabel = false;  //グラフに値を表示するように指定
                chart1.Series[ChartName].Points.Add(dp);   //グラフにデータ追加
            }

            chart_name += chart_id.ToString();
            chart_id++;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //PrintDocumentオブジェクトの作成
            System.Drawing.Printing.PrintDocument pd =
                new System.Drawing.Printing.PrintDocument();
            //PrintPageイベントハンドラの追加
            pd.PrintPage +=
                new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);

            //PrintDialogクラスの作成
            PrintDialog pdlg = new PrintDialog();
            //PrintDocumentを指定
            pdlg.Document = pd;
            //印刷の選択ダイアログを表示する
            if (pdlg.ShowDialog() == DialogResult.OK)
            {
                //OKがクリックされた時は印刷する

            }

        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            //throw new NotImplementedException();

            //画像を読み込む
            //Image img = chart1.Images;
            //画像を描画する
            //e.Graphics.DrawImage(img, e.MarginBounds);
            //次のページがないことを通知する
            //e.HasMorePages = false;
            //後始末をする
            //img.Dispose();
        }
    }
    internal class Complex
    {
        public double real = 0.0;
        public double imag = 0.0;
        //Empty constructor
        public Complex()
        {
        }
        public Complex(double real, double img)
        {
            this.real = real;
            this.imag = img;
        }
        override public string ToString()
        {
            string data = real.ToString() + " " + imag.ToString() + "i";
            return data;
        }
        //Convert from polar to rectangular
        public static Complex from_polar(double r, double radians)
        {
            Complex data = new Complex(r * Math.Cos(radians), r * Math.Sin(radians));
            return data;
        }
        //Override addition operator
        public static Complex operator +(Complex a, Complex b)
        {
            Complex data = new Complex(a.real + b.real, a.imag + b.imag);
            return data;
        }
        //Override subtraction operator
        public static Complex operator -(Complex a, Complex b)
        {
            Complex data = new Complex(a.real - b.real, a.imag - b.imag);
            return data;
        }
        //Override multiplication operator
        public static Complex operator *(Complex a, Complex b)
        {
            Complex data = new Complex((a.real * b.real) - (a.imag * b.imag),
           (a.real * b.imag + (a.imag * b.real)));
            return data;
        }
        //Return magnitude of complex number
        public double magnitude
        {
            get
            {
                return Math.Sqrt(Math.Pow(real, 2) + Math.Pow(imag, 2));
            }
        }
        public double phase
        {
            get
            {
                return Math.Atan(imag / real); // アークタンジェントを返し、-n/2<=theta<=n/2となる値を返す
            }
        }
    }
    internal class Fourier
    {
        public enum WindowFunc
        {
            Hamming,
            Hanning,
            Blackman,
            Rectangular
        }
        public static Complex[] HannWindow(Complex[] x)
        {
            int N = x.Length;

            return x; //Disenable
        } // 窓関数
        public static double[] Windowing(double[] data, WindowFunc windowFunc)
        {
            int size = data.Length;
            double[] windata = new double[size];

            for (int i = 0; i < size; i++)
            {
                double winValue = 0;
                // 各々の窓関数
                if (WindowFunc.Hamming == windowFunc)
                {
                    winValue = 0.54 - 0.46 * Math.Cos(2 * Math.PI * i / (size - 1));
                }
                else if (WindowFunc.Hanning == windowFunc)
                {
                    winValue = 0.5 - 0.5 * Math.Cos(2 * Math.PI * i / (size - 1));
                }
                else if (WindowFunc.Blackman == windowFunc)
                {
                    winValue = 0.42 - 0.5 * Math.Cos(2 * Math.PI * i / (size - 1))
                                    + 0.08 * Math.Cos(4 * Math.PI * i / (size - 1));
                }
                else if (WindowFunc.Rectangular == windowFunc)
                {
                    winValue = 1.0;
                }
                else
                {
                    winValue = 1.0;
                }
                // 窓関数を掛け算
                windata[i] = data[i] * winValue;
            }
            return windata;
        }

        public static Complex[] DFT(Complex[] x)
        {
            int N = x.Length;
            Complex[] X = new Complex[N];
            double d_theta = (-2) * Math.PI / N;
            for (int k = 0; k < N; k++)
            {
                X[k] = new Complex(0, 0);
                for (int n = 0; n < N; n++)
                {
                    // Complex temp = Complex.from_polar(1, -2 * Math.PI * n * k / N);
                    Complex temp = Complex.from_polar(1, d_theta * n * k);
                    temp *= x[n]; //演算子 * はオーバーライドしたもの
                    X[k] += temp; //演算子 + はオーバーライドしたもの
                }
            }
            return X;
        }
        public static Complex[] FFT(Complex[] x)
        {
            int N = x.Length;
            Complex[] X = new Complex[N];
            Complex[] d, D, e, E;
            if (N == 1)
            {
                X[0] = x[0];
                return X;
            }
            int k;
            e = new Complex[N / 2];
            d = new Complex[N / 2];
            for (k = 0; k < N / 2; k++)
            {
                e[k] = x[2 * k];
                d[k] = x[2 * k + 1];
            }
            D = FFT(d);
            E = FFT(e);
            for (k = 0; k < N / 2; k++)
            {
                Complex temp = Complex.from_polar(1, -2 * Math.PI * k / N);
                D[k] *= temp;
            }
            for (k = 0; k < N / 2; k++)
            {
                X[k] = E[k] + D[k];
                X[k + N / 2] = E[k] - D[k];
            }
            return X;
        }
    }
}
