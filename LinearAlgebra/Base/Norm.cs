namespace LinearAlgebra
{
    /// <summary>
    /// 范数
    /// </summary>
    public class Norm
    {
        /// <summary>
        /// 返回向量v的1-范数
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double One(Vector v)
        {
            // 把向量中每个数的绝对值加起来
            double sum = 0;
            for (int i = 0; i < v.Length; i++)
            {
                sum += Math.Abs(v[i]);
            }
            return sum;
        }

        /// <summary>
        /// 返回向量v的2-范数
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double Two(Vector v)
        {
            // 把向量每个数的平方加起来在开根号，即向量长度
            double sum = 0;
            for (int i = 0; i < v.Length; i++)
            {
                sum += v[i] * v[i];
            }
            return Math.Sqrt(sum);
        }

        /// <summary>
        /// 返回向量v的∞-范数
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double Infinity(Vector v)
        {
            // 无穷范数就是向量元素里绝对值最大的那个值
            double max = 0;
            for (int i = 0; i < v.Length; i++)
            {
                if (max < Math.Abs(v[i]))
                    max = Math.Abs(v[i]);
            }
            return max;
        }

        /// <summary>
        /// 返回矩阵m的1-范数
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double One(Matrix m)
        {
            // 求每一列向量的1-范数然后找出最大的那个就是矩阵的1-范数
            double max = 0;
            for (int i = 0; i < m.ColumnCount; i++)
            {
                double x = Norm.One(m.GetColumn(i));
                if (x > max)
                    max = x;
            }
            return max;
        }

        /// <summary>
        /// 返回矩阵m的∞-范数
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double Infinity(Matrix m)
        {
            // 求每一行向量的1-范数然后找出最大的那个就是矩阵的无穷范数
            double max = 0;
            for (int i = 0; i < m.RowCount; i++)
            {
                double x = Norm.One(m.GetRow(i));
                if (x > max)
                    max = x;
            }
            return max;
        }

        /// <summary>
        /// 返回矩阵的Frobenius范数
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double Frobenius(Matrix m)
        {
            // 把矩阵摊平成一个向量，然后它的二范数就是矩阵的F范数
            double sum = 0;
            for (int i = 0; i < m.RowCount; i++)
            {
                for (int j = 0; j < m.ColumnCount; j++)
                {
                    sum += m[i, j] * m[i, j];
                }
            }
            return Math.Sqrt(sum);
        }
    }
}