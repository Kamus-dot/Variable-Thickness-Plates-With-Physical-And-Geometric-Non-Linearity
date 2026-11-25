using System;
using System.Collections.Generic;
using System.Data;
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
        public double V2 => _v2;
        private double _v2;
        /// <summary>
        /// Число шагов конечно-разностной сетки по одной из координат
        /// </summary>
        public virtual int N
        {
            get => _n; set
            {
                if (value < 0 || value % 2 != 0) throw new ArgumentException($"Ошибка в объявлении {nameof(N)}: (Разбиение по плану должно быть чётным)");
                try { ReallocateGrid(value, _nt, _nm); }
                catch (Exception e) { throw new Exception($"Не удалось изменить размерность матриц: {e.Message}"); }
                _n = value;
                _v2 = Math.Pow(1.0 / (2.0 * N), 2);
            }
        }
        private int _n;
        /// <summary>
        /// Число слоёв по толщине
        /// </summary>
        public virtual int NT
        {
            get => _nt; set
            {
                try
                {
                    if (_jf)
                    {
                        _nt = 2;
                        ReallocateGrid(_n, 2, _nm);
                        return;
                    }
                }
                catch (Exception) { throw new Exception($"Не удалось присвоить значение элементу {nameof(NT)} из-за необъявленного флага JF"); }
                if (value > 0)
                {
                    try { ReallocateGrid(_n, value, _nm); }
                    catch (Exception e) { throw new Exception($"Не удалось изменить размерность матриц: {e.Message}"); }
                    _nt = value;
                }
                else throw new ArgumentException($"Ошибка в объявлении {nameof(value)}");
            }
        }
        private int _nt;
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
        /// Количество членов ряда базисной функции 
        /// <para>(Не рекомендуется использовать больше 4)</para>
        /// </summary>
        public virtual int NM
        {
            get => _nm;
            set
            {
                try { ReallocateGrid(_n, _nt, value); }
                catch (Exception e) { throw new Exception($"Не удалось изменить размерность матриц: {e.Message}"); }
                _nm = value;
            }
        }
        private int _nm;
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
        public virtual int ICON
        {
            get => _icon; set
            {
                if (value > 0 && value % 2 == 0 && value <= N)
                    _icon = value;
                else throw new ArgumentException($"Ошибка в объявлении {nameof(ICON)}");
            }
        }
        private int _icon;
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
        /// Начальная толщина пластинки (значение толщины в каждой точке)
        /// </summary>
        public double H0 { get => _h0; set => NormDoubleValidation(nameof(H0), value, ref _h0); }
        private double _h0;
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
        public virtual bool JF
        {
            get => _jf;
            set
            {
                _jf = value;
                if (value) NT = 2;
            }
        }
        private bool _jf;
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
        /// <para>(3-ий индекс указывает на номер слоя, где 0 - нижний, NT - верхний)</para>
        /// </summary>
        public double[,,] EX { get; set; }
        /// <summary>
        /// Полные деформации пластины по направлению y
        /// <para>(3-ий индекс указывает на номер слоя, где 0 - нижний, NT - верхний)</para>
        /// </summary>
        public double[,,] EY { get; set; }
        /// <summary>
        /// Полные деформации сдвига
        /// <para>(3-ий индекс указывает на номер слоя, где 0 - самый нижний, NT - самый верхний)</para>
        /// </summary>
        public double[,,] EXY { get; set; }
        ///<summary>
        /// Начальное значение поперечной нагрузки
        /// </summary>
        public virtual double QP0 { get; set; }
        ///<summary>
        /// Перемещения по x
        ///</summary>
        public double[,,] dU_dX { get; set; }
        ///<summary>
        /// Перемещения по y
        ///</summary>
        public double[,,] dV_DY { get; set; }
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
        /// <summary>
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
        public double Delta_t { get => _delta_t; set => NormDoubleValidation(nameof(Delta_t), value, ref _delta_t); }
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
        /// Модель коррозии 
        /// </summary>
        public enum CorrosionModels { Dolinskyi, Gutman };
        public CorrosionModels CorrosionModel { get; set; }
        private double[,,] Resilience { get; set; }

        private double MaxEI;
        private double HMaxEI;
        private double MaxAVG;
        private double HMaxAVG;

        // Координаты максимума
        private int MaxI;
        private int MaxJ;
        private int MaxK;

        // Флаг на случай преждевременной остановки.
        public bool EmergencyStop = false;
        public MathCorrosionModel()
        {
            Initialize();
        }
        private static void NormDoubleValidation(string valueName, double value, ref double _value)
        {
            if (value > 1 || value < 0) throw new ArgumentException($"Ошибка в объявлении значения {valueName}");
            _value = value;
        }
    }

}
