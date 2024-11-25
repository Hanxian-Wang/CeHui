using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeHui
{
    /// <summary>
    /// 算法类
    /// </summary>
    public class Algo
    {
        #region 数据存放
        //各区的事件数量
        public static List<int> counts = new List<int>()
        {
            0, 0, 0, 0, 0, 0, 0
        };
        //平均中心坐标
        public static double meanX = 0, meanY = 0;
        //标准差椭圆参数
        public static double sigma, SDEx, SDEy;
        public static double A, B, C;
        //各区的平均中心
        public static List<double> meanX_ = new List<double>()
        {
            0, 0, 0, 0, 0, 0, 0
        };
        public static List<double> meanY_ = new List<double>()
        {
            0, 0, 0, 0, 0, 0, 0
        };
        //空间权重矩阵
        public static double[,] weightMartix;

        //莫兰指数
        public static double meanX_Moran = 0;
        //辅助量S0, Si
        public static double S0 = 0, Si = 0;
        //全局和局部
        public static double I = 0;
        public static double[] Ii = new double[7];
        public static double u;
        public static double o;
        public static double[] Zi = new double[7];
        #endregion

        #region 标准差椭圆
        /// <summary>
        /// 数据统计 统计每个区的事件数量 共七个区
        /// </summary>
        /// <param name="data"></param>
        public static void CountDatas(List<Point> data)
        {
            foreach(var p in data)
            {
                var code = p.areaCode;
                switch (code)
                {
                    case 1:
                        counts[0]++;
                        break;
                    case 2:
                        counts[1]++;
                        break;
                    case 3:
                        counts[2]++;
                        break;
                    case 4:
                        counts[3]++;
                        break;
                    case 5:
                        counts[4]++;
                        break;
                    case 6:
                        counts[5]++;
                        break;
                    case 7:
                        counts[6]++;
                        break;
                }
            }
        }
        /// <summary>
        /// 计算平均中心
        /// </summary>
        /// <param name="data"></param>
        public static void CalculateCenter(List<Point> data)
        {
            meanX = data.Average(p => p.x);
            meanY = data.Average(p => p.y);
        }
        /// <summary>
        /// 计算标准差椭圆
        /// </summary>
        /// <param name="data"></param>
        public static void CalculateSDE(List<Point> data)
        {
            var num = data.Count;
            double a2 = 0, b2 = 0, ab = 0;
            foreach(var p in data)
            {
                p.a = p.x - meanX;
                p.b = p.y - meanY;
                a2 += p.a * p.a;
                b2 += p.b * p.b;
                ab += p.a * p.b;
            }
            A = a2 - b2;
            B = Math.Sqrt(Math.Pow(a2 - b2, 2) + 4 * Math.Pow(ab, 2));
            C = 2 * ab;
            sigma = Math.Atan((A + B) / C);
            double SDExOver = 0, SDEyOver = 0;
            for (int i = 0; i < num; i++)
            {
                SDExOver += Math.Pow(data[i].a * Math.Cos(sigma) + data[i].b * Math.Sin(sigma), 2);
                SDEyOver += Math.Pow(data[i].a * Math.Sin(sigma) - data[i].b * Math.Cos(sigma), 2);
            }
            SDEx = Math.Sqrt(2 * SDExOver / num);
            SDEy = Math.Sqrt(2 * SDEyOver / num);
        }
        #endregion
        /// <summary>
        /// 计算各区的平均中心
        /// </summary>
        /// <param name="data"></param>
        public static void CenterEach(List<Point> data)
        {
            List<double> tempCountX = new List<double>()
            {
                0, 0, 0, 0, 0, 0, 0
            };
            List<double> tempCountY = new List<double>()
            {
                0, 0, 0, 0, 0, 0, 0
            };
            //计算每个区的x, y各和
            foreach (var p in data)
            {
                var code = p.areaCode;
             switch (code)
                {
                    case 1:
                        tempCountX[0] += p.x;
                        tempCountY[0] += p.y;
                        break;
                    case 2:
                        tempCountX[1] += p.x;
                        tempCountY[1] += p.y;
                        break;
                    case 3:
                        tempCountX[2] += p.x;
                        tempCountY[2] += p.y;
                        break;
                    case 4:
                        tempCountX[3] += p.x;
                        tempCountY[3] += p.y;
                        break;
                    case 5:
                        tempCountX[4] += p.x;
                        tempCountY[4] += p.y;
                        break;
                    case 6:
                        tempCountX[5] += p.x;
                        tempCountY[5] += p.y;
                        break;
                    case 7:
                        tempCountX[6] += p.x;
                        tempCountY[6] += p.y;
                        break;
                }
            }
            //计算每个区的平均中心
            for (int i = 0; i < 7; i++)
            {
                meanX_[i] = tempCountX[i] / counts[i];
                meanY_[i] = tempCountY[i] / counts[i];
            }
        }
        /// <summary>
        /// 计算各区的空间权重矩阵
        /// </summary>
        /// <param name="data"></param>
        public static void WeightMartix(List<Point> data)
        {
            weightMartix = new double[7, 7];
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (i == j)
                    {
                        weightMartix[i, j] = 0.0;
                        continue;
                    }
                    var dx = meanX_[i] - meanX_[j];
                    var dy = meanY_[i] - meanY_[j];
                    weightMartix[i, j] = 1000 / Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                }
            }

        }
        /// <summary>
        /// 莫兰指数计算
        /// </summary>
        /// <param name="data"></param>
        public static void MoranIndex(List<Point> data)
        {
            int N = 7;
            //研究区域犯罪事件的平均值
            meanX_Moran = (double)data.Count / N;

            double over = 0, under = 0;
            for (int i = 0; i < N; i++)
            {
                under += Math.Pow(counts[i] - meanX_Moran, 2);
                for (int j = 0; j < N; j++)
                {
                    S0 += weightMartix[i, j];
                    over += weightMartix[i, j] * (counts[i] - meanX_Moran) * (counts[j] - meanX_Moran);
                }
            }
            I = (N / S0) * (over / under);
        }
        /// <summary>
        /// 局部莫兰指数计算
        /// </summary>
        /// <param name="code"></param>
        public static void MoranIndex_(int code)
        {
            //局部莫兰指数的计算
            int N = 7;
            //传入区域号为code 即为i = code-1
            double over2 = 0, right = 0;
            for (int j = 0; j < N; j++)
            {
                //j!=i
                if (j == (code - 1))
                {
                    continue;
                }
                else
                {
                    over2 += Math.Pow(counts[j] - meanX_Moran, 2);
                    right += weightMartix[code - 1, j] * (counts[j] - meanX_Moran);
                }

            }
            Si = over2 / (N - 1);
            Ii[code - 1] = right * (counts[code - 1] - meanX_Moran) / Si / 1000;
        }
        /// <summary>
        /// 计算局部莫兰指数的Z得分
        /// </summary>
        /// <param name="Ii"></param>
        public static void CalculateZScore(double[] I)
        {
            int N = 7;
            double over = 0;
            double over2 = 0;
            foreach(var i in I)
            {
                over += i;
            }
            u = over / N;
            foreach(var i in I)
            {
                over2 += Math.Pow(i - u, 2);
            }
            o = Math.Sqrt(over2 / (N - 1));
            for (int i = 0; i < N; i++)
            {
                Zi[i] = (I[i] - u) / o;
            }
        }
    }
}
