using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Input;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using System.Text.RegularExpressions;

namespace tekenprogramma
{
    public sealed partial class MainPage : Page
    {
        string type = "Rectangle"; //creation modus
        bool selecting = false; //selection modus
        public Invoker invoker = new Invoker();
        public List<Shape> selectedShapesList = new List<Shape>();
        public List<FrameworkElement> selectedElements = new List<FrameworkElement>();
        public FrameworkElement selectedElement; //selected element
        public List<FrameworkElement> elements = new List<FrameworkElement>(); //selected elements list

        public MainPage()
        {
            InitializeComponent();
        }

        //press on canvas
        private void Drawing_pressed(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement checkElement = e.OriginalSource as FrameworkElement;
            //canvas elements
            if (checkElement.Name == "Rectangle")
            {
                selectedElement = checkElement;
                selecting = true;
                Selecting(sender, e, selectedElement);
            }
            else if (checkElement.Name == "Ellipse")
            {
                selectedElement = checkElement;
                selecting = true;
                Selecting(sender, e, selectedElement);
            }
            //not canvas elements
            else
            {
                selecting = false;
                //move
                if (type == "Move")
                {
                     MovingShape(sender, e);                   
                }
                //resize
                else if (type == "Resize")
                {
                     ResizingShape(sender, e);               
                }
                //make shapes
                else if (type == "Rectangle")
                {
                    MakeRectangle(sender, e);
                }
                else if (type == "Elipse")
                {
                    MakeEllipse(sender, e);
                }
            }
        }

        //selecting shape
        private void Selecting(object sender, PointerRoutedEventArgs e, FrameworkElement element)
        {
            Shape shape = new Shape(e.GetCurrentPoint(paintSurface).Position.X, e.GetCurrentPoint(paintSurface).Position.Y, 50, 50);
            ICommand place = new Select(shape, e, this.invoker, paintSurface);
            this.invoker.Execute(place);
        }

        //make rectangle shape
        private void MakeRectangle(object sender, PointerRoutedEventArgs e)
        {
            Shape shape = new Shape(e.GetCurrentPoint(paintSurface).Position.X, e.GetCurrentPoint(paintSurface).Position.Y, 50, 50);
            ICommand place = new MakeRectangles(shape, this.invoker, paintSurface);
            this.invoker.Execute(place);
        }

        //make ellipse shape
        private void MakeEllipse(object sender, PointerRoutedEventArgs e)
        {
            Shape shape = new Shape(e.GetCurrentPoint(paintSurface).Position.X, e.GetCurrentPoint(paintSurface).Position.Y, 50, 50);
            ICommand place = new MakeEllipses(shape, this.invoker, paintSurface);
            this.invoker.Execute(place);
        }

        //moving shape
        private void MovingShape(object sender, PointerRoutedEventArgs e)
        {
            Location location = new Location();
            location.x = e.GetCurrentPoint(paintSurface).Position.X;
            location.y = e.GetCurrentPoint(paintSurface).Position.Y;
            location.width = selectedElement.Width;
            location.height = selectedElement.Height;
            Shape shape = new Shape(location.x, location.y, location.width, location.height);
            ICommand place = new Moving(shape, invoker, location, paintSurface, selectedElement);
            this.invoker.Execute(place);
        }

        //resizing shape
        private void ResizingShape(object sender, PointerRoutedEventArgs e)
        {
            Location location = new Location();
            location.x = e.GetCurrentPoint(paintSurface).Position.X;
            location.y = e.GetCurrentPoint(paintSurface).Position.Y;
            location.width = selectedElement.Width;
            location.height = selectedElement.Height;
            Shape shape = new Shape(location.x, location.y, location.width, location.height);
            ICommand place = new Resize(shape, invoker, e, location, paintSurface, selectedElement);
            this.invoker.Execute(place);
        }
        
        //move click
        private void Move_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
        }

        //resize click
        private void Resize_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
        }

        //elipse click
        private void Elipse_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
        }

        //rectangle click
        private void Rectangle_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
        }

        //ornament click
        private void Ornament_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
        }

        //group click
        private void Group_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
        }

        //undo click
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
            invoker.Undo();
        }

        //redo click
        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
            invoker.Redo();
        }

        //save click
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
            Shape command = new Shape(0, 0, 0, 0);
            ICommand place = new Saved(command, paintSurface);
            invoker.Execute(place);
        }

        //load click
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
            Shape command = new Shape(0, 0, 0, 0);
            ICommand place = new Loaded(command, paintSurface,invoker);
            invoker.Execute(place);
        }

        private void Front_canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {

        }

        private void Width_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        
        private void Height_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}