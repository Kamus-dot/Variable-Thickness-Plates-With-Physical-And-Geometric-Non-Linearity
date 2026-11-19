using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorrosionModel;

namespace WPFPlateModel.Models
{
    /// <summary>
    /// Модель пластины для WPF приложения
    /// </summary>
    public class PlateModel : MathCorrosionModel
    {
        public override void Initialize()
        {
            // Инициализация входных параметров по умолчанию
            N = 30;
            NT = 2;
            NM = 3;
            EPSP = 0.01;
            EPSV = 0.01;

            EO = 2.56;
            HUO = 0.28;
            OMY = 1.94;
            ES = 0.69;
            SS = 2.07;
            TensileStrength = 2.68;

            G[0] = -1;
            G[1] = -1;
            GF[0] = 1;
            GF[1] = 1;
            JF = false;
            IL = false;

            Lambda = 1.0;
            H0 = 0.749;

            QP0 = 0.5;
            QPE = 100;
            DQP = 0.5;

            QL0 = 0;
            QLE = 1.2;
            DQL = 0.1;

            LoadType = IPL.Both;
            ICON = 10;
            QK = 1;
            G1 = 0.57;

            CorrosionModel = CorrosionModels.Dolinskyi;
            Delta_t = 0.1;
            TimeLimit = 70;
        }

        public PlateModel() : base() { }
    }
}
