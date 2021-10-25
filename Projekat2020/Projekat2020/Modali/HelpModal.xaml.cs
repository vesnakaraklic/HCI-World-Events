using System;
using System.Windows;
using System.Windows.Input;

namespace Projekat2020.Modali
{
    /// <summary>
    /// Interaction logic for HelpModal.xaml
    /// </summary>
    public partial class HelpModal : Window
    {
        private double posX;
        private double posY;
        public HelpModal()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (posX < 0 || posX > this.Width || posY < 0 || posY > this.Height)
                this.Close();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(this);
            Console.WriteLine(posX);
            Console.WriteLine(posY);
            posX = p.X; // private double posX is a class member
            posY = p.Y; // private double posY is a class member
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            System.Windows.Input.Mouse.Capture(this, System.Windows.Input.CaptureMode.SubTree);
        }
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            MoveBottomRightEdgeOfWindowToMousePosition();
        }

        private void MoveBottomRightEdgeOfWindowToMousePosition()
        {
            var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
            var mouse = transform.Transform(GetMousePosition());
            Left = mouse.X - ActualWidth -10;
            Top = mouse.Y + 10;
        }

        public System.Windows.Point GetMousePosition()
        {
            return this.PointToScreen(Mouse.GetPosition(this));
        }
        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            Close();
        }
    }
}
