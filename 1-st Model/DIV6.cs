using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public void Deflec(int IWF)
        {
            int N1 = N + 1;

            for (int i = 0; i < N1; i++)
            {
                for (int j = 0; j < N1; j++)
                {
                    double SP = 0;
                    if (IWF == 1)
                    {
                        for (int k = 0; k < NM; k++)
                        {
                            SP += X[k, i + 1] * Y[k, j + 1];
                        }
                        WXY[i, j] = SP;
                    }
                    else if (IWF == 2)
                    {
                        for (int k = 0; k < NM; k++)
                        {
                            SP += XF[k, i + 1] * YF[k, j + 1];
                        }
                        FXY[i, j] = SP;
                    }
                    else return;
                }
            }
        }
    }
}
