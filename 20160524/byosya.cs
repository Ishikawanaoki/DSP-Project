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
    /// <summary>
    /// 
    /// </summary>
    public partial class byosya : Form //フィールド
    {
        private const int CNSTMAX = 256;
        private double[] y = new double[CNSTMAX];
        double[] y_out = new double[CNSTMAX];
        int[] y2 = new int[CNSTMAX];
        private int[] x2 = new int[CNSTMAX];
        private int chart_id = 0;
        private string chart_name;
        String fileout = @"C:\Users\チャリンコ\Desktop\kapuro\koeout.txt";
        string filename = @"C:\Users\チャリンコ\Desktop\kapuro";


        /// <summary>
        /// byosyaのコンストラクタです。
        /// </summary>
        public byosya()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        public byosya(double[] y)
        {
            this.y = y;
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart1_Click(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// DFTを実行するテストクラスです。
        /// </summary>
        private void test() //本体
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


        /// <summary>
        /// FFTを実行するテストクラスです。
        /// </summary>
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            test();
            Plot("dft-100Hz-2KAD");
            label();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            test2();
            Plot("fft-100Hz-2KAD");
            label();
        }

        /// <summary>
        /// 
        /// </summary>
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
            s += ((double)(max_num*3.9)).ToString();
            label1.Text = s;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
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
            
            // 初期化処理

            String ChartName = str;
            string[] xValues = new string[x2.Length];

            chart1.Series.Clear();  //グラフ初期化

            chart1.Series.Add(ChartName);

            chart1.Series[ChartName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            x2[0] = x2.Length / 2 * 4; // x[0] はグラフ描写の開始点


            for (int i = 0; i < x2.Length; i++)
            {
                if(i+1 < x2.Length) x2[i+1] = x2[i] - 4;
                //x2[i] = i * 4;
                xValues[i] = x2[i].ToString();
                //グラフに追加するデータクラスを生成
                DataPoint dp = new DataPoint();
                dp.SetValueXY(xValues[i], y2[i]);  //XとYの値を設定
                dp.IsValueShownAsLabel = false;  //グラフに値を表示するように指定
                chart1.Series[ChartName].Points.Add(dp);   //グラフにデータ追加
            }

            chart_name += chart_id.ToString();
            chart_id++;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            filename += chart_name;
            filename += ".png";
            chart1.SaveImage(filename , System.Drawing.Imaging.ImageFormat.Png);
            chart1.Dispose();
            System.Diagnostics.Process p = System.Diagnostics.Process.Start(filename);
            button1.Enabled = false;
        }
    }


    /// <summary>
    /// なんか難しい構造体です。
    /// フーリエ変換後の値を格納します。
    /// </summary>
    internal class Complex
    {
        public double real = 0.0;
        public double imag = 0.0;


        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public Complex()
        {
        }

        /// <summary>
        /// フィールドです。
        /// </summary>
        /// <param name="real">実部です。</param>
        /// <param name="img">虚部です。</param>
        public Complex(double real, double img)
        {
            this.real = real;
            this.imag = img;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        override public string ToString()
        {
            string data = real.ToString() + " " + imag.ToString() + "i";
            return data;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static Complex from_polar(double r, double radians)
        {
            Complex data = new Complex(r * Math.Cos(radians), r * Math.Sin(radians));
            return data;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Complex operator +(Complex a, Complex b)
        {
            Complex data = new Complex(a.real + b.real, a.imag + b.imag);
            return data;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Complex operator -(Complex a, Complex b)
        {
            Complex data = new Complex(a.real - b.real, a.imag - b.imag);
            return data;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Complex operator *(Complex a, Complex b)
        {
            Complex data = new Complex((a.real * b.real) - (a.imag * b.imag),
           (a.real * b.imag + (a.imag * b.real)));
            return data;
        }


        /// <summary>
        /// 
        /// </summary>
        public double magnitude
        {
            get
            {
                return Math.Sqrt(Math.Pow(real, 2) + Math.Pow(imag, 2));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double phase
        {
            get
            {
                return Math.Atan(imag / real); // アークタンジェントを返し、-n/2<=theta<=n/2となる値を返す
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    internal class Fourier
    {

        /// <summary>
        /// 
        /// </summary>
        public enum WindowFunc
        {
            Hamming,
            Hanning,
            Blackman,
            Rectangular
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Complex[] HannWindow(Complex[] x)
        {
            int N = x.Length;

            return x; //Disenable
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="windowFunc"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
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
