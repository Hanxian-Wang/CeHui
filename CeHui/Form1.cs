using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CeHui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Point> data = new List<Point>();
        private void 读取文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            utils.Read(data);
            int index = 0;
            foreach (var p in data)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = p.id;
                dataGridView1.Rows[index].Cells[1].Value = p.x;
                dataGridView1.Rows[index].Cells[2].Value = p.y;
                dataGridView1.Rows[index].Cells[3].Value = p.areaCode;
                index++;
            }
            textBox1.Text = "已读取！";
            richTextBox1.Text += "读取文件成功！\t\n";
        }

        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Algo.CountDatas(data);
            Algo.CalculateCenter(data);
            Algo.CalculateSDE(data);
            Algo.CenterEach(data);
            Algo.WeightMartix(data);
            Algo.MoranIndex(data);
            for (int i = 0; i < 7; i++)
            {
                Algo.MoranIndex_(i + 1);
            }
            Algo.CalculateZScore(Algo.Ii);
            textBox2.Text = "已成功计算！";
            richTextBox1.Text += $"1, P6的坐标x: {data[5].x:F3}\t\n";
            richTextBox1.Text += $"2, P6的坐标y: {data[5].y:F3}\t\n";
            richTextBox1.Text += $"3, P6的区号: {data[5].areaCode}\t\n";
            richTextBox1.Text += $"4, 1区的事件数量n1: {Algo.counts[0]}\t\n";
            richTextBox1.Text += $"5, 4区的事件数量n1: {Algo.counts[3]}\t\n";
            richTextBox1.Text += $"6, 6区的事件数量n1: {Algo.counts[5]}\t\n";
            richTextBox1.Text += $"7, 事件总数n: {data.Count}\t\n";
            richTextBox1.Text += $"8, 坐标分量x的平均值: {Algo.meanX:F3} \t\n" +
                $"9, 坐标分量y的平均值: {Algo.meanY:F3}\t\n";
            richTextBox1.Text += $"10, P6坐标分量与平均中心之间的偏移量a6: {data[5].a:F3}\t\n" +
                $"11, P6坐标分量与平均中心之间的偏移量b6 : {data[5].b:F3} \t\n";
            richTextBox1.Text += $"12, 辅助量A: {Algo.A:F3}\t\n";
            richTextBox1.Text += $"13, 辅助量B: {Algo.B:F3}\t\n";
            richTextBox1.Text += $"14, 辅助量C: {Algo.C:F3}\t\n";
            richTextBox1.Text += $"15, 标准差椭圆长轴与竖直方向的夹角: {Algo.sigma:F3}\t\n";
            richTextBox1.Text += $"16, SDEx: {Algo.SDEx:F3}\t\n";
            richTextBox1.Text += $"17, SDEy: {Algo.SDEy:F3}\t\n";
            richTextBox1.Text += $"18, 1区的平均中心: {Algo.meanX_[0]:F3}, {Algo.meanY_[0]:F3}\t\n";
            richTextBox1.Text += $"19, 4区的平均中心: {Algo.meanX_[3]:F3}, {Algo.meanY_[3]:F3}\t\n";
            richTextBox1.Text += $"20, 1区和4区的权重矩阵: {Algo.weightMartix[0, 3]:F6} \t\n";
            richTextBox1.Text += $"21, 6区和7区的权重矩阵: {Algo.weightMartix[5, 6]:F6} \t\n";
            richTextBox1.Text += $"22, 研究区域犯罪事件的平均值: {Algo.meanX_Moran:F6}\t\n";
            richTextBox1.Text += $"23, 全局莫兰指数辅助量S0: {Algo.S0:F6}\t\n";
            richTextBox1.Text += $"24, 全局莫兰指数: {Algo.I:F6}\t\n";
            richTextBox1.Text += $"25, 1区局部莫兰指数: {Algo.Ii[0]:F6}\t\n";
            richTextBox1.Text += $"26, 3区局部莫兰指数: {Algo.Ii[2]:F6}\t\n";
            richTextBox1.Text += $"27, 5区局部莫兰指数: {Algo.Ii[4]:F6}\t\n";
            richTextBox1.Text += $"28, 7区局部莫兰指数: {Algo.Ii[6]:F6}\t\n";
            richTextBox1.Text += $"29, 局部莫兰指数的平均数μ: {Algo.u:F6} \t\n";
            richTextBox1.Text += $"30, 局部莫兰指数的平均数σ: {Algo.o:F6} \t\n";
            richTextBox1.Text += $"31, 1区局部莫兰指数Z: {Algo.Zi[0]:F6}\t\n";
            richTextBox1.Text += $"32, 3区局部莫兰指数Z: {Algo.Zi[2]:F6}\t\n";
            richTextBox1.Text += $"33, 4区局部莫兰指数Z: {Algo.Zi[4]:F6}\t\n";
            richTextBox1.Text += $"34, 7区局部莫兰指数Z: {Algo.Zi[6]:F6}\t\n";
        }

        private void 保存文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            utils.Save(richTextBox1.Text);
            richTextBox1.Text += "保存文件成功！\t\n";
        }
    }
}
