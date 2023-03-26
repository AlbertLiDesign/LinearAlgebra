namespace LinearAlgebra
{
    /// <summary>
    /// 矩阵的条件数，用于求矩阵的常用条件数，条件数很大时说明矩阵是病态的
    /// 条件数越大，解就与b的值越敏感。b的微小扰动对结果的影响就越大
    /// </summary>
    public class ConditionNumber
    {
        /// <summary>
        /// 返回矩阵m的1-条件数
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double One(Matrix m)
        {
            // 矩阵的一条数就是算m和m^(-1)的1-范数的积
            Matrix mInv = m.Inverse();
            // 如果矩阵奇异，没有逆，则1-条件数无穷大
            if (mInv is null)
                return double.PositiveInfinity;
            return Norm.One(m) * Norm.One(mInv);
        }

        /// <summary>
        /// 返回矩阵m的∞-条件数
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double Infinity(Matrix m)
        {
            // 矩阵的一条数就是算m和m^(-1)的无穷范数的积
            Matrix mInv = m.Inverse();
            // 如果矩阵奇异，没有逆，则无穷条件数无穷大
            if (mInv is null)
                return double.PositiveInfinity;
            return Norm.Infinity(m) * Norm.Infinity(mInv);
        }
    }
}