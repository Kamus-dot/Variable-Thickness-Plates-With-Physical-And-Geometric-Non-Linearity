using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public static void SURFS()
        {
            using (StreamWriter fileS = new StreamWriter("S.DAT"))
            {
            fileS.WriteLine("Интенсивность:");
            int N1 = N + 1;
            double SL = 0.5 / N;
                for (int I = 0; I < N1; I++)
                {
                    double X = I * SL;
                    for (int J = 0; J < N1; J++)
                    {
                        double Y = J * SL;
                        fileS.WriteLine($"{X:F2} {Y:F2} {STR[I, J]:F3}");
                    }
                }
            }
        }
    }
}
