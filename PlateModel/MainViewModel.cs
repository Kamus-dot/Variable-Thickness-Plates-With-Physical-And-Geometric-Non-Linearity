using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelixToolkit;
using HelixToolkit.Geometry;
using HelixToolkit.Maths;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using System;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using Color = System.Windows.Media.Color;
using Colors = System.Windows.Media.Colors;
using Media3D = System.Windows.Media.Media3D;
using MeshGeometry3D = HelixToolkit.SharpDX.MeshGeometry3D;
using Point3D = System.Windows.Media.Media3D.Point3D;
using Transform3D = System.Windows.Media.Media3D.Transform3D;
using Vector3D = System.Windows.Media.Media3D.Vector3D;


namespace PlateModel
{
    partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private HelixToolkit.Wpf.SharpDX.Camera? camera;
        
        [ObservableProperty]
        private IEffectsManager? effectsManager;

        [ObservableProperty]
        private Transform3D model1Transform;

        [ObservableProperty]
        private Vector3D directionalLightDirection;

        [ObservableProperty]
        private Color directionalLightColor;

        [ObservableProperty]
        private Color ambientLightColor;

        [ObservableProperty]
        private Vector3D upDirection = new(0, 1, 0);
        public LineGeometry3D AxisModel { get; }

        public HelixToolkit.Wpf.SharpDX.Material AxisModelMaterial { get; }

        public Stream? BackgroundTexture { get; }
        public MainViewModel()
        {
            effectsManager = new DefaultEffectsManager();

            // Камера
            camera = new HelixToolkit.Wpf.SharpDX.OrthographicCamera
            {
                Position = new Point3D(3, 3, 5),
                LookDirection = new Vector3D(-3, -3, -5),
                UpDirection = new Vector3D(0, 0, 1),
                FarPlaneDistance = 50000
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

            model1Transform = new Media3D.TranslateTransform3D(0, 0, 0);
        }

        private Stream? CreatePNGSample()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            //Read the texture description           
            var texDescriptionStream = assembly.GetManifestResourceStream("SimpleDemo.Sample.png");

            if (texDescriptionStream is not null)
            {
                texDescriptionStream.Position = 0;
            }

            return texDescriptionStream;
        }
    }
}


