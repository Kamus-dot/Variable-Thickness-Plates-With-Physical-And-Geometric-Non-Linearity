using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelixToolkit;
using HelixToolkit.Geometry;
using HelixToolkit.Maths;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf;
using HelixToolkit.Wpf.SharpDX;
using Microsoft.Win32.SafeHandles;
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
            model = ModelMeshSetUp();
            modelMaterial = modelMaterial = new PBRMaterial
            {
                AlbedoColor = Colors.DarkOrange.ToColor4(), 
                MetallicFactor = 0.8,                      
                RoughnessFactor = 0.3,                     
                EnableAutoTangent = true,                   
            };
            model.IsDynamic = true;
            modelTransform = new Media3D.TranslateTransform3D(0, 0, 0);

        }

        /// <summary>
        /// Постороение модели типа MeshGeometry3D, которое выполняется в Dispatcher
        /// </summary>
        private MeshGeometry3D ModelMeshSetUp()
        {
            var meshBuilder = new MeshBuilder(true, true, true);
            int N = _plate.N;
            int N_Times2 = 2 * N;
            float _H0_Div2 = -(float)_plate.H0 / 2.0f;
            float Step = 1.0f / N_Times2;

            for (int i = 0; i < N_Times2; i++)
            {
                for (int j = 0; j < N_Times2; j++)
                {
                    MeshBuilding(i, j);
                }
            }
            return meshBuilder.ToMeshGeometry3D();

            //void QuarterDefinition(int i, int j)
            //{
            //    if (i == N || j == N) return;
            //    int dim = 1;
            //    float cutStep = Step / dim;
            //    int i_flag = i > N ? -1 : 1;
            //    int j_flag = j > N ? -1 : 1;

            //    float SI = i * Step;
            //    float SJ = j * Step;

            //    if (i_flag < 0) i = N_Times2 - i;
            //    if (j_flag < 0) j = N_Times2 - j;

            //    float Ux = Step * (float)_plate.EX[i, j, _plate.NT];
            //    float Udxx = (Step * (float)_plate.EX[i + 1, j, _plate.NT] - Ux) / dim;
            //    float Udxy = (Step * (float)_plate.EX[i, j + 1, _plate.NT] - Ux) / dim;
            //    float Udx = (Step * (float)_plate.EX[i + 1, j + 1, _plate.NT] - Ux) / dim;

            //    float Uy = Step * (float)_plate.EY[i, j, _plate.NT];
            //    float Udyx = (Step * (float)_plate.EY[i + 1, j, _plate.NT] - Uy) / dim;
            //    float Udyy = (Step * (float)_plate.EY[i, j + 1, _plate.NT] - Uy) / dim;
            //    float Udy = (Step * (float)_plate.EY[i + 1, j + 1, _plate.NT] - Uy) / dim;

            //    float Dx = Step * (float)_plate.EX[i, j, 0];
            //    float Ddxx = (Step * (float)_plate.EX[i + 1, j, 0] - Ux) / dim;
            //    float Ddxy = (Step * (float)_plate.EX[i, j + 1, 0] - Ux) / dim;
            //    float Ddx = (Step * (float)_plate.EX[i + 1, j + 1, 0] - Ux) / dim;

            //    float Dy = Step * (float)_plate.EY[i, j, 0];
            //    float Ddyx = (Step * (float)_plate.EY[i + 1, j, 0] - Uy) / dim;
            //    float Ddyy = (Step * (float)_plate.EY[i, j + 1, 0] - Uy) / dim;
            //    float Ddy = (Step * (float)_plate.EY[i + 1, j + 1, 0] - Uy) / dim;

            //    float HW0 = _H0_Div2 - (float)_plate.WXY[i, j];
            //    float HW0dx = (_H0_Div2 - (float)_plate.WXY[i + 1, j] - HW0) / dim;
            //    float HW0dy = (_H0_Div2 - (float)_plate.WXY[i, j + 1] - HW0) / dim;
            //    float HW0d = (_H0_Div2 - (float)_plate.WXY[i + 1, j + 1] - HW0) / dim;

            //    float H = (float)_plate.H[i, j];
            //    float Hdx = ((float)_plate.H[i + 1, j] - H) / dim;
            //    float Hdy = ((float)_plate.H[i, j + 1] - H) / dim;
            //    float Hd = ((float)_plate.H[i + 1, j + 1] - H) / dim;

            //    for (int k = 0; k < dim; k++)
            //    {
            //        for (int m = 0; m < dim; m++)
            //        {
            //            float m1Stepx = i_flag * (m + 1) * cutStep;
            //            float m1Stepy = j_flag * (m + 1) * cutStep;

            //            Vector3 Up1 = new(SI + Ux + m * Udxx,
            //                SJ + Uy + m * Udyx, H + HW0 + m * (HW0dx + Hdx));
            //            Vector3 Up2 = new(SI + Ux + m * Udxx + Udxy,
            //                SJ + m1Stepy + Uy + m * Udyx + Udyy, H + Hdy + HW0 + HW0dy + m * (HW0dx + Hdx));
            //            Vector3 Up3 = new(SI + m1Stepx + Ux + m * Udxx + Udx,
            //                SJ + m1Stepy + Uy + m * Udyx + Udy, H + Hd + HW0 + HW0d + m * (HW0dx + Hdx));
            //            Vector3 Up4 = new(SI + m1Stepx + Ux + (m + 1) * Udxx,
            //                SJ + Uy + (m + 1) * Udyx, H + HW0 + (m + 1) * (HW0dx + Hdx));

            //            Vector3 Dn1 = new(SI + Dx + m * Ddxx,
            //                SJ + Dy + m * Ddyx, HW0 + m * HW0dx);
            //            Vector3 Dn2 = new(SI + Dx + m * Ddxx + Ddxy,
            //                SJ + m1Stepy + m * Ddyx + Dy + Ddyy, HW0 + HW0dy + m * HW0dx);
            //            Vector3 Dn3 = new(SI + m1Stepx + Dx + m * Ddxx + Ddx,
            //                SJ + m1Stepy + m * Ddyx + Ddy, HW0 + HW0d + m * HW0dx);
            //            Vector3 Dn4 = new(SI + m1Stepx + Dx + (m + 1) * Ddxx,
            //                SJ + Dy + (m + 1) * Ddyx, HW0 + (m + 1) * HW0dx);

            //            if (i_flag * j_flag > 0)
            //            {
            //                // Верхняя грань
            //                meshBuilder.AddQuad(Up1, Up4, Up3, Up2);
            //                // Нижняя грань
            //                meshBuilder.AddQuad(Dn1, Dn2, Dn3, Dn4);

            //                // Боковые грани
            //                if (j == 0) meshBuilder.AddQuad(Up1, Dn1, Dn4, Up4);
            //                if (i == 0) meshBuilder.AddQuad(Dn1, Up1, Up2, Dn2);
            //            }
            //            else
            //            {
            //                // Верхняя грань
            //                meshBuilder.AddQuad(Up1, Up2, Up3, Up4);
            //                // Нижняя грань
            //                meshBuilder.AddQuad(Dn1, Dn4, Dn3, Dn2);

            //                // Боковые грани
            //                if (j == 0) meshBuilder.AddQuad(Up1, Up4, Dn4, Dn1);
            //                if (i == 0) meshBuilder.AddQuad(Up1, Dn1, Dn2, Up2);
            //            }
            //            Ux += Udxy;
            //            Uy += Udyy;
            //            Dx += Ddxy;
            //            Dy += Ddyy;
            //            HW0 += HW0dy;
            //            H += Hdy;
            //        }
            //    }
            //}
            void MeshBuilding(int i, int j)
            {
                int i_flag = i < N ? 1 : -1;
                int j_flag = j < N ? 1 : -1;

                float SI = i * Step;
                float SI1 = (i + 1) * Step;
                float SJ = j * Step;
                float SJ1 = (j + 1) * Step;

                if (i_flag < 0) i = N_Times2 - i;
                if (j_flag < 0) j = N_Times2 - j;
                try
                {
                    Vector3 Up1 = new(SI + Step * (float)_plate.dU_dX[i, j, 1],
                        SJ + Step * (float)_plate.dV_DY[i, j, 0],
                        (float)_plate.H[i, j] + _H0_Div2 - (float)_plate.WXY[i, j]);
                    Vector3 Up2 = new(SI + Step * (float)_plate.dU_dX[i, j + j_flag, 1],
                        SJ1 + Step * (float)_plate.dV_DY[i, j + j_flag, 1],
                        (float)_plate.H[i, j + j_flag] + _H0_Div2 - (float)_plate.WXY[i, j + j_flag]);
                    Vector3 Up3 = new(SI1 + Step * (float)_plate.dU_dX[i + i_flag, j + j_flag, 1],
                        SJ1 + Step * (float)_plate.dV_DY[i + i_flag, j + j_flag, 1],
                        (float)_plate.H[i + i_flag, j + j_flag] + _H0_Div2 - (float)_plate.WXY[i + i_flag, j + j_flag]);
                    Vector3 Up4 = new(SI1 + Step * (float)_plate.dU_dX[i + i_flag, j, 1],
                        SJ + Step * (float)_plate.dV_DY[i + i_flag, j, 1],
                        (float)_plate.H[i + i_flag, j] + _H0_Div2 - (float)_plate.WXY[i + i_flag, j]);
                    Vector3 Dn1 = new(SI + Step * (float)_plate.dU_dX[i, j, 0],
                        SJ + Step * (float)_plate.dV_DY[i, j, 0],
                        _H0_Div2 - (float)_plate.WXY[i, j]);
                    Vector3 Dn2 = new(SI + Step * (float)_plate.dU_dX[i, j + j_flag, 0],
                        SJ1 + Step * (float)_plate.dV_DY[i, j + j_flag, 0],
                        _H0_Div2 - (float)_plate.WXY[i, j + j_flag]);
                    Vector3 Dn3 = new(SI1 + Step * (float)_plate.dU_dX[i + i_flag, j + j_flag, 0],
                        SJ1 + Step * (float)_plate.dV_DY[i + i_flag, j + j_flag, 0],
                        _H0_Div2 - (float)_plate.WXY[i + i_flag, j + j_flag]);
                    Vector3 Dn4 = new(SI1 + Step * (float)_plate.dU_dX[i + i_flag, j, 0],
                        SJ + Step * (float)_plate.dV_DY[i + i_flag, j, 0],
                        _H0_Div2 - (float)_plate.WXY[i + i_flag, j]);

                    // Верхняя грань
                    meshBuilder.AddQuad(Up1, Up4, Up3, Up2);
                    // Нижняя грань
                    meshBuilder.AddQuad(Dn1, Dn2, Dn3, Dn4);

                    // Боковые грани
                    if (j == 0) meshBuilder.AddQuad(Up1, Dn1, Dn4, Up4);
                    else if (j + j_flag == 0) meshBuilder.AddQuad(Dn2, Up2, Up3, Dn3);
                    if (i == 0) meshBuilder.AddQuad(Dn1, Up1, Up2, Dn2);
                    else if (i + i_flag == 0) meshBuilder.AddQuad(Up4, Dn4, Dn3, Up3);

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + $"at {i}, {j}");
                }
            }
        }
    }
}


