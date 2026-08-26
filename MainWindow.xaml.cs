using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vortice.Wpf;

namespace VorticeDirectX_Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public Render DrawRender { get; private set; } = new Render();

        public MainWindow() {
            InitializeComponent();

            ViewSurface.LoadContent += LoadViewSurface;
            ViewSurface.Draw += DrawViewSurface;
            ViewSurface.UnloadContent += UnLoadViewSurface;
        }

        public void LoadViewSurface(object? sender, DrawingSurfaceEventArgs e) {
            DrawRender.Init(e);
        }

        public void DrawViewSurface(object? sender, DrawEventArgs e) {
            DrawRender.Draw(e);
        }

        public void UnLoadViewSurface(object? sender, DrawingSurfaceEventArgs e) {
            DrawRender.Release(e);
        }
    }
}