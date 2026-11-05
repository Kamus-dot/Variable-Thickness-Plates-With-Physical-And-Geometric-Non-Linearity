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
        model.H0 = 0.749;

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
        model.StartLoad();
        model.Cycle(model.PLAST);
        Console.WriteLine("Program completed.");
    }
}
