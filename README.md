# 空间数据探索性分析

### 本项目为自己在2024年参加的测绘程序设计大赛的比赛代码，题目为空间数据探索性分析，为特等奖

### 测绘程序大赛基本就是 [文件读取](https://www.runoob.com/csharp/csharp-text-files.html)+基于已有算法转化为代码 实现两大点

 
### 具体算法实现

为每个点计算中心坐标
````
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
````

计算各区的空间矩阵

````
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
````

莫兰指数方面的计算

````
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
````