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
        public void Cycle(Func<double, double> SIG)
        {
            do
            {
                if (CorrosionModel == CorrosionModels.Dolinskyi)
                {
                    if (DolinskyiThickness(Delta_t, MaxEI) <= 0.015 * HMaxEI) break;
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
                    if (DolinskyiThickness(t, MaxEI) > 0.015 * HMaxEI)
                    {
                        t -= Delta_t;
                        Delta_t /= 2;
                        continue;
                    }
                    DolinskyiThickness(t, SIG);
                    if (!JF) INTEGER(1, PLAST);
                    else INTEGER();
                }

                else if (CorrosionModel == CorrosionModels.Gutman)
                {
                    if (GutmanThickness(t, MaxAVG) > 0.015 * HMaxAVG)
                    {
                        Delta_t = 0.015 * HMaxAVG / GutmanThickness(t, MaxAVG);
                        t -= Delta_t;
                        continue;
                    }
                    GutmanThickness(t);
                    if (!JF) INTEGER(1, PLAST);
                    else INTEGER(); 
                }
                Console.WriteLine($"Время: {t}, Шаг по времени: {Delta_t}");
                
                CycleBuild();
                if (SIG(MaxEI) >= TensileStrength) throw new Exception("Конец");
            }
        }

        private void CycleBuild()
        {
            if (!JF) Param();
            else if (IL) Varite();
            else Var();
            DeformationIntesivity();
            if (CorrosionModel == CorrosionModels.Gutman)
            {
                Console.WriteLine($"{MaxAVG} is Max Average Deformation at {HMaxAVG}");
            }
            Console.WriteLine($"Тип коррозии: {CorrosionModel}");
            Console.WriteLine($"Физическая линейность: {JF}, Геометрическая линейность: {IL}");
            Console.WriteLine("Граничное условие для прогиба: Шарнирное опирание");
            Console.WriteLine("Граничное условие для усилий: Жёсткое опирание");
            Console.WriteLine($"{MaxEI} is Max Deformation Intensivity");
            Console.WriteLine($"ПОПЕР.НАГР.={Q} ПРОД.НАГР.={QL} ПРОГИБ В ЦЕНТРЕ={WXY[N, N]}");

            double PRO = PROGIB(WXY, N);
            Console.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO}");

            SURFW();
            double VOL = Volume(H, N);

            Console.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL}\n");
        }
    }
}

