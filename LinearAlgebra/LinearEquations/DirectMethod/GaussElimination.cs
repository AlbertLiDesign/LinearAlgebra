using System;

namespace LinearAlgebra
{
    /// <summary>
    /// 高斯消元法
    /// </summary>
    public class GaussElimination
	{
        /// <summary>
        /// 返回矩阵A经过列主元高斯消元后形成的上三角阵；
        /// 如果矩阵A前RowCount列组成的矩阵奇异，那么返回null
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static Matrix ToUpper(Matrix A)
        {
            return ToUpper(A, out int _);
        }

        /// <summary>
        /// 返回矩阵A经过列主元高斯消元后形成的上三角阵；
        /// 如果矩阵A前RowCount列组成的矩阵奇异，那么返回null;
        /// swapCount将被赋值为列主元消去时交换两行的次数
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static Matrix ToUpper(Matrix A, out int swapCount)
        {
            // 先拷贝一下避免修改原矩阵
            Matrix U = A.Copy();

            swapCount = 0;
            // 消元执行到倒数第二行
            for (int i = 0; i < U.RowCount - 1; i++)
            {
                // 获取列主元所在行
                int pivot = GetColumnPivot(U, i, i);

                // 如果对角线有0，主元返回-1，说明矩阵奇异
                if (pivot == -1)
                    return null;

                // 如果pivot != i，说明需要交换，该行对角线元素不是最大列主元
                if (pivot != i)
                {
                    // 交换列主元所在行与当前行
                    SwapRow(U, i, pivot);
                    swapCount++;
                }

                // 取出第i行
                var rowVec = U.GetRow(i);

                // 用来消元的行为第i行的下一行
                for (int j = i + 1; j < U.RowCount; j++)
                {
                    // 消去的倍数为第j行的元素除上一行对应的元素
                    double factor = U[j, i] / U[i, i];

                    // 修改U的第j行
                    U.SetRow(j, U.GetRow(j) - factor * rowVec);
                }
            }
            return U;
        }

        /// <summary>
        /// 返回矩阵colDex列上从beginRowDex开始的绝对值最大元素的所在行；
        /// 如果查找的元素均为0，则返回-1
        /// </summary>
        /// <param name="m"></param>
        /// <param name="colDex"></param>
        /// <param name="beginRowDex"></param>
        /// <returns></returns>
        public static int GetColumnPivot(Matrix m, int colDex, int beginRowDex)
        {
            int pivot = -1;
            double max = 0;

            for (int i = beginRowDex; i < m.RowCount; i++)
            {
                if (Math.Abs(m[i,colDex])>max)
                {
                    pivot = i;
                    max = Math.Abs(m[i, colDex]);
                }
            }
            return pivot;
        }

        /// <summary>
        /// 交换矩阵的两行，会改变原矩阵
        /// </summary>
        /// <param name="m"></param>
        /// <param name="rowDex1"></param>
        /// <param name="rowDex2"></param>
        public static void SwapRow(Matrix m, int rowDex1, int rowDex2)
        {
            if (rowDex1 == rowDex2)
                return;
            var row1 = m.GetRow(rowDex1);
            m.SetRow(rowDex1, m.GetRow(rowDex2));
            m.SetRow(rowDex2, row1);
        }


        /// <summary>
        /// 返回矩阵A的行列式值
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static double Determinant(Matrix A)
        {
            // 求行列式值要求A必须是方阵
            if (A.RowCount != A.ColumnCount)
                throw new Exception("A不是方阵，无法求行列式值！");

            // 先进性列主元消元得到上三角矩阵U
            Matrix U = ToUpper(A, out int swapCount);

            // 如果U是奇异矩阵，行列式为0
            if (U is null)
                return 0;

            // 计算行列式为上三角矩阵的对角线元素相乘
            double product = 1;
            for (int i = 0; i < A.RowCount; i++)
            {
                product *= U[i, i];
            }

            // 行列式的值与行交换次数有关，偶数次时不变，奇数次时为相反数
            return product % 2 == 0 ? product : -product;
        }

        /// <summary>
        /// 返回A的逆矩阵；
        /// 当A奇异时返回null
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Matrix Inverse(Matrix A)
        {
            // 求逆矩阵要求A必须是方阵
            if (A.RowCount != A.ColumnCount)
                throw new Exception("A不是方阵，无法求逆矩阵！");

            // 先把A和单位阵拼接
            var AI = A.Concat(Matrix.Identity(A.RowCount));
            // 再计算上三角矩阵
            AI = ToUpper(AI);
            if (AI is null)
                return null;

            // 然后做一个高斯消元
            for (int i = AI.RowCount-1; i >= 0; i--)
            {
                var rowVec = AI.GetRow(i);
                rowVec /= rowVec[i];
                AI.SetRow(i, rowVec);

                // 让前面的每一行减去某个倍数的rowVec
                for (int j = 0; j < i; j++)
                {
                    // 把右上角变成0
                    AI.SetRow(j, AI.GetRow(j) - AI[j, i] * rowVec);
                }
            }

            return AI.SubColumns(A.ColumnCount, AI.ColumnCount);
        }

        /// <summary>
        /// 使用列主元高斯消元法求解Ax=b
        /// 如果矩阵A奇异，则返回null
        /// </summary>
        /// <param name="A"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Vector Solve(Matrix A, Vector b)
        {
            // 高斯消元要求A必须是方阵
            if (A.RowCount!=A.ColumnCount)
                throw new Exception("A不是方阵，无法求解Ax=b！");
            if (A.RowCount != b.Length)
                throw new Exception("A的行数与b的元素个数不同，无法求解Ax=b！");

            // 高斯消元要先把A和b组成一个增广矩阵
            // b需要变成矩阵（行向量）再转置成列向量
            var Ab = A.Concat(new Matrix(b).Transpose());

            // 消元
            Ab = ToUpper(Ab);

            // 说明方程无解
            if (Ab is null)
                return null;

            //把系数矩阵分离出来
            // 取Ab的前n列（A的列数）
            var U = Ab.SubColumns(0, A.ColumnCount);

            // 取Ab的最后一列作为右侧向量
            var Ub = Ab.GetColumn(A.ColumnCount);

            // 创建x向量
            var x = new Vector(A.RowCount);

            // x的求解
            //例：x2 = (b2 - row2*x) / a22
            for (int i = x.Length-1; i >= 0; i--)
            {
                x[i] = (Ub[i] - U.GetRow(i) * x) / U[i, i];
            }
            return x;
        }
    }
}

