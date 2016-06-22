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
    public enum WindowFunc
    {
        Hamming,
        Hanning,
        Blackman,
        Rectangular,
        None
    }
    public enum FTFunc
    {
        DFT,
        FFT
    }

    public partial class byosya : Form
    {
        readonly private int CNSTMAX;
        readonly private double[] y;
        readonly private string filename;
        double[] y_out;
        int[] y2;
        private double[] x2;
        // 入力データの登録
        // 現在の登録数 : 3
        // Dictionaryの初期化
        readonly static Dictionary<string, int> datamap = new Dictionary<string, int>()
            {
                { "100Hz-2KAD.txt", 2000 },
                { "MAN01.KOE", 10000 },
                { "WOMAN01.KOE", 10000 },
            };
        public byosya()
        {
            InitializeComponent();
        }
        public byosya(int cnstmax,double[] y, string filename)
        {
            this.CNSTMAX = cnstmax;
            this.y = y;
            y_out = new double[CNSTMAX];
            y2 = new int[CNSTMAX];
            x2 = new double[CNSTMAX/2];
            this.filename = filename;
            InitializeComponent();
        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }
        private void test()
        {
            WindowFunc windowfunc = WindowFunc.None;
            foreach (RadioButton rdo in groupBox1.Controls)
            {
                if (rdo.Checked)
                {
                    String s = rdo.Text;
                    windowfunc = (WindowFunc)Enum.Parse(typeof(WindowFunc), s);
                }
            }
            y_out = Fourier.Execute(y, FTFunc.DFT, windowfunc);
            for (int i =0; i < CNSTMAX; i++)
            {
                y2[i] = Convert.ToInt32(y_out[i]);
            }

            // 結果をファイル出力する
            String fileout = @"C:\Users\N.Ishikawa\Desktop\data\dft_out.txt";
            String yout;
            System.IO.StreamWriter kekkaout = new System.IO.StreamWriter(fileout);
            for (int iii = 0; iii < CNSTMAX; iii++)
            {
                yout = y2[iii].ToString("D10");
                kekkaout.WriteLine(yout);
            }
            kekkaout.Close();
        }
        private void test2()
        {
            WindowFunc windowfunc = WindowFunc.None;
            foreach (RadioButton rdo in groupBox1.Controls)
            {
                if (rdo.Checked)
                {
                    String s = rdo.Text;
                    windowfunc = (WindowFunc)Enum.Parse(typeof(WindowFunc), s);
                }
            }
            y_out = Fourier.Execute(y, FTFunc.DFT, windowfunc);
            for (int i = 0; i < CNSTMAX; i++)
            {
                y2[i] = Convert.ToInt32(y_out[i]);
            }

            // 結果をファイル出力する
            String fileout = @"C:\Users\N.Ishikawa\Desktop\data\fft_out.txt";
            String yout;
            System.IO.StreamWriter kekkaout = new System.IO.StreamWriter(fileout);
            for (int iii = 0; iii < CNSTMAX; iii++)
            {
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
            // 全てに初期化を実行すること
            chart1.Titles.Clear();
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            Series seriesLine = new Series();
            seriesLine.ChartType = SeriesChartType.Line; // 折れ線グラフ
            seriesLine.LegendText = "Legend:Line";       // 凡例

            chart1.ChartAreas.Add(new ChartArea("Area1"));            // ChartArea作成
            chart1.ChartAreas["Area1"].AxisX.Title = "周波数 f [Hz]";  // X軸タイトル設定
            chart1.ChartAreas["Area1"].AxisY.Title = "周波数スペクトル |F(f)| [/]";  // Y軸タイトル設定
            
            // 以上、初期化処理

            String ChartName = str;
            string[] xValues = new string[(y.Length/2)];
           
            
            chart1.Series.Add(ChartName);
            chart1.Series[ChartName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            int rate = 1000;
            if (datamap.ContainsKey(filename))
            {
                rate = datamap[filename];
                Console.WriteLine("sycceed, {0}", filename);
            }

            Axis plot_axis = new Axis(CNSTMAX, rate);
            plot_axis.stringAxie(ref xValues);
            
            for (int i = 0; i < xValues.Length; i++)
            {
                //グラフに追加するデータクラスを生成
                DataPoint dp = new DataPoint();
                //dp.SetValueXY(xValues[i], y2[]);  //XとYの値を設定
                dp.SetValueXY(xValues[i], y2[i]);
                dp.IsValueShownAsLabel = false;  //グラフに値を表示するように指定
                chart1.Series[ChartName].Points.Add(dp);   //グラフにデータ追加
            }
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

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            WindowFunc windowfunc = WindowFunc.None;
            foreach (RadioButton rdo in groupBox1.Controls)
            {
                if (rdo.Checked)
                {
                    String s = rdo.Text;
                    windowfunc = (WindowFunc)Enum.Parse(typeof(WindowFunc), s);
                }
            }
            double[] y_window = Fourier.Windowing(y, windowfunc);
            int[] y2_window = new int[y_window.Length];
            //y2_window.CopyTo(y_window, 0);
            for (int i = 0; i < y2_window.Length; i++)
                y2_window[i] = (int)y_window[i];
            DataViewer view = new DataViewer(y2_window);
            view.Show();

            // 結果をファイル出力する
            String fileout = @"C:\Users\N.Ishikawa\Desktop\data\viewer_out.txt";
            String yout;
            System.IO.StreamWriter kekkaout = new System.IO.StreamWriter(fileout);
            for (int iii = 0; iii < CNSTMAX; iii++)
            {
                yout = y2_window[iii].ToString("D10");
                kekkaout.WriteLine(yout);
            }
            kekkaout.Close();
        }
    }
    class Axis
    {
        //double[] row_sign;
        private double time; // 時間軸領域の目盛り
        private double frequency; // 周波数領域の目盛り
        public Axis()
        {
        }
        public Axis(int sample_value, int sampling_frequency)
        {
            //row_sign = y;
            time = 1 / sampling_frequency;
            frequency = sampling_frequency / sample_value;
        }
        public void doubleAxie(ref double[] x)
        {
            x[0] = frequency;
            for (int i = 1; i < x.Length; i++)
                x[i] = x[i - 1] + frequency;
        }
        public void stringAxie(ref string[] x)
        {
            int dimF = (int)frequency;
            int[] x2 = new int[x.Length];
            x2[0] = dimF;
            x[0] = dimF.ToString();
            for (int i = 1; i < x.Length; i++) {
                x2[i] = x2[i - 1] + dimF;
                x[i] = x2[i].ToString();
            }
        }
        public void intAxis(ref int[] x)
        {
            int dimF = (int)frequency;
            x[0] = dimF;
            for(int i=1; i<x.Length; i++)
                x[i] = x[i - 1] + dimF;
        }
    }
    class Complex
    {
        private double real = 0.0;
        private double imag = 0.0;
        //Empty constructor
        public Complex()
        {
        }
        public Complex(double real, double img)
        {
            this.real = real;
            this.imag = img;
        }
        public Complex(double[] real)
        {

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
    class Fourier
    {
        
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
        public static double[] Execute(double[] y, FTFunc func, WindowFunc windwfunc)
        {
            //
            Complex[] sign = new Complex[y.Length];
            Complex[] done = new Complex[y.Length];
            // 窓関数の実行
            y = Fourier.Windowing(y, windwfunc);
            // オイラー単位円への割当
            for (int i = 0; i < y.Length; i++)
            {
                sign[i] = new Complex(y[i], 0);
            }
            // 任意の変換
            if (FTFunc.DFT == func)
                done = Fourier.DFT(sign);
            else if(FTFunc.FFT == func)
                    done = Fourier.FFT(sign);
            for(int ii=0; ii<y.Length; ii++)
            {
                y[ii] = done[ii].magnitude;
            }
            Seikika(ref y); // referencive functioon
            return y;
        }
        public static void Seikika(ref double[] y)
        {
            double max = 0;
            for(int i=0; i<y.Length; i++)
            {
                if (max < y[i]) max = y[i];
                if (y[i] < 0)
                {
                    max = 0;
                    Seikika2(ref y);
                    break;
                }
            }
            for (int ii = 0; ii < y.Length; ii++)
                y[ii] = y[ii] / max * 100;
        }
        public static void Seikika2(ref double[] y)
        {
            double max = 0;
            double min = 100;
            for (int i = 0; i < y.Length; i++)
            {
                if (max < y[i]) max = y[i];
                if (min > y[i]) min = y[i];
            }
            max += min * (-1);
            for (int ii = 0; ii < y.Length; ii++)
                y[ii] = y[ii] / max * 100;
        }
    }
}
