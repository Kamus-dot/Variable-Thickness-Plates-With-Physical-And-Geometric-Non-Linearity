using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        /// <summary>
        /// Матрица коэффициентов физических параметров пластины
        /// </summary>
        private double[,] A { get; set; }
        /// <summary>
        /// Матрица коэффициентов физических параметров пластины
        /// </summary>
        private double[,] A_nu { get; set; }
        /// <summary>
        /// Матрица коэффициентов физических параметров пластины
        /// </summary>
        private double[,] AZ { get; set; }
        /// <summary>
        /// Матрица коэффициентов физических параметров пластины
        /// </summary>
        private double[,] AZ_nu { get; set; }
        /// <summary>
        /// Матрица коэффициентов физических параметров пластины
        /// </summary>
        private double[,] B { get; set; }
        /// <summary>
        /// Матрица коэффициентов физических параметров пластины
        /// </summary>
        private double[,] BZ { get; set; }
        /// <summary>
        /// Базисная функция прогиба по оси x
        /// </summary>
        public double[,] X { get; set; }
        /// <summary>
        /// Базисная функция прогиба по оси y
        /// </summary>
        public double[,] Y { get; set; }
        /// <summary>
        /// Базисная функции усилий, зависящая от x
        /// </summary>
        public double[,] XF { get; set; }
        /// <summary>
        /// Базисная функция усилий, зависящая от y
        /// </summary>
        public double[,] YF { get; set; }
        /// <summary>
        /// Коэффициент прямоугольности пластинки
        /// <para>Lambda = a/b, где a, b - длина и ширина пластинки</para>
        /// </summary>
        public virtual double Lambda { get; set; }
        /// <summary>
        /// Шаг сетки в квадрате
        /// </summary>
        public double V2 { get; }
        // Число приближений в методе вариационных итераций (<= 4)
        public virtual int NM { get; set; }
        /// <summary>
        /// Число шагов конечно-разностной сетки по одной из координат
        /// </summary>
        public virtual int N
        {
            get => _n; set
            {
                if (value < 0 || value % 2 != 0) throw new ArgumentOutOfRangeException(nameof(value), "Ошибка в объявлении (Разбиение по плану должно быть чётным)");
                _n = value;
            }
        }
        private int _n;
        /// <summary>
        /// Число слоёв по толщине
        /// </summary>
        public virtual int NT { get; set; }
        private double EMIN { get; set; }
        /// <summary>
        /// Погрешность вычислений в методе переменных параметров упругости
        /// </summary>
        public virtual double EPSP { get => _epsp; set => NormDoubleValidation(nameof(EPSP), value, ref _epsp); }
        private double _epsp;
        /// <summary>
        /// Погрешность вычислений в методе вариационных итераций
        /// </summary>
        public virtual double EPSV { get => _epsv; set => NormDoubleValidation(nameof(EPSV), value, ref _epsv); }
        private double _epsv;
        /// <summary>
        /// Модуль упругости в физически линейной задаче
        /// </summary>
        public virtual double EO { get; set; }
        /// <summary>
        /// Коэффициент Пуассона в физически линейной задаче
        /// </summary>
        public virtual double HUO { get; set; }
        /// <summary>
        /// Объемный модуль упругости
        /// </summary>
        public virtual double OMY { get; set; }
        /// <summary>
        /// Деформация текучести
        /// </summary>
        public virtual double ES { get; set; }
        /// <summary>
        /// Напряжение текучести
        /// </summary>
        public virtual double SS { get; set; }
        /// <summary>
        /// Предел прочности пластины
        /// </summary>
        public virtual double TensileStrength { get; set; }
        public double Q { get; set; }
        /// <summary>
        /// Размерность приложенной нагрузки на пластину
        /// </summary>
        public virtual int ICON { get; set; }
        /// <summary>
        /// Коэффиценты граничных условий базисных функций
        /// </summary>
        public virtual double[] G { get; set; } = new double[2];

        /// <summary>
        /// Коэффиценты граничных условий функций усилий
        /// </summary>
        public virtual double[] GF { get; set; } = new double[2];
        /// <summary>
        /// Поперечная нагрузка для уравнения прогибов
        /// </summary>
        private double[,] QTW { get; set; }
        /// <summary>
        /// Нагрузка для уравнения функции усилий
        /// </summary>
        private double[,] QTF { get; set; }
        /// <summary>
        /// Начальная толщина пластинки
        /// </summary>
        public double H0 { get; set; }
        /// <summary>
        /// Толщина пластинки
        /// </summary>
        public double[,] H { get; set; }
        /// <summary>
        /// Поля прогибов
        /// </summary>
        public double[,] WXY { get; set; }
        /// <summary>
        /// Функция усилий (Функция Эри)
        /// </summary>
        public double[,] FXY { get; set; }
        /// <summary>
        /// JF = true - физически линейная задача, JF = false - нелинейная)
        /// </summary>
        public virtual bool JF { get; set; }
        /// <summary>
        /// IL = true - геометрически линейная задача, IL = false - нелинейная)
        /// </summary>
        public virtual bool IL { get; set; }
        /// <summary>
        /// Интенсивность деформации для каждной точки
        /// </summary>
        public double[,,] DEF_INT { get; set; }
        /// <summary>
        /// <summary>
        /// Среднее напряжение на верхнем слое пластины
        /// </summary>
        public double[,] TENS_AVG { get; set; }
        /// <summary>
        /// Полные деформации пластины по направлению x
        /// </summary>
        public double[,,] EX { get; set; }
        /// <summary>
        /// Полные деформации пластины по направлению y
        /// </summary>
        public double[,,] EY { get; set; }
        /// <summary>
        /// Полные деформации сдвига
        /// </summary>
        public double[,,] EXY { get; set; }
        /// Начальное значение поперечной нагрузки
        /// </summary>
        public virtual double QP0 { get; set; }
        /// <summary>
        /// Конечное значение поперечной нагрузки
        /// </summary>
        public virtual double QPE { get; set; }
        /// <summary>
        /// Шаг изменения поперечной нагрузки
        /// </summary>
        public virtual double DQP { get; set; }
        /// <summary>
        /// Начальное значение продольной нагрузки
        /// </summary>
        public virtual double QL0 { get; set; }
        /// <summary>
        /// Конечное значение продольной нагрузки
        /// </summary>
        public virtual double QLE { get; set; }
        /// <summary>
        /// Шаг изменения продольной нагрузки
        /// </summary>
        public virtual double DQL { get; set; }
        /// <summary>
        /// Если IPL = 0 - нагрузка поперечная
        /// <para>Если IPL = 1 - нагрузка продольная</para>
        /// <para>Если IPL = 2 - нагрузка смешанная</para>
        /// </summary>
        /// 
        public enum IPL { Transverse, Longitudinal, Both }
        public IPL LoadType { get; set; }
        /// <summary>
        /// Продольная нагрузка
        /// </summary>
        public double QL { get; set; }
        /// QK = 0 - Одноостное сжатие
        /// QK = 1 - Двуостное сжатие
        /// </summary>
        public virtual double QK { get => _qk; set => _qk = value != 1 && value != 0 ? throw new ArgumentException($"Ошибка объявления {nameof(QK)}") : value; }
        private double _qk;
        /// <summary>
        /// Модуль сдвига
        /// </summary>
        public virtual double G1 { get => _g1; set => NormDoubleValidation(nameof(G1), value, ref _g1); }
        private double _g1;
        public double[,]? SEN { get; set; }
        /// <summary>
        /// Энергия
        /// </summary>
        public double[,] SVO { get; set; }
        public double MVO { get; set; }
        /// <summary>
        /// Безразмерный шаг по времени 
        /// </summary>
        public virtual double Delta_t { get => _delta_t; set => NormDoubleValidation(nameof(Delta_t), value, ref _delta_t); }
        private double _delta_t;
        /// <summary>
        /// Количество шагов по времени
        /// </summary>
        public virtual double TimeLimit { get; set; }
        /// <summary>
        /// Матрица изменений толщины в результате коррозии
        /// </summary>
        private double[,] Corrosion { get; set; }
        /// <summary>
        /// Матрица упругости 
        /// </summary>
        public enum CorrosionModels { Dolinskyi, Gutman };
        public CorrosionModels CorrosionModel { get; set; }
        private double[,,] Resilience { get; set; }
        private double MaxEI;
        private double HMaxEI;
        private double MaxAVG;
        private double HMaxAVG;
        public MathCorrosionModel()
        {
            Initialize();

            int N4 = N + 4;
            int N1 = N + 1;

            X = new double[NT, N4];
            Y = new double[NT, N4];
            XF = new double[NT, N4];
            YF = new double[NT, N4];

            A = new double[N4, N4];
            A_nu = new double[N4, N4];
            AZ = new double[N4, N4];
            AZ_nu = new double[N4, N4];
            B = new double[N4, N4];
            BZ = new double[N4, N4];
            Corrosion = new double[N4, N4];

            H = new double[N1, N1];
            QTW = new double[N1, N1];
            QTF = new double[N1, N1];
            WXY = new double[N1, N1];
            FXY = new double[N1, N1];
            SVO = new double[N1, N1];
            SEN = new double[N1, N1];

            TENS_AVG = new double[N1, N1];

            DEF_INT = new double[N1, N1, NT + 1];
            EX = new double[N1, N1, NT + 1];
            EY = new double[N1, N1, NT + 1];
            EXY = new double[N1, N1, NT + 1];
            Resilience = new double[N1, N1, NT + 1];

            V2 = Math.Pow((double)1 / (2 * N), 2);
        }
        private static void NormDoubleValidation(string valueName, double value, ref double _value)
        {
            if (value > 1 || value < 0) throw new ArgumentException($"Ошибка в объявлении значения {valueName}");
            _value = value;
        }
    }

}
