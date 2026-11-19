using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelixToolkit;
using HelixToolkit.Geometry;
using HelixToolkit.Maths;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf;
using HelixToolkit.Wpf.SharpDX;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using WPFPlateModel.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using Color = System.Windows.Media.Color;
using Colors = System.Windows.Media.Colors;
using Media3D = System.Windows.Media.Media3D;
using MeshGeometry3D = HelixToolkit.SharpDX.MeshGeometry3D;
using Point3D = System.Windows.Media.Media3D.Point3D;
using Transform3D = System.Windows.Media.Media3D.Transform3D;
using Vector3D = System.Windows.Media.Media3D.Vector3D;


namespace WPFPlateModel.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly PlateModel _plate;

        [ObservableProperty]
        private HelixToolkit.Wpf.SharpDX.Camera? camera;

        [ObservableProperty]
        private IEffectsManager? effectsManager;

        [ObservableProperty]
        private MeshGeometry3D model;

        [ObservableProperty]
        private Transform3D modelTransform;

        [ObservableProperty]
        private Vector3D directionalLightDirection;

        [ObservableProperty]
        private Color directionalLightColor;

        [ObservableProperty]
        private Color ambientLightColor;

        [ObservableProperty]
        private Vector3D upDirection = new(0, 1, 0);

        public LineGeometry3D AxisModel { get; }

        [ObservableProperty]
        private HelixToolkit.Wpf.SharpDX.Material modelMaterial;
        public HelixToolkit.Wpf.SharpDX.Material AxisModelMaterial { get; }

        public Stream? BackgroundTexture { get; }
        public MainViewModel() : this(new PlateModel()) { }
        public MainViewModel(PlateModel plate)
        {
            _plate = plate;
            try
            {
                _plate.Start();
                _plate.StartLoad();
                _plate.Cycle(_plate.PLAST);
            }
            catch (Exception e) { MessageBox.Show(e.Message); }

            effectsManager = new DefaultEffectsManager();

            // Камера
            Camera = new HelixToolkit.Wpf.SharpDX.PerspectiveCamera()
            {
                Position = new System.Windows.Media.Media3D.Point3D(0, 0, 10),
                LookDirection = new System.Windows.Media.Media3D.Vector3D(0, 0, -10),
                UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0)
            };

            // Axis 
            var lineBuilder = new LineBuilder();
            lineBuilder.AddLine(Vector3.Zero, Vector3.UnitX * 1.5f);
            lineBuilder.AddLine(Vector3.Zero, Vector3.UnitY * 1.5f);
            lineBuilder.AddLine(Vector3.Zero, Vector3.UnitZ * 1.5f);
            AxisModel = lineBuilder.ToLineGeometry3D();
            AxisModel.Colors = AxisModel.Positions is null ? null : new Color4Collection(AxisModel.Positions.Count)
        {
            Colors.Red.ToColor4(),
            Colors.Red.ToColor4(),
            Colors.Green.ToColor4(),
            Colors.Green.ToColor4(),
            Colors.Blue.ToColor4(),
            Colors.Blue.ToColor4()
        };
            AxisModelMaterial = new LineArrowHeadMaterial()
            {
                Color = Colors.White,
                ArrowSize = 0.05
            };
            model = ModelWireFrameSetUp();
            modelMaterial = new PhongMaterial { DiffuseColor = Colors.DodgerBlue.ToColor4() };
            model.IsDynamic = true;
            modelTransform = new Media3D.TranslateTransform3D(0, 0, 0);

        }

        /// <summary>
        /// Постороение модели типа MeshGeometry3D, которе выполняется в Dispatcher
        /// </summary>
        private MeshGeometry3D ModelWireFrameSetUp()
        {
            var meshBuilder = new MeshBuilder(true, true, true);
            int N = _plate.N;
            int N_Times2 = 2 * N;
            float _H0_Div2 = -(float)_plate.H0 / 2.0f;
            float Step = 1.0f / N;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    QuarterDefinition(i, j);
                    QuarterDefinition(i, N_Times2 - j);
                    QuarterDefinition(N_Times2 - i, j);
                    QuarterDefinition(N_Times2 - i, N_Times2 - j);
                }
            }
            return meshBuilder.ToMeshGeometry3D();

            void QuarterDefinition(int i, int j)
            {
                int i_flag = i > N ? -1 : 1;
                int j_flag = j > N ? -1 : 1;
                if (i == N) i_flag = 0;
                if (j == N) j_flag = 0;

                float SI = i * Step;
                float SI1 = (i + i_flag) * Step;
                float SJ = j * Step;
                float SJ1 = (j + j_flag) * Step;

                if (i_flag < 0) i = N_Times2 - i;
                if (j_flag < 0) j = N_Times2 - j;

                try
                {
                    Vector3 Up1 = new(SI + Step * (float)_plate.EX[i, j, _plate.NT],
                        SJ + Step * (float)_plate.EY[i, j, _plate.NT],
                        (float)_plate.H[i, j] + _H0_Div2 - (float)_plate.WXY[i, j]);
                    Vector3 Up2 = new(SI + Step * (float)_plate.EX[i, j + 1, _plate.NT],
                        SJ1 + Step * (float)_plate.EY[i, j + 1, _plate.NT],
                        (float)_plate.H[i, j + 1] + _H0_Div2 - (float)_plate.WXY[i, j + 1]);
                    Vector3 Up3 = new(SI1 + Step * (float)_plate.EX[i + 1, j + 1, _plate.NT],
                        SJ1 + Step * (float)_plate.EY[i + 1, j + 1, _plate.NT],
                        (float)_plate.H[i + 1, j + 1] + _H0_Div2 - (float)_plate.WXY[i + 1, j + 1]);
                    Vector3 Up4 = new(SI1 + Step * (float)_plate.EX[i + 1, j, _plate.NT],
                        SJ + Step * (float)_plate.EY[i + 1, j, _plate.NT],
                        (float)_plate.H[i + 1, j] + _H0_Div2 - (float)_plate.WXY[i + 1, j]);

                    Vector3 Dn1 = new(SI + Step * (float)_plate.EX[i, j, 0],
                        SJ + Step * (float)_plate.EY[i, j, 0],
                        _H0_Div2 - (float)_plate.WXY[i, j]);
                    Vector3 Dn2 = new(SI + Step * (float)_plate.EX[i, j + 1, 0],
                        SJ1 + Step * (float)_plate.EY[i, j + 1, 0],
                        _H0_Div2 - (float)_plate.WXY[i, j + 1]);
                    Vector3 Dn3 = new(SI1 + Step * (float)_plate.EX[i + 1, j + 1, 0],
                        SJ1 + Step * (float)_plate.EY[i + 1, j + 1, 0],
                        _H0_Div2 - (float)_plate.WXY[i + 1, j + 1]);
                    Vector3 Dn4 = new(SI1 + Step * (float)_plate.EX[i + 1, j, 0],
                        SJ + Step * (float)_plate.EY[i + 1, j, 0],
                        _H0_Div2 - (float)_plate.WXY[i + 1, j]);

                    if (i_flag * j_flag > 0)
                    {
                        // Верхняя грань
                        meshBuilder.AddQuad(Up1, Up4, Up3, Up2);
                        // Нижняя грань
                        meshBuilder.AddQuad(Dn1, Dn2, Dn3, Dn4);

                        // Боковые грани
                        if (j == 0) meshBuilder.AddQuad(Up1, Dn1, Dn4, Up4);
                        if (i == 0) meshBuilder.AddQuad(Dn1, Up1, Up2, Dn2);
                    }
                    else
                    {
                        // Верхняя грань
                        meshBuilder.AddQuad(Up1, Up2, Up3, Up4);
                        // Нижняя грань
                        meshBuilder.AddQuad(Dn1, Dn4, Dn3, Dn2);

                        // Боковые грани
                        if (j == 0) meshBuilder.AddQuad(Up1, Up4, Dn4, Dn1);
                        if (i == 0) meshBuilder.AddQuad(Up1, Dn1, Dn2, Up2);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + $"at i:{i}, j:{j}");
                    throw new Exception();
                }
            }
        }
    }
}


