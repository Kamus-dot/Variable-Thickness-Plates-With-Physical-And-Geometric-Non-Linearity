using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_nd_Model
{
    public partial class ModelTwo
    {
        public static void Prisv(double[,] X, double[,] Y)
        {
            int M1 = N + 4;
            for (int i = 0; i < M1; i++)
            {
                for (int j = 0; j < M1; j++)
                {
                    X[i, j] = Y[i, j];
                }
            }
        }
    }
}
