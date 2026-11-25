using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        /// <summary>
        /// Основная процедура коррозийного процесса
        /// </summary>
        /// <param name="SIG">Диаграмма деформирования</param>
        /// <exception cref="Exception"></exception>
        public virtual MathCorrosionModel Cycle(Func<double, double> SIG)
        {
            do
            {
                if (CorrosionModel == CorrosionModels.Dolinskyi)
                {
                    if (DolinskyiThickness(Delta_t, SIG(MaxEI)) <= 0.015 * HMaxEI) break;
                    Delta_t /= 2;
                }
                else if (CorrosionModel == CorrosionModels.Gutman)
                {
                    if (GutmanThickness(Delta_t, MaxAVG) <= 0.015 * HMaxAVG) break;
                    Delta_t /= 2;
                }
                else break;
            }
            while (true);

            for (double t = Delta_t; t < TimeLimit; t += Delta_t)
            {
                if (CorrosionModel == CorrosionModels.Dolinskyi)
                {
                    if (DolinskyiThickness(t, SIG(MaxEI)) > 0.015 * HMaxEI)
                    {
                        t -= Delta_t;
                        Delta_t /= 2;
                        continue;
                    }
                    DolinskyiThickness(t, SIG);
                }

                else if (CorrosionModel == CorrosionModels.Gutman)
                {
                    if (GutmanThickness(t, MaxAVG) > 0.015 * HMaxAVG)
                    {
                        t -= Delta_t;
                        Delta_t /= 2;
                        continue;
                    }
                    GutmanThickness(t);
                }
                Console.WriteLine($"Время: {t}, Шаг по времени: {Delta_t}");

                if (!JF) INTEGER(1, PLAST);
                else INTEGER();
                CycleBuild();

                if (SIG(MaxEI) >= TensileStrength) throw new Exception("Конец");
            }
            return this;
        }

        public virtual void CycleBuild()
        {
            if (!JF) Param();
            else if (IL) Varite();
            else Var();
            DeformationIntesivity(true);
            if (CorrosionModel == CorrosionModels.Gutman)
            {
                Console.WriteLine($"{MaxAVG} is Max Average Deformation at {HMaxAVG}");
            }
            Console.WriteLine($"Размерности: N={N}, ICON={ICON}, NT={NT}");
            Console.WriteLine($"Тип коррозии: {CorrosionModel}");
            Console.WriteLine($"Физическая линейность: {JF}, Геометрическая линейность: {IL}");
            Console.WriteLine($"{MaxEI} is Max Deformation Intensivity");
            Console.WriteLine($"At: x={MaxI}, y={MaxJ}, z={MaxK}");
            Console.WriteLine($"ПОПЕР.НАГР.={Q} ПРОД.НАГР.={QL} ПРОГИБ В ЦЕНТРЕ={WXY[N, N]}");

            double PRO = PROGIB(WXY, N);
            Console.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO}");

            SURFW();
            double VOL = Volume(H);

            Console.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL}\n");
        }
    }
}

