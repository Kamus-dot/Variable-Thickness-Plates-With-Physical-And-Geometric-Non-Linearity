using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        /// <summary>
        /// Инициализирует входные параметры необходимые для постороения модели
        /// <para> (По умолчанию загружает значения из файла DIVD.DAT, который лежит внутри сборки)
        /// </summary>
        public virtual void Initialize()
        {
            using (StreamReader reader = new("DIVD.DAT"))
            {
                var customNumberFormat = new NumberFormatInfo
                {
                    NumberDecimalSeparator = ".",
                };
                string line;
                string[] parts;

                line = reader.ReadLine();
                parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                NT = int.Parse(parts[0]);
                N = int.Parse(parts[1]);
                EPSP = double.Parse(parts[2], customNumberFormat);
                EPSV = double.Parse(parts[3], customNumberFormat);

                line = reader.ReadLine();
                parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                EO = double.Parse(parts[0], customNumberFormat);
                HUO = double.Parse(parts[1], customNumberFormat);
                OMY = double.Parse(parts[2], customNumberFormat);
                ES = double.Parse(parts[3], customNumberFormat);
                SS = double.Parse(parts[4], customNumberFormat);
                TensileStrength = double.Parse(parts[5], customNumberFormat);

                line = reader.ReadLine();
                parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                G[0] = double.Parse(parts[0], customNumberFormat);
                G[1] = double.Parse(parts[1], customNumberFormat);
                GF[0] = double.Parse(parts[2], customNumberFormat);
                GF[1] = double.Parse(parts[3], customNumberFormat);
                JF = bool.Parse(parts[4]);
                IL = bool.Parse(parts[5]);

                line = reader.ReadLine();
                parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Lambda = double.Parse(parts[0], customNumberFormat);
                H0 = double.Parse(parts[1], customNumberFormat);

                line = reader.ReadLine();
                parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                QP0 = double.Parse(parts[0], customNumberFormat);
                QPE = double.Parse(parts[1], customNumberFormat);
                DQP = double.Parse(parts[2], customNumberFormat);

                line = reader.ReadLine();
                parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                QL0 = double.Parse(parts[0], customNumberFormat);
                QLE = double.Parse(parts[1], customNumberFormat);
                DQL = double.Parse(parts[2], customNumberFormat);

                line = reader.ReadLine();
                parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                LoadType = (IPL)int.Parse(parts[0]);
                ICON = int.Parse(parts[1]);
                QK = double.Parse(parts[2], customNumberFormat);
                G1 = double.Parse(parts[3], customNumberFormat);
            }
            CorrosionModel = CorrosionModels.Dolinskyi;
            Delta_t = 0.1;
            TimeLimit = 70;
            NM = 3;
        }
        /// <summary>
        /// Вызывается в случае изменения размерности сетки (по плану, либо по толщине),
        /// либо при изменении числа членов ряда базисной функции
        /// </summary>
        public virtual void ReallocateGrid(int N, int NT, int NM)
        {
            int N4 = N + 4;
            int N1 = N + 1;
            int NT1 = NT + 1;

            X = new double[NM, N4];
            Y = new double[NM, N4];
            XF = new double[NM, N4];
            YF = new double[NM, N4];

            H = new double[N1, N1];
            WXY = new double[N1, N1];
            FXY = new double[N1, N1];
            SVO = new double[N1, N1];
            SEN = new double[N1, N1];
            QTW = new double[N1, N1];

            TENS_AVG = new double[N1, N1];

            DEF_INT = new double[N1, N1, NT1];
            EX = new double[N1, N1, NT1];
            EY = new double[N1, N1, NT1];
            EXY = new double[N1, N1, NT1];
            Resilience = new double[N1, N1, NT1];

            A = new double[N4, N4];
            A_nu = new double[N4, N4];
            AZ = new double[N4, N4];
            AZ_nu = new double[N4, N4];
            B = new double[N4, N4];
            BZ = new double[N4, N4];
            Corrosion = new double[N4, N4];
        }
    }
}
