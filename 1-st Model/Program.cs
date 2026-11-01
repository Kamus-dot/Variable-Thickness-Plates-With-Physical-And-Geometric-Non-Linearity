using CorrosionModel;
using System;
using System.Globalization;

public class Program
{
    // Точка входа программы.
    public static void Main()
    {
        MathCorrosionModel model = new();

        int N1 = model.N + 1;
        int M1 = model.N + 3;

        for (int i = 0; i < N1; i++)
        {
            for (int j = 0; j < N1; j++)
            {
                model.H[i, j] = 0.749;
            }
        }

        if (model.JF) model.NT = 2;

        double SL = 0.5 / model.N;
        double SP = Math.PI * SL;

        for (int K = 0; K < model.NM; K++)
        {
            int KL = 2 * K + 1;
            for (int I = 0; I < M1; I++)
            {
                double P = Math.Sin(KL * I * SP) / KL;
                model.X[K, I + 1] = P;
                model.XF[K, I + 1] = P;
            }
        }
        model.Delta_t = 0.1;
        double TimeStep = 0;
        int NT1 = model.NT + 1;
        for (int i = 1; i <= 5; i++)
        {
            TimeStep = i * model.Delta_t;
            model.Cycle(model.ICON);
            model.Deflec(1);
            model.Deflec(2);
            model.Thickness(TimeStep, model.PLAST);
        }
        Console.WriteLine("Program completed.");
    }
}
