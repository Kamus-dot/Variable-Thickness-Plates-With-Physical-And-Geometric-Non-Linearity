using CorrosionModel;
using System;
using System.Globalization;

public class Program
{
    // Точка входа программы.
    public static void Main()
    {
        MathCorrosionModel m = new();
        MathCorrosionModel model = new()
        {
            N = 4,
            NT = 8,
            ICON = 2,
            QL0 = 0.5,
            DQL = 0.1,
            DQP = 0.1,
            LoadType = MathCorrosionModel.IPL.Transverse,
            //CorrosionModel = MathCorrosionModel.CorrosionModels.Gutman
        };
        model.Start();
        model.StartLoad();
        try
        {
            m = model.Cycle(model.PLAST);
        }
        catch (Exception ex)
        {
            if (model.EmergencyStop)
            {
                Console.WriteLine(ex.Message + ", выведена последняя устойчивая версия");
                throw new Exception();
            }
            else m = model;
        }

        Console.WriteLine("Program completed.");
    }
}
