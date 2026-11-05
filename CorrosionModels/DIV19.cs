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
            int N1 = N + 1;

            for (int i = 0; i < N3; i++)
            {
                Corrosion[i, N2] = Corrosion[i, N1];
                Corrosion[N2, i] = Corrosion[N1, i];
                Corrosion[N3, i] = Corrosion[N, i];
                Corrosion[i, N3] = Corrosion[i, N];
                Corrosion[0, i] = 2.0 * Corrosion[1, i] - Corrosion[2, i];
                Corrosion[i, 0] = 2.0 * Corrosion[i, 1] - Corrosion[i, 2];
            }
            Corrosion[0, 0] = 2.0 * Corrosion[1, 0] - Corrosion[2, 0];
            Corrosion[N3, 0] = Corrosion[N, 0];
            Corrosion[0, N3] = Corrosion[0, N];
        }
    }
}
