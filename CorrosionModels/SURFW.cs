using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        public void SURFW()
        {
            using (StreamWriter fileW = new StreamWriter("W.DAT"))
            {
            fileW.WriteLine("Прогиб:");
            int N1 = N + 1;
            double SL = 0.5 / N;
                for (int I = 0; I < N1; I++)
                {
                    double X = I * SL;
                    for (int J = 0; J < N1; J++)
                    {
                        double Y = J * SL;
                        fileW.WriteLine($"{X:F2} {Y:F2} {WXY[I, J]:F3}");
                    }
                }
            }
        }
    }
}
