using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        /// <summary>
        /// Диаграмма напряжения
        /// </summary>
        /// <param name="x">Значение интенсивности деформации</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public double PLAST(double x)
        {
            if (JF)
            {
                return 3 * x;
            }
            if (x >= ES)
            {
                double? D = 3 * ES + 3 * G1 * (x - ES);
                return D is null ? throw new NullReferenceException() : D.Value;
            }
            return 3 * x;
        }
    }
}
