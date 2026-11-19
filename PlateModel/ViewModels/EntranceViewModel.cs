using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CorrosionModel;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using WPFPlateModel.Models;
using WPFPlateModel;


namespace WPFPlateModel.ViewModels
{
    public partial class EntranceViewModel(PlateModel model) : ObservableObject, INotifyDataErrorInfo
    {
        private readonly PlateModel _plate = model ?? throw new ArgumentNullException(nameof(model));

        public PlateModel Model => _plate;

        public EntranceViewModel() : this(new PlateModel()) { }

        #region Exception Handeling

        private readonly Dictionary<string, List<string>> ErrorsDict = [];
        public IEnumerable GetErrors(string? propertyName)
        {
            return ErrorsDict.ContainsKey(propertyName) ? ErrorsDict[propertyName] : null;
        }
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => ErrorsDict.Count != 0;
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        private void ErrorsAdd(string propertyName, string error)
        {
            if (ErrorsDict.TryGetValue(propertyName, out List<string>? list))
            {
                if (!list.Contains(error)) list.Add(error);
            }
            else ErrorsDict.Add(propertyName, [error]);
            OnErrorsChanged(propertyName);
        }
        private void ErrorsClear(string propertyName)
        {
            ErrorsDict.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }

        public Dictionary<string, string>? ConverterException
        {
            get => null;
            set
            {
                if (value is not null)
                {
                    foreach (string key in value.Keys)
                    {
                        ErrorsAdd(key, value[key]);
                    }
                }
            }
        }
        #endregion

        #region Bound Properties from EntranceWindow.xaml

