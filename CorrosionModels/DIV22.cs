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
            using (StreamReader reader = new StreamReader("DIVD.DAT"))
            {
                var customNumberFormat = new NumberFormatInfo
                {
                    NumberDecimalSeparator = ".",
                };
                string line;
                string[] parts;

                line = reader.ReadLine();
                parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                NM = int.Parse(parts[0]);
                N = int.Parse(parts[1]);
                NT = int.Parse(parts[2]);
                EPSP = double.Parse(parts[3], customNumberFormat);
                EPSV = double.Parse(parts[4], customNumberFormat);

                line = reader.ReadLine();
                parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                EO = double.Parse(parts[0], customNumberFormat);
                HUO = double.Parse(parts[1], customNumberFormat);
                OMY = double.Parse(parts[2], customNumberFormat);
                ES = double.Parse(parts[3], customNumberFormat);
                SS = double.Parse(parts[4], customNumberFormat);

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
                Lambda_h = double.Parse(parts[1], customNumberFormat);

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
                IPL = int.Parse(parts[0]);
                ICON = int.Parse(parts[1]);
                QK = double.Parse(parts[2], customNumberFormat);
                G1 = double.Parse(parts[3], customNumberFormat);

            }

        }
    }
}
