using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public static void SURFH()
        {
            using (StreamWriter fileH = new StreamWriter("H.DAT"))
            {
            fileH.WriteLine("Толщина:");
            int N1 = N + 1;
            double SL = 0.5 / N;
                for (int I = 0; I < N1; I++)
                {
                    double X = I * SL;
                    for (int J = 0; J < N1; J++)
                    {
                        double Y = J * SL;
                        fileH.WriteLine($"{X:F2} {Y:F2} {H[I, J]:F5}");
                    }
                }
            }
        }
    }
}
