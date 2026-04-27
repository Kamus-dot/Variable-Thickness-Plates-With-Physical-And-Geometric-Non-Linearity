using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public static void Deflec(int IWF)
        {
            int M1 = N + 4;

            for (int i = 0; i < M1; i++)
            {
                for (int j = 0; j < M1; j++)
                {
                    double SP1 = 0.0;
                    double SP2 = 0.0;
                    for (int k = 0; k < NM; k++)
                    {
                        SP1 += X[k, i] * Y[k, j];
                        SP2 += XF[k, i] * YF[k, j];
                    }
                    if (IWF == 2)
                    {
                        FXY[i, j] = SP2;
                    }
                    else
                    {
                        WXY[i, j] = SP1;
                    }
                }
            }
        }
    }
}
