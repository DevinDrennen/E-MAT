using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace EMAT3.Windows.Utitlities
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class NodeVisualizationWindow : Window
    {
        public NodeVisualizationWindow()
        {
            InitializeComponent();

            //nv = Node Visualization

            Viewport3D nvViewport = new Viewport3D();
            Model3DGroup nvModel3DGroup = new Model3DGroup();
            GeometryModel3D nvGeometryModel = new GeometryModel3D();
            ModelVisual3D nvModelVisual3D = new ModelVisual3D();

            // Defines the camera used to view the 3D object - the 3D object representing our sensor. 
            PerspectiveCamera nodeCamera = new PerspectiveCamera();

            // Camera is two units above the XY plane
            nodeCamera.Position = new Point3D(0, 0, 4);

            // Camera is look down at the XY plane
            nodeCamera.LookDirection = new Vector3D(0, 0, -1);

            // The Camera's FOV - will need adjusted
            nodeCamera.FieldOfView = 60;

            // Asign the camera to the viewport
            nvViewport.Camera = nodeCamera;

            AmbientLight _ambLight = new AmbientLight(System.Windows.Media.Brushes.White.Color);

            RotateTransform3D myRotateTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 20));

            Vector3DAnimation myVectorAnimation = new Vector3DAnimation(new Vector3D(-1, -1, -1), new Duration(TimeSpan.FromMilliseconds(5000)));
            myVectorAnimation.RepeatBehavior = RepeatBehavior.Forever;

            myRotateTransform.Rotation.BeginAnimation(AxisAngleRotation3D.AxisProperty, myVectorAnimation);

            cube1TransformGroup.Children.Add(myRotateTransform);
        }
    }
}
