using System;

namespace LinearAlgebra
{
	/// <summary>
	/// LDLT分解，适用于对称矩阵
	/// </summary>
	public class LDLT
	{
		/// <summary>
		/// 对对称矩阵A进行LDLT分解，返回L矩阵和D矩阵(L,D)，其中L是单位下三角矩阵，D为对角阵
		/// 分解过程中仅用到A的下三角元素，当存在顺序主子式为0时，返回null
		/// </summary>
		/// <param name="A"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static Tuple<Matrix, Matrix> Factorize(Matrix A)
		{
            // LDLT分解要求A必须是方阵
            if (A.RowCount != A.ColumnCount)
                throw new Exception("A不是方阵，无法执行LDLT分解！");

            Matrix L = Matrix.Identity(A.RowCount);
			Vector D = new Vector(A.RowCount);

			for (int i = 0; i < A.RowCount; i++)
			{
				// 求解L[i,j]
				for (int j = 0; j < i; j++)
				{
					// A[i,j]是A的下三角部分
					// 直接使用矩阵乘向量，对应元素相乘能快一点，避免矩阵乘矩阵的复杂度
                    L[i, j] = (A[i, j] - L.GetRow(i).Multiply(D) * L.GetRow(j)) / D[j];
				}

				// 求解D[i,i]
				// A[i,i]是A的对角线部分
				D[i] = A[i, i] - L.GetRow(i).Multiply(D) * L.GetRow(i);

				// 如果对角线有值为0直接返回
				if (D[i]==0)
				{
					return null;
				}
			}

			return new Tuple<Matrix, Matrix>(L,Matrix.Diagonal(D));
		}

        /// <summary>
        /// 使用LDLT分解法求解Ax=b，其中A是对称矩阵
        /// 如果矩阵A存在顺序主子式为0时，返回null
        /// </summary>
        /// <param name="A"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Vector Solve(Matrix A, Vector b)
		{
            // LDLT求解要求A必须是方阵
            if (A.RowCount != A.ColumnCount)
                throw new Exception("A不是方阵，无法求解Ax=b！");
            if (A.RowCount != b.Length)
                throw new Exception("A的行数与b的元素个数不同，无法求解Ax=b！");

            var LD = LDLT.Factorize(A);
			if (LD is null)
			{
				return null;
			}

            var L = LD.Item1;
            var D = LD.Item2;
			var LT = L.Transpose();

			// 目标求解L*D*LT*x=b
			// 先求解L*y=b
			Vector y = new Vector(b.Length);
			for (int i = 0; i < A.RowCount; i++)
			{
				y[i] = b[i] - L.GetRow(i) * y;
			}

			// D*z=y
			y = y.Divide(D.GetDiagonal());

			// LT*x=y
			Vector x = new Vector(b.Length);
			for (int i = A.RowCount-1; i >=0;i--)
			{
				x[i] = y[i] - LT.GetRow(i) * x;
			}
			return x;
        }
	}
}

