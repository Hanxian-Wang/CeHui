using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CeHui
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class utils
    {
        /// <summary>
        /// 读取文件函数
        /// </summary>
        /// <param name="data">读取数据到数据集</param>
        public static void Read(List<Point> data)
        {
            var open = new OpenFileDialog();
            open.Filter = "文本文件|*.txt";
            if(open.ShowDialog() == DialogResult.OK)
            {
                using(var sr = new StreamReader(open.FileName))
                {
                    //第一行跳过
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine().Split(',');
                        var p = new Point();
                        p.id = line[0];
                        p.x = Convert.ToDouble(line[1]);
                        p.y = Convert.ToDouble(line[2]);
                        p.areaCode = Convert.ToInt16(line[3]);
                        data.Add(p);
                    }
                }
            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="str"></param>
        public static void Save(string str)
        {
            var save = new SaveFileDialog();
            save.Filter = "文本文件|*.txt";
            save.FileName = "result";
            if(save.ShowDialog() == DialogResult.OK)
            {
                var sw = new StreamWriter(save.FileName);
                sw.Write(str);
                sw.Flush();
            }
        }
    }
}