        /// <summary>
        /// Начальная толщина пластинки
        /// </summary>
        public double? H0
        {
            get => _h0;
            set
            {
                _h0 = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(H0), new ArgumentNullException(nameof(H0)).Message);
                    return;
                }
                try
                {
                    _plate.H0 = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(H0));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(H0), e.Message);
                }
            }
        }
        private double? _h0 = model.H0;

        /// <summary>
        /// Число шагов конечно-разностной сетки по одной из координат (Разбиение по плану)
        /// </summary>
        public int? N
        {
            get => _n;
            set
            {
                _n = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(N), new ArgumentNullException(nameof(N)).Message);
                    return;
                }
                try
                {
                    _plate.N = value.Value;
                    OnPropertyChanged();
                    ICON = _icon;
                    ErrorsClear(nameof(N));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(N), e.Message);
                }
            }
        }
        private int? _n = model.N;

        /// <summary>
        /// Число слоёв по толщине (Разбиение пластинки по толщине)
        /// </summary>
        public int? NT
        {
            get => _nt;
            set
            {
                _nt = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(NT), new ArgumentNullException(nameof(NT)).Message);
                    return;
                }
                try
                {
                    _plate.NT = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(NT));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(NT), e.Message);
                }
            }
        }
        private int? _nt = model.NT;
        
        /// <summary>
        /// Число членов ряда базисной функции
        /// </summary>
        public int? NM
        {
            get => _nm;
            set
            {
                _nm = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(NM), new ArgumentNullException(nameof(NM)).Message);
                    return;
                }
                try
                {
                    _plate.NM = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(NM));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(NM), e.Message);
                }
            }
        }
        private int? _nm = model.NM;

        /// <summary>
        /// Отношение длины к ширине пластины (Lambda = a/b)
        /// </summary>
        public double? Lambda
        {
            get => _lambda;
            set
            {
                _lambda = value;
                if (value is null)
                {

                    ErrorsAdd(nameof(Lambda), new ArgumentNullException(nameof(Lambda)).Message);
                    return;
                }
                try
                {
                    _plate.Lambda = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(Lambda));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(Lambda), e.Message);
                }
            }
        }
        private double? _lambda = model.Lambda;

        /// <summary>
        /// Коэффициент Пуассона в физически линейной задаче
        /// </summary>
        public double? HUO
        {
            get => _huo;
            set
            {
                _huo = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(HUO), new ArgumentNullException(nameof(HUO)).Message);
                    return;
                }
                try
                {
                    _plate.HUO = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(HUO));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(HUO), e.Message);
                }
            }
        }
        private double? _huo = model.HUO;
        /// <summary>
        /// Модуль упругости в физически линейной задаче
        /// </summary>
        public double? EO
        {
            get => _eo;
            set
            {
                _eo = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(EO), new ArgumentNullException(nameof(EO)).Message);
                    return;
                }
                try
                {
                    _plate.EO = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(EO));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(EO), e.Message);
                }
            }
        }
        private double? _eo = model.EO;

        /// <summary>
        /// Объемный модуль упругости
        /// </summary>
        public double? OMY
        {
            get => _omy;
            set
            {
                _omy = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(OMY), new ArgumentNullException(nameof(OMY)).Message);
                    return;
                }
                try
                {
                    _plate.OMY = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(OMY));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(OMY), e.Message);
                }
            }
        }
        private double? _omy = model.OMY;
        /// <summary>
        /// Модуль сдвига
        /// </summary>
        public double? G1
        {
            get => _g1;
            set
            {
                _g1 = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(G1), new ArgumentNullException(nameof(G1)).Message);
                    return;
                }
                try
                {
                    _plate.G1 = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(G1));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(G1), e.Message);
                }
            }
        }
        private double? _g1 = model.G1;

        /// <summary>
        /// Деформация текучести
        /// </summary>
        public double? ES
        {
            get => _es;
            set
            {
                _es = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(ES), new ArgumentNullException(nameof(ES)).Message);
                    return;
                }
                try
                {
                    _plate.ES = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(ES));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(ES), e.Message);
                }
            }
        }
        private double? _es = model.ES;

        /// <summary>
        /// Напряжение текучести
        /// </summary>
        public double? SS
        {
            get => _ss;
            set
            {
                _ss = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(SS), new ArgumentNullException(nameof(SS)).Message);
                    return;
                }
                try
                {
                    _plate.SS = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(SS));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(SS), e.Message);
                }
            }
        }
        private double? _ss = model.SS;

        /// <summary>
        /// Предел прочности пластины
        /// </summary>
        public double? TensileStrength
        {
            get => _ts;
            set
            {
                _ts = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(TensileStrength), new ArgumentNullException(nameof(TensileStrength)).Message);
                    return;
                }
                try
                {
                    _plate.TensileStrength = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(TensileStrength));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(TensileStrength), e.Message);
                }
            }
        }
        private double? _ts = model.TensileStrength;

        /// <summary>
        /// Погрешность вычислений в методе переменных параметров упругости
        /// </summary>
        public double? EPSP
        {
            get => _epsp;
            set
            {
                _epsp = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(EPSP), new ArgumentNullException(nameof(EPSP)).Message);
                    return;
                }
                try
                {
                    _plate.EPSP = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(EPSP));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(EPSP), e.Message);
                }
            }
        }
        private double? _epsp = model.EPSP;

        /// <summary>
        /// Погрешность вычислений в методе вариационных итераций
        /// </summary>
        public double? EPSV
        {
            get => _epsv;
            set
            {
                _epsv = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(EPSV), new ArgumentNullException(nameof(EPSV)).Message);
                    return;
                }
                try
                {
                    _plate.EPSV = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(EPSV));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(EPSV), e.Message);
                }
            }
        }
        private double? _epsv = model.EPSV;

        /// <summary>
        /// Количество шагов по времени / Лимит по времени
        /// </summary>
        public double? TimeLimit
        {
            get => _timeLimit;
            set
            {
                _timeLimit = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(TimeLimit), new ArgumentNullException(nameof(TimeLimit)).Message);
                    return;
                }
                try
                {
                    _plate.TimeLimit = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(TimeLimit));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(TimeLimit), e.Message);
                }
            }
        }
        private double? _timeLimit = model.TimeLimit;

        /// <summary>
        /// Размерность приложенной нагрузки на пластину
        /// </summary>
        public int? ICON
        {
            get => _icon;
            set
            {
                _icon = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(ICON), new ArgumentNullException(nameof(ICON)).Message);
                    return;
                }
                try
                {
                    _plate.ICON = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(ICON));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(ICON), e.Message);
                }
            }
        }
        private int? _icon = model.ICON;
        /// <summary>
        /// Начальное значение поперечной нагрузки
        /// </summary>
        public double? QP0
        {
            get => _qp0;
            set
            {
                _qp0 = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(QP0), new ArgumentNullException(nameof(QP0)).Message);
                    return;
                }
                try
                {
                    _plate.QP0 = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(QP0));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(QP0), e.Message);
                }
            }
        }
        private double? _qp0 = model.QP0;
        /// <summary>
        /// Шаг изменения поперечной нагрузки
        /// </summary>
        public double? DQP
        {
            get => _dqp;
            set
            {
                _dqp = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(DQP), new ArgumentNullException(nameof(DQP)).Message);
                    return;
                }
                try
                {
                    _plate.DQP = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(DQP));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(DQP), e.Message);
                }
            }
        }
        private double? _dqp = model.DQP;

        /// <summary>
        /// Конечное значение поперечной нагрузки
        /// </summary>
        public double? QPE
        {
            get => _qpe;
            set
            {
                _qpe = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(QPE), new ArgumentNullException(nameof(QPE)).Message);
                    return;
                }
                try
                {
                    _plate.QPE = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(QPE));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(QPE), e.Message);
                }
            }
        }
        private double? _qpe = model.QPE;
        /// <summary>
        /// Начальное значение продольной нагрузки
        /// </summary>
        public double? QL0
        {
            get => _ql0;
            set
            {
                _ql0 = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(QL0), new ArgumentNullException(nameof(QL0)).Message);
                    return;
                }
                try
                {
                    _plate.QL0 = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(QL0));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(QL0), e.Message);
                }
            }
        }
        private double? _ql0 = model.QL0;
        /// <summary>
        /// Шаг изменения продольной нагрузки
        /// </summary>
        public double? DQL
        {
            get => _dql;
            set
            {
                _dql = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(DQL), new ArgumentNullException(nameof(DQL)).Message);
                    return;
                }
                try
                {
                    _plate.DQL = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(DQL));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(DQL), e.Message);
                }
            }
        }
        private double? _dql = model.DQL;
        /// <summary>
        /// Конечное значение продольной нагрузки
        /// </summary>
        public double? QLE
        {
            get => _qle;
            set
            {
                _qle = value;
                if (value is null)
                {
                    ErrorsAdd(nameof(QLE), new ArgumentNullException(nameof(QLE)).Message);
                    return;
                }
                try
                {
                    _plate.QLE = value.Value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(QLE));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(QLE), e.Message);
                }
            }
        }
        private double? _qle = model.QLE;
        #endregion

        #region ComboBox Bindings

        /// <summary>
        /// Модель коррозийного износа: 0 - Долинского, 1 - Гутмана
        /// </summary>
        public int SelectedCorrosionModelIndex
        {
            get => (int)_plate.CorrosionModel;
            set
            {
                if ((int)_plate.CorrosionModel != value && Enum.IsDefined(typeof(MathCorrosionModel.CorrosionModels), value))
                {
                    _plate.CorrosionModel = (MathCorrosionModel.CorrosionModels)value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Физическая линейность: 0 - Да (JF = true), 1 - Нет (JF = false)
        /// </summary>
        public int SelectedPhysicalLinearityIndex
        {
            get => _plate.JF ? 0 : 1;
            set
            {
                if (value == 0)
                {
                    _plate.JF = true;
                    NT = default;
                }
                else if (value == 1) _plate.JF = false;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Геометрическая линейность: 0 - Да (IL = true), 1 - Нет (IL = false)
        /// </summary>
        public int SelectedGeometricalLinearityIndex
        {
            get => _plate.IL ? 0 : 1;
            set
            {
                bool il = (value == 0);
                if (_plate.IL != il)
                {
                    _plate.IL = il;
                    OnPropertyChanged();
                }
            }
        }

        private int _selectedBoundaryConditionIndex = 0;

        /// <summary>
        /// Вид закрепления на границе: 0 - Шарнирное опирание, 1 - Жёсткое защемление
        /// </summary>
        public int SelectedBoundaryConditionIndex
        {
            get => _selectedBoundaryConditionIndex;
            set
            {
                if (_selectedBoundaryConditionIndex != value)
                {
                    _selectedBoundaryConditionIndex = value;
                    if (value == 0)
                    {
                        _plate.G[0] = -1;
                        _plate.G[1] = -1;
                        _plate.GF[0] = 1;
                        _plate.GF[1] = 1;
                    }
                    else
                    {
                        _plate.G[0] = 1;
                        _plate.G[1] = 1;
                        _plate.GF[0] = -1;
                        _plate.GF[1] = -1;
                    }
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Вид прикладываемой внешней нагрузки: 0 - Продольная, 1 - Поперечная, 2 - Продольная и поперечная
        /// </summary>
        public int SelectedExternalLoadingIndex
        {
            get => _plate.LoadType switch
            {
                MathCorrosionModel.IPL.Longitudinal => 0,
                MathCorrosionModel.IPL.Transverse => 1,
                MathCorrosionModel.IPL.Both => 2,
                _ => 1
            };
            set
            {
                MathCorrosionModel.IPL loadType = value switch
                {
                    0 => MathCorrosionModel.IPL.Longitudinal,
                    1 => MathCorrosionModel.IPL.Transverse,
                    2 => MathCorrosionModel.IPL.Both,
                    _ => MathCorrosionModel.IPL.Transverse
                };
                if (_plate.LoadType != loadType)
                {
                    _plate.LoadType = loadType;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Вид сжатия: 0 - Одноостное, 1 - Двухостное
        /// </summary>
        public int SelectedCompressionIndex
        {
            get => (int)_plate.QK;
            set
            {
                if ((int)_plate.QK != value && (value == 0 || value == 1))
                {
                    _plate.QK = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands
        public RelayCommand<string> RemoveCommand => new(execute: ErrorsClear,
            canExecute: ErrorsDict.ContainsKey);
        public RelayCommand ApplyCommand => new(execute => Apply(),
            canExcecute => !HasErrors);
        public RelayCommand<string> UpdateCommand => new(execute: OnPropertyChanged);
        private void Apply()
        {
            var mainWindow = new MainWindow(new MainViewModel(_plate));
            mainWindow.Show();
            IsWindowClosed = true;
        }

        [ObservableProperty]
        private bool _isWindowClosed = false;
        #endregion
    }
}
