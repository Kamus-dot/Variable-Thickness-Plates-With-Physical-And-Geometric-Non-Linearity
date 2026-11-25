using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        /// <summary>
        /// Интеграл по составной формуле Симпсона при чётном N,
        /// по формуле трапеций — при нечётном.
        /// </summary>
        /// <param name="y">Значения функции в узлах (длина ≥ N+1)</param>
        /// <param name="a">Длина отрезка интегрирования</param>
        /// <param name="n">Число шагов</param>
        public static double Integrate(double[] y, double a, int n)
        {
            if (a == 0) return 0;
            if (y == null) throw new ArgumentNullException(nameof(y));
            if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n));
            if (y.Length < n + 1) throw new ArgumentException("Массив y короче n+1", nameof(y));

            double h = a / n;

            if (n % 2 == 0)
            {
                double s = y[0] + y[n];
                for (int i = 1; i < n; i++)
                    s += (i % 2 == 1 ? 4 : 2) * y[i];
                return h * s / 3.0;
            }
            else
            {
                double s = 0;
                for (int i = 0; i < n; i++)
                    s += (y[i] + y[i + 1]) / 2.0;
                return h * s;
            }
        }
    }
}
