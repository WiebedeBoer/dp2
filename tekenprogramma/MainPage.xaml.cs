using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Input;

namespace tekenprogramma
{
    public sealed partial class MainPage : Page
    {
        string type = "Rectangle";
        double cpx;
        double cpy;
        bool firstcp = true;
        bool moving = false;
        Rectangle backuprectangle;
        Ellipse backupellipse;

        public Invoker invoker = new Invoker();



        public MainPage()
        {
            InitializeComponent();
        }

        private void Drawing_pressed(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement backupprep = e.OriginalSource as FrameworkElement;
            
            if (backupprep.Name == "Rectangle")
            {
                /*
                Rectangle tmp = backupprep as Rectangle;
                backuprectangle = tmp;
                type = "Rectangle";
                */
                Shape shape = new Shape(cpx, cpy, e.GetCurrentPoint(paintSurface).Position.X, e.GetCurrentPoint(paintSurface).Position.Y);
                ICommand place = new MakeRectangles(shape);
                //ICommand place = new PlaceRectangles(command, invoker,sender,e);
                this.invoker.Execute(place);


            }
            else if(backupprep.Name == "Ellipse")
            {
                /*
                Ellipse tmp = backupprep as Ellipse;
                backupellipse = tmp;
                type = "Ellipse";
                */
                Shape shape = new Shape(cpx, cpy, e.GetCurrentPoint(paintSurface).Position.X, e.GetCurrentPoint(paintSurface).Position.Y);
                Commands command = null;
                ICommand place = new PlaceEllipses(command, invoker, sender, e);
                invoker.Execute(place);
            }
            if (moving)
            {
                /*
                cpx = e.GetCurrentPoint(paintSurface).Position.X;
                cpy = e.GetCurrentPoint(paintSurface).Position.Y;
                if (type == "Rectangle")
                {
                    Canvas.SetLeft(backuprectangle, cpx);
                    Canvas.SetTop(backuprectangle, cpy);
                    paintSurface.Children.Remove(backuprectangle);
                    paintSurface.Children.Add(backuprectangle);
                }
                else if(type == "Ellipse")
                {
                    Canvas.SetLeft(backupellipse, cpx);
                    Canvas.SetTop(backupellipse, cpy);
                    paintSurface.Children.Remove(backupellipse);
                    paintSurface.Children.Add(backupellipse);
                }
                moving = !moving;
                */
                Commands command = null;
                ICommand place = new Moving(command, invoker, sender, e);
                invoker.Execute(place);
            }
            else
            {
                if (firstcp)
                {
                    cpx = e.GetCurrentPoint(paintSurface).Position.X;
                    cpy = e.GetCurrentPoint(paintSurface).Position.Y;
                }
                else
                {
                    if (type == "Rectangle")
                    {
                        MakeRectangle(e.GetCurrentPoint(paintSurface).Position.X, e.GetCurrentPoint(paintSurface).Position.Y);
                    }
                    else
                    {
                        MakeEllipse(e.GetCurrentPoint(paintSurface).Position.X, e.GetCurrentPoint(paintSurface).Position.Y);
                    }
                }
                firstcp = !firstcp;
            }
        }

        public void MakeRectangle(double left, double top)
        {
            /*
            Rectangle newRectangle = new Rectangle();
            newRectangle.Height = Math.Abs(cpy - top);
            newRectangle.Width = Math.Abs(cpx - left);
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Windows.UI.Colors.Blue;
            newRectangle.Fill = brush;
            newRectangle.Name = "Rectangle";
            Canvas.SetLeft(newRectangle, ReturnSmallest(left, cpx));
            Canvas.SetTop(newRectangle, ReturnSmallest(top, cpy));
            newRectangle.PointerPressed += Drawing_pressed;
            paintSurface.Children.Add(newRectangle);
            Rectangle.Content = paintSurface.Children[0].Opacity;
            */
            Commands command = null;
            ICommand place = new MakeRectangles(command,left,top,paintSurface,invoker);
            invoker.Execute(place);
        }

        public void MakeEllipse(double left, double top)
        {
            /*
            Ellipse newEllipse = new Ellipse();
            newEllipse.Height = Math.Abs(cpy - top);
            newEllipse.Width = Math.Abs(cpx - left);
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Windows.UI.Colors.Blue;
            newEllipse.Fill = brush;
            newEllipse.Name = "Ellipse";
            Canvas.SetLeft(newEllipse, ReturnSmallest(left, cpx));
            Canvas.SetTop(newEllipse, ReturnSmallest(top, cpy));
            newEllipse.PointerPressed += Drawing_pressed;
            paintSurface.Children.Add(newEllipse);
            */
            Commands command = null;
            //command = new Ellipse(left,top);
            ICommand place = new MakeEllipses(command, left, top,paintSurface,invoker);
            invoker.Execute(place);
        }
        
        private void Move_Click(object sender, RoutedEventArgs e)
        {
            moving = !moving;
        }

        private void Resize_Click(object sender, RoutedEventArgs e)
        {
            if (type == "Rectangle")
            {
                paintSurface.Children.Remove(backuprectangle);
                paintSurface.Children.Add(backuprectangle);
            }
            else if (type == "Ellipse")
            {
                paintSurface.Children.Remove(backupellipse);
                backupellipse.Height = Convert.ToDouble(Height.Text);
                backupellipse.Width = Convert.ToDouble(Width.Text);
                paintSurface.Children.Add(backupellipse);
            }
        }

        public double ReturnSmallest(double first, double last)
        {
            if(first < last)
            {
                return first;
            }
            else
            {
                return last;
            }
        }


        //elipse
        private void Elipse_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
        }

        //rectangle
        private void Rectangle_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
        }

        //ornament
        private void Ornament_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
        }

        //group
        private void Group_Click(object sender, RoutedEventArgs e)
        {

        }

        //undo
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            invoker.Undo();
        }

        //redo
        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            invoker.Redo();
        }

        //save
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Commands command = null;
            ICommand place = new Saved(command, paintSurface);
            invoker.Execute(place);
        }

        //load
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            paintSurface.Children.Clear();
            Commands command = null;
            ICommand place = new Loaded(command, paintSurface);
            invoker.Execute(place);
            //return paintSurface;
        }

        //resize

        //move

        private void Front_canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //front_canvas.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0,0,0,0));
        }

        private void Width_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        
        private void Height_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}