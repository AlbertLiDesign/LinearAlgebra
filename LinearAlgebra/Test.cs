using System;

namespace LinearAlgebra
{
    public static class Test
	{
		public static void _GaussElimination()
		{
			Matrix A = new Matrix(new double[,]
				{
					{0, 2, 3 },
					{3, 4.2, 6 },
					{4.1, 5.03, 7 }
				});

			Vector b = new Vector(1, 1, 1);
			var x = GaussElimination.Solve(A, b);
			Console.WriteLine(x);
			Console.WriteLine(A * x - b);
		}

        public static void _Determinant()
        {
            Matrix A = new Matrix(new double[,]
                {
                    {4, 2, 3 },
                    {0, 4.2, 6 },
                    {0, 0, 7 }
                });

            Console.WriteLine(GaussElimination.Determinant(A));
        }

        public static void _Inverse()
        {
            Matrix A = new Matrix(new double[,]
                {
                    {4, 2, 3 },
                    {0, 4.2, 6 },
                    {0, 1, 27 }
                });

            Console.WriteLine(GaussElimination.Inverse(A) * A);
        }

        public static void _LDLT()
        {
            Matrix A = new Matrix(new double[,]
                {
                    {1, 2, 3 },
                    {2, 4.2, -1 },
                    {3, -1, 3 }
                });

            var b = new Vector(3, 2, 1);

            Console.WriteLine(b-A*LDLT.Solve(A,b));
        }

        public static void _ConditionNumber()
        {
            Matrix A = new Matrix(new double[,]
            {
                    {1, 2, 3 },
                    {2, 4.2, -1 },
                    {3, -1, 3 }
            });
            var b = new Vector(0, -3, -4);
            var b2 = b.Copy();
            b2[0] += 0.001;

            var B = Matrix.Hilbert(3);
            var x = GaussElimination.Solve(B, b);
            var x2 = GaussElimination.Solve(B, b2);
            Console.WriteLine(ConditionNumber.One(B));
            Console.WriteLine(x2-x);

            x = GaussElimination.Solve(A, b);
            x2 = GaussElimination.Solve(A, b2);
            Console.WriteLine(ConditionNumber.One(A));
            Console.WriteLine(x2 - x);
        }
    }
}

