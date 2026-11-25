using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        private void CorrosionExtrapolation()
        {
            int N3 = N + 3;
            int N2 = N + 2;
            int N_1 = N - 1;

            for (int i = 0; i < N3; i++)
            {
                Corrosion[i, N2] = Corrosion[i, N];
                Corrosion[N2, i] = Corrosion[N, i];
                Corrosion[N3, i] = Corrosion[N_1, i];
                Corrosion[i, N3] = Corrosion[i, N_1];
                Corrosion[0, i] = 3.0 * Corrosion[1, i] - 3.0 * Corrosion[2, i] + Corrosion[3, i];
                Corrosion[i, 0] = 3.0 * Corrosion[i, 1] - 3.0 * Corrosion[i, 2] + Corrosion[i, 3];
            }
            Corrosion[0, 0] = 3.0 * Corrosion[0, 1] - 3.0 * Corrosion[0, 2] + Corrosion[0, 3];
            Corrosion[N3, 0] = Corrosion[N_1, 0];
            Corrosion[0, N3] = Corrosion[0, N_1];
        }
    }
}
