using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        // Y - матрица прогиба по оси, A - шаг для интегрирования методом SIM,
        // NORM - флаг для нормализации базиса:
        // 0 - нормировать, 1 - не нормировать

        // Данный метод использует алгоритм ортогонализации Грама-Шмидта
        // базисов X или Y (данные массивы содержат NM количество
        // дискретных функций (2N точек), которые задаются в Program.cs).
        // Скалярное произведение задаётся определённым интегралом произведений
        // функций на интервале [0;1]. Произведение по тригонометрическим
        // формулами переводится в сумму, которые обращаются в ноль (поэтому
        // нужно 2N точек, расположенных зеркально, так как точка возвращается
        // в исходное положение(или еще проще: на каждое положительное значение
        // найдётся такое же отрицательное) (Важно отметить, что количество
        // элементов в массиве нечётно, но оставшиеся элементы равны (индекс N+1)).
        // (Пример для (sin(a), sin(b)) => (1/2(cos(a-b)-(cos(a+b)))).
        // Проверка ортогональности производится здесь же;

        // Норму определяю по обычному, через корень суммы квадратов элементов
        // базиса.
        public static double[,] Ort(double[,] Y, double A, double NORM)
        {
            double[,] X = new double[4, 24];
            double[,] E = new double[4, 24];
            double[] XE = new double[24];

            // Новый базис
            double[] EE = new double[24];

            double[] SL = new double[10];
            double[] S = new double[10];

            int M1 = N + 1;
            int N1 = 2 * N + 1;
            int N2 = 2 * M1;
            int M2 = 2 * N;
            for (int I = 0; I < NM; I++)
            {
                for (int J = 0; J < M1; J++)
                {
                    X[I, J] = Y[I, J + 1];
                    X[I, N2 - J - 1] = Y[I, J + 1];
                }
            }
            // Задание 1-ого нового базисного элемента
            for (int I = 0; I < N1; I++)
            {
                E[0, I] = X[0, I];
            }
            if (NM != 1)
            {
                for (int K = 1; K < NM; K++)
                {
                    for (int L = 0; L < N1; L++)
                    {
                        double F = 0;
                        for (int I = 0; I < K; I++)
                        {
                            if (L == 0)
                            {
                                for (int M = 0; M < N1; M++)
                                {
                                    XE[M] = X[K, M] * E[I, M];
                                    EE[M] = Math.Pow(X[I, M], 2);
                                }
                                // Можно выразить как (En, Xn+1)/(En, En)
                                SL[I] = SIM(XE, A, M2) / SIM(EE, A, M2);
                            }
                            F += SL[I] * E[I, L];
                        }
                        // Задание последующих элементов
                        E[K, L] = X[K, L] - F;
                    }
                }

                // Проверка ортогональности (выполняется)

                double[] Ort_EE = new double[N1];
                for (int i = 0; i < N1; i++)
                {
                    Ort_EE[i] = E[0, i] * E[2, i];
                }
                Console.WriteLine(SIM(Ort_EE, 1, M2));

                // Перезаписывается для учёта граничного условия в 1-ой точке отчёта
                // (предположительно)
                for (int I = 0; I < NM; I++)
                {
                    for (int J = 0; J < M1; J++)
                    {
                        Y[I, J + 1] = E[I, J];
                    }
                }
            }
            if (NORM == 0)
            {
                for (int I = 0; I < NM; I++)
                {
                    double P = 0;
                    for (int J = 0; J < N1; J++)
                    {
                        P += Math.Pow(E[I, J], 2);
                    }
                    double Nr = Math.Sqrt(P);
                    //for (int J = 0; J < N1; J++)
                    //{
                    //    E[I, J] = E[I, J] / N;
                    //}
                    for (int J = 0; J < M1; J++)
                    {
                        Y[I, J + 1] = Y[I, J + 1] / Nr;
                        
                    }
                }

                // Проверка выполнения нормирования (пример для 3-х слоёв)
                // (также необходимо раскомментировать блок сверху)

                //double SUM1 = 0;
                //double SUM2 = 0;
                //double SUM3 = 0;
                //for (int i = 0; i < N1; i++)
                //{
                //    SUM1 += Math.Pow(E[0, i], 2);
                //    SUM2 += Math.Pow(E[1, i], 2);
                //    SUM3 += Math.Pow(E[2, i], 2);
                //}
                //Console.WriteLine(Math.Sqrt(SUM1));
                //Console.WriteLine(Math.Sqrt(SUM2));
                //Console.WriteLine(Math.Sqrt(SUM3));
            }
            return Y;
        }
    }
}
