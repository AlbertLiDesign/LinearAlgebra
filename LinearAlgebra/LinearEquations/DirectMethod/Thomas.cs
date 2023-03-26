using System;

namespace LinearAlgebra
{
    /// <summary>
    /// 三对角矩阵的追赶法
    /// </summary>
    public class Thomas
    {
        /// <summary>
        /// 用追赶法分解三对角矩阵，其中low为下次对角线、diag为对角线、up为上次对角线；
        /// 返回L的下次对角线以及U的对角线，满足L*U=A，且L为单位下三角矩阵；
        /// U的上次对角线与原来相同，即为up；
        /// 当存在顺序主子式为0时返回null
        /// </summary>
        /// <param name="low"></param>
        /// <param name="diag"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Tuple<Vector, Vector> Factorize(Vector low, Vector diag, Vector up)
        {
            if (low.Length != diag.Length - 1 || up.Length != diag.Length - 1)
                throw new Exception("次对角线元素个数不等于对角线元素个数-1，无法分解！");

            Vector lowL = new Vector(low.Length);
            Vector diagU = new Vector(diag.Length);

            diagU[0] = diag[0];
            if (diagU[0] == 0)
                return null;
            for (int i = 1; i < diagU.Length; i++)
            {
                lowL[i - 1] = low[i - 1] / diagU[i - 1];
                diagU[i] = diag[i] - lowL[i - 1] * up[i - 1];
                if (diagU[i] == 0)
                    return null;
            }

            return new Tuple<Vector, Vector>(lowL, diagU);
        }

        /// <summary>
        /// 用追赶法求解Ax=b，其中A为三对角矩阵，low为下次对角线、diag为对角线、up为上次对角线；
        /// 当存在顺序主子式为0时返回null
        /// </summary>
        /// <param name="low"></param>
        /// <param name="diag"></param>
        /// <param name="up"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Vector Solve(Vector low, Vector diag, Vector up, Vector b)
        {
            if (diag.Length != b.Length)
                throw new Exception("对角线元素个数与b元素个数不同，无法进行求解！");
            if (low.Length != diag.Length - 1 || up.Length != diag.Length - 1)
                throw new Exception("次对角线元素个数不等于对角线元素个数-1，无法求解！");

            var LU = Thomas.Factorize(low, diag, up);
            if (LU is null)
                return null;

            var lowL = LU.Item1;
            var diagU = LU.Item2;

            Vector y = new Vector(b.Length);
            y[0] = b[0];
            for (int i = 1; i < diag.Length; i++)
            {
                y[i] = b[i] - lowL[i - 1] * y[i - 1];
            }

            Vector x = new Vector(b.Length);
            x[x.Length - 1] = y[x.Length - 1] / diagU[x.Length - 1];
            for (int i = x.Length - 2; i >= 0; i--)
            {
                x[i] = (y[i] - up[i] * x[i + 1]) / diagU[i];
            }
            return x;
        }
    }
}