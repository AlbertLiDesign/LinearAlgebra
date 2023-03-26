namespace LinearAlgebra
{
    /// <summary>
    /// 矩阵
    /// </summary>
    public class Matrix
    {
        #region 保护成员
        /// <summary>
        /// 存储矩阵数据的二维数组
        /// </summary>
        protected double[,] _data;
        #endregion

        #region 公有属性
        /// <summary>
        /// 矩阵行数
        /// </summary>
        public int RowCount => _data.GetLength(0);

        /// <summary>
        /// 矩阵列数
        /// </summary>
        public int ColumnCount => _data.GetLength(1);
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造一个空矩阵
        /// </summary>
        public Matrix()
        {
            _data = new double[0, 0];
        }

        /// <summary>
        /// 构造rowCount行的方阵，默认元素均为0；
        /// fillOne为true时，元素均为1
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="fillOne"></param>
        public Matrix(int rowCount, bool fillOne = false)
            : this(rowCount, rowCount, fillOne)
        {

        }

        /// <summary>
        /// 构造rowCount行、colCount列的矩阵，默认元素均为0；
        /// fillOne为true时，元素均为1
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        /// <param name="fillOne"></param>
        public Matrix(int rowCount, int colCount, bool fillOne = false)
        {
            _data = new double[rowCount, colCount];
            if (fillOne)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        _data[i, j] = 1;
                    }
                }
            }
        }

        /// <summary>
        /// 根据二维数组构造矩阵
        /// </summary>
        /// <param name="data"></param>
        public Matrix(double[,] data)
        {
            _data = new double[data.GetLength(0), data.GetLength(1)];
            Array.Copy(data, _data, data.Length);
        }

        /// <summary>
        /// 根据一系列行向量构造新矩阵，这些向量具有相同的长度；
        /// 若要作为列向量构造，请对得到的矩阵进行转置
        /// </summary>
        /// <param name="rows"></param>
        /// <exception cref="Exception"></exception>
        public Matrix(params Vector[] rows)
        {
            int rowCount = rows.Length;
            if (rowCount == 0)
            {
                _data = new double[0, 0];
                return;
            }
            int colCount = rows[0].Length;
            for (int i = 1; i < rowCount; i++)
            {
                if (rows[i].Length != colCount)
                    throw new Exception("各行向量的长度不同，无法组成矩阵！");
            }
            _data = new double[rowCount, colCount];
            for (int i = 0; i < rowCount; i++)
            {
                rows[i].CopyTo(_data, i * colCount);
            }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 返回size行size列的单位矩阵
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Matrix Identity(int size)
        {
            Matrix m = new Matrix(size, size);
            for (int i = 0; i < size; i++)
            {
                m[i, i] = 1;
            }
            return m;
        }

        /// <summary>
        /// 返回size行size列的Hilbert矩阵
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Matrix Hilbert(int size)
        {
            Matrix m = new Matrix(size);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    m[i, j] = 1.0 / (i + j + 1);
                }
            }
            return m;
        }

        /// <summary>
        /// 根据对角线元素构造对角方阵
        /// </summary>
        /// <param name="diag"></param>
        /// <returns></returns>
        public static Matrix Diagonal(Vector diag)
        {
            Matrix m = new Matrix(diag.Length, diag.Length);
            for (int i = 0; i < diag.Length; i++)
            {
                m[i, i] = diag[i];
            }
            return m;
        }

        /// <summary>
        /// 获取矩阵中第rowDex行，第colDex列的元素
        /// </summary>
        /// <param name="rowDex"></param>
        /// <param name="colDex"></param>
        /// <returns></returns>
        public double this[int rowDex, int colDex]
        {
            get => _data[rowDex, colDex];
            set => _data[rowDex, colDex] = value;
        }

        /// <summary>
        /// 获取矩阵中的第rowDex行，作为向量返回；
        /// 对向量元素的修改不会反映到矩阵中
        /// </summary>
        /// <param name="rowDex"></param>
        /// <returns></returns>
        public Vector GetRow(int rowDex)
        {
            double[] data = new double[ColumnCount];
            Buffer.BlockCopy(_data, rowDex * ColumnCount * sizeof(double),
                data, 0, ColumnCount * sizeof(double));
            return new Vector(data);
        }

        /// <summary>
        /// 设置矩阵的第rowDex行，其中row的长度与矩阵列数相同
        /// </summary>
        /// <param name="rowDex"></param>
        /// <param name="row"></param>
        /// <exception cref="Exception"></exception>
        public void SetRow(int rowDex, Vector row)
        {
            if (row.Length != ColumnCount)
                throw new Exception("行向量长度与矩阵列数不同，不能设置！");
            row.CopyTo(_data, rowDex * ColumnCount);
        }

        /// <summary>
        /// 获取矩阵中的第colDex列，作为向量返回；
        /// 对向量元素的修改不会反映到矩阵中
        /// </summary>
        /// <param name="colDex"></param>
        /// <returns></returns>
        public Vector GetColumn(int colDex)
        {
            Vector v = new Vector(RowCount);
            for (int i = 0; i < RowCount; i++)
            {
                v[i] = this[i, colDex];
            }
            return v;
        }

        /// <summary>
        /// 设置矩阵的第colDex列，其中col的长度与矩阵行数相同
        /// </summary>
        /// <param name="colDex"></param>
        /// <param name="col"></param>
        /// <exception cref="Exception"></exception>
        public void SetColumn(int colDex, Vector col)
        {
            if (col.Length != RowCount)
                throw new Exception("列向量长度与矩阵行数不同，不能设置！");
            for (int i = 0; i < RowCount; i++)
            {
                this[i, colDex] = col[i];
            }
        }

        /// <summary>
        /// 获取矩阵对角线元素组成的向量；
        /// 返回向量的长度为行数与列数的较小值
        /// </summary>
        /// <returns></returns>
        public Vector GetDiagonal()
        {
            Vector v = new Vector(Math.Min(RowCount, ColumnCount));
            for (int i = 0; i < v.Length; i++)
            {
                v[i] = this[i, i];
            }
            return v;
        }

        /// <summary>
        /// 正号，返回m1的拷贝
        /// </summary>
        /// <param name="m1"></param>
        /// <returns></returns>
        public static Matrix operator +(Matrix m1)
        {
            return m1.Copy();
        }

        /// <summary>
        /// 负号，返回m1的相反矩阵
        /// </summary>
        /// <param name="m1"></param>
        /// <returns></returns>
        public static Matrix operator -(Matrix m1)
        {
            Matrix m = new Matrix(m1.RowCount, m1.ColumnCount);
            for (int i = 0; i < m1.RowCount; i++)
            {
                for (int j = 0; j < m1.ColumnCount; j++)
                {
                    m[i, j] = -m1[i, j];
                }
            }
            return m;
        }

        /// <summary>
        /// 矩阵加法
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.RowCount != m2.RowCount || m1.ColumnCount != m2.ColumnCount)
                throw new Exception("两矩阵形状不同，不能相加！");
            Matrix m = new Matrix(m1.RowCount, m1.ColumnCount);
            for (int i = 0; i < m1.RowCount; i++)
            {
                for (int j = 0; j < m1.ColumnCount; j++)
                {
                    m[i, j] = m1[i, j] + m2[i, j];
                }
            }
            return m;
        }

        /// <summary>
        /// 矩阵减法
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.RowCount != m2.RowCount || m1.ColumnCount != m2.ColumnCount)
                throw new Exception("两矩阵形状不同，不能相减！");
            Matrix m = new Matrix(m1.RowCount, m1.ColumnCount);
            for (int i = 0; i < m1.RowCount; i++)
            {
                for (int j = 0; j < m1.ColumnCount; j++)
                {
                    m[i, j] = m1[i, j] - m2[i, j];
                }
            }
            return m;
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            // 左矩阵的列数要等于右矩阵的行数
            if (m1.ColumnCount != m2.RowCount)
                throw new Exception("左矩阵列数与右矩阵行数不同，不能相乘！");
            Matrix m = new Matrix(m1.RowCount, m2.ColumnCount);
            // O(N^3)的算法复杂度
            for (int i = 0; i < m.RowCount; i++)
            {
                for (int j = 0; j < m.ColumnCount; j++)
                {
                    // 等价于
                    // m[i,j]=m1.GetRow(i)*m2.GetColumn(j);
                    for (int k = 0; k < m1.ColumnCount; k++)
                    {
                        m[i, j] += m1[i, k] * m2[k, j];
                    }
                }
            }
            return m;
        }

        /// <summary>
        /// 数乘矩阵
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="m1"></param>
        /// <returns></returns>
        public static Matrix operator *(double scale, Matrix m1)
        {
            Matrix m = new Matrix(m1.RowCount, m1.ColumnCount);
            for (int i = 0; i < m1.RowCount; i++)
            {
                for (int j = 0; j < m1.ColumnCount; j++)
                {
                    m[i, j] = m1[i, j] * scale;
                }
            }
            return m;
        }

        /// <summary>
        /// 数乘矩阵
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix m1, double scale)
        {
            return scale * m1;
        }

        /// <summary>
        /// 矩阵除数
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Matrix operator /(Matrix m1, double scale)
        {
            return 1 / scale * m1;
        }

        /// <summary>
        /// 矩阵乘以(列)向量，返回(列)向量
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="v1"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Vector operator *(Matrix m1, Vector v1)
        {
            // 矩阵右乘向量得到的还是向量
            if (m1.ColumnCount != v1.Length)
                throw new Exception("矩阵列数与列向量长度不同，不能进行矩阵左乘！");
            Vector v = new Vector(m1.RowCount);
            for (int i = 0; i < v.Length; i++)
            {
                for (int j = 0; j < v1.Length; j++)
                {
                    v[i] += m1[i, j] * v1[j];
                }
            }
            return v;
        }

        /// <summary>
        /// (行)向量乘以矩阵，返回(行)向量
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="m1"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Vector operator *(Vector v1, Matrix m1)
        {
            if (v1.Length != m1.RowCount)
                throw new Exception("行向量长度与矩阵行数不同，不能进行矩阵右乘！");
            Vector v = new Vector(m1.ColumnCount);
            for (int i = 0; i < v.Length; i++)
            {
                for (int j = 0; j < v1.Length; j++)
                {
                    v[i] += m1[j, i] * v1[j];
                }
            }
            return v;
        }

        /// <summary>
        /// 按元素相乘
        /// </summary>
        /// <param name="m1"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Matrix Multiply(Matrix m1)
        {
            if (RowCount != m1.RowCount || ColumnCount != m1.ColumnCount)
                throw new Exception("两矩阵形状不同，不能按元素相乘！");
            Matrix m = new Matrix(RowCount, ColumnCount);
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    m[i, j] = this[i, j] * m1[i, j];
                }
            }
            return m;
        }

        /// <summary>
        /// 按元素相除
        /// </summary>
        /// <param name="m1"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Matrix Divide(Matrix m1)
        {
            if (RowCount != m1.RowCount || ColumnCount != m1.ColumnCount)
                throw new Exception("两矩阵形状不同，不能按元素相除！");
            Matrix m = new Matrix(RowCount, ColumnCount);
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    m[i, j] = this[i, j] / m1[i, j];
                }
            }
            return m;
        }

        /// <summary>
        /// 返回当前矩阵的转置矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix Transpose()
        {
            Matrix m = new Matrix(ColumnCount, RowCount);
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    m[j, i] = this[i, j];
                }
            }
            return m;
        }

        /// <summary>
        /// 返回矩阵A的行列式值
        /// </summary>
        /// <returns></returns>
        //public double Determinant()
        //{
        //    return GaussElimination.Determinant(this);
        //}

        /// <summary>
        /// 返回A的逆矩阵；
        /// 当A奇异时，返回null
        /// </summary>
        /// <returns></returns>
        //public Matrix Inverse()
        //{
        //    return GaussElimination.Inverse(this);
        //}

        /// <summary>
        /// 判断是否与m1的所有元素均相同
        /// </summary>
        /// <param name="m1"></param>
        /// <returns></returns>
        public bool Equals(Matrix m1)
        {
            if (RowCount != m1.RowCount || ColumnCount != m1.ColumnCount)
                return false;
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    if (this[i, j] != m1[i, j])
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 返回子矩阵，包含原矩阵的begin到end-1行
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Matrix SubRows(int begin, int end)
        {
            if (begin < 0)
                throw new Exception("输入的begin值要大于等于0");
            if (end > ColumnCount || end == ColumnCount)
                throw new Exception("输入的end值要小于列数");

            Matrix m = new Matrix(end - begin, ColumnCount);
            for (int i = begin; i < end; i++)
            {
                m.SetRow(i - begin, GetRow(i));
            }
            return m;
        }

        /// <summary>
        /// 返回子矩阵，包含原矩阵的begin到end-1列
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Matrix SubColumns(int begin, int end)
        {
            if (begin < 0)
                throw new Exception("输入的begin值要大于等于0");
            if (end > RowCount || end == RowCount)
                throw new Exception("输入的end值要小于行数");

            var m = new Matrix(RowCount, end - begin);
            for (int i = 0; i < RowCount; i++)
            {
                Array.Copy(_data, i * ColumnCount + begin, m._data,
                    i * m.ColumnCount, m.ColumnCount);
            }
            return m;
        }

        /// <summary>
        /// 返回当前矩阵与m1拼接后的新矩阵，默认横向拼接(添加列数)
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="addNewRow"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Matrix Concat(Matrix m1, bool addNewRow = false)
        {
            // 上下拼接
            if (addNewRow)
            {
                if (ColumnCount != m1.ColumnCount)
                    throw new Exception("矩阵列数不同，不能竖向拼接！");
                Matrix m = new Matrix(RowCount + m1.RowCount, ColumnCount);
                Array.Copy(_data, m._data, _data.Length);
                Array.Copy(m1._data, 0, m._data, _data.Length, m1._data.Length);
                return m;
            }
            // 左右拼接
            else
            {
                if (RowCount != m1.RowCount)
                    throw new Exception("矩阵行数不同，不能横向拼接！");
                Matrix m = new Matrix(RowCount, ColumnCount + m1.ColumnCount);
                for (int i = 0; i < RowCount; i++)
                {
                    Array.Copy(_data, i * ColumnCount, m._data,
                        i * m.ColumnCount, ColumnCount);
                    Array.Copy(m1._data, i * m1.ColumnCount, m._data,
                        i * m.ColumnCount + ColumnCount, m1.ColumnCount);
                }
                return m;
            }
        }

        /// <summary>
        /// 矩阵的拷贝
        /// </summary>
        /// <returns></returns>
        public Matrix Copy()
        {
            return new Matrix(_data);
        }

        public override string ToString()
        {
            object[] rows = new Vector[RowCount];
            for (int i = 0; i < RowCount; i++)
            {
                rows[i] = GetRow(i);
            }
            return "[" + string.Join(",\n", rows) + "]";
        }
        #endregion
    }
}