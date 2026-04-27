using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_nd_Model
{
    public partial class ModelTwo
    {
        public static void Start()
        {
            int VN = NX;
            double STEP1 = STEP / (VN * Math.Sqrt(2)) * (Math.Sqrt(VN + 1) + VN - 1);
            double STEP2 = STEP / (VN * Math.Sqrt(2)) * (Math.Sqrt(VN + 1) - 1);

            double[,] A = new double[50, 50];

            // Initialize A array
            for (int J = 0; J < NX; J++)
            {
                A[0, J] = 0;
            }

            for (int I = 1; I < K1; I++)
            {
                for (int J = 0; J < NX; J++)
                {
                    A[I, J] = STEP2;
                    int L = I - 1;
                    A[I, L] = STEP1;
                }
            }

            // Update X1 array
            for (int I = 0; I < K1; I++)
            {
                for (int J = 0; J < NX; J++)
                {
                    X1[I, J] = X_[J] + A[I, J];
                }
            }
        }
    }
}
