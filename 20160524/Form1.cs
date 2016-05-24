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
    public partial class Form1 : Form
    {
        public string fileName, safefileName; //他のクラスでも使用
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 使用するファイルを選択する（ファイルを開く）ためのダイアログボックスをを利用するには
            // OpenFileDialog コンポ―ネントを利用する

            // OpenFileDialog の新しいインスタンスを生成する
            //
            OpenFileDialog ofp = new OpenFileDialog();
            DialogResult dr;        // OpenfileDialog の結果を dr に格納
            dr = ofp.ShowDialog(this);

            fileName = ofp.FileName;

            safefileName = ofp.SafeFileName;

            if (dr == DialogResult.OK)
            {
                label1.Text = safefileName;
                 // label1.Text = fileName; //  show file name with path
            }

            // Egaku Egaku = new Egaku(this);
            // Egaku.show();
        }
    }
}
