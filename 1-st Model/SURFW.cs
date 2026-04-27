using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public static void SURFW()
        {
            using (StreamWriter fileW = new StreamWriter("W.DAT"))
            {
            fileW.WriteLine("Прогиб:");
            int N2 = N + 2;
            double SL = 0.5 / N;
                for (int I = 1; I < N2; I++)
                {
                    double X = (I - 1) * SL;
                    for (int J = 0; J < N2; J++)
                    {
                        double Y = (J - 1) * SL;
                        fileW.WriteLine($"{X:F2} {Y:F2} {WXY[I, J]:F3}");
                    }
                }
            }
        }
    }
}
