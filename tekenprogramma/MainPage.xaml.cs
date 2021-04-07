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
//using System.Windows.Controls;
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
            //selecting modus
            if (selecting ==false)
            {
                selectedElement = e.OriginalSource as FrameworkElement;
                //canvas elements
                if (selectedElement.Name == "Rectangle")
                {
                    selecting = true;
                    selectedElement.Opacity =0.6; //fill opacity
                    selectedElements.Add(selectedElement);
                }
                else if (selectedElement.Name == "Ellipse")
                {
                    selecting = true;
                    selectedElement.Opacity = 0.6; //fill opacity
                    selectedElements.Add(selectedElement);
                }
                //not canvas elements
                else
                {
                    selecting = false;
                    //move
                    if (type == "Move")
                    {
                        movingShape(sender, e);
                    }
                    //resize
                    else if (type == "Resize")
                    {
                        resizingShape(sender, e);
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
            //not selecting modus
            else
            {
                selecting = false;
                //move
                if (type == "Move")
                {
                    movingShape(sender, e);
                }
                //resize
                else if (type == "Resize")
                {
                    resizingShape(sender, e);
                }
                //make
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

        //make rectangle shape
        private void MakeRectangle(object sender, PointerRoutedEventArgs e)
        {
            Shape shape = new Shape(e.GetCurrentPoint(paintSurface).Position.X, e.GetCurrentPoint(paintSurface).Position.Y, 50, 50);
            ICommand place = new MakeRectangles(shape, this.invoker, paintSurface);
            this.invoker.Execute(place);
            //Reshape(paintSurface); //repaint
        }

        //make ellipse shape
        private void MakeEllipse(object sender, PointerRoutedEventArgs e)
        {
            Shape shape = new Shape(e.GetCurrentPoint(paintSurface).Position.X, e.GetCurrentPoint(paintSurface).Position.Y, 50, 50);
            ICommand place = new MakeEllipses(shape, this.invoker, paintSurface);
            this.invoker.Execute(place);
            //Reshape(paintSurface); //repaint
        }

        //moving shape
        private void movingShape(object sender, PointerRoutedEventArgs e)
        {
            ////remove selected
            //if (selectedElement.Name == "Rectangle")
            //{
            //    paintSurface.Children.Remove(selectedElement);
            //}
            //else if (selectedElement.Name == "Ellipse")
            //{
            //    paintSurface.Children.Remove(selectedElement);
            //}
            //Shape shape = selectedShapesList.First();
            Location location = new Location();
            location.x = e.GetCurrentPoint(paintSurface).Position.X;
            location.y = e.GetCurrentPoint(paintSurface).Position.Y;
            location.width = selectedElement.Width;
            location.height = selectedElement.Height;
            Shape shape = new Shape(location.x, location.y, location.width, location.height);
            ICommand place = new Moving(shape, invoker, location, paintSurface, selectedElement);
            this.invoker.Execute(place);
            //Reshape(paintSurface); //repaint
            //type = "deselecting";
            //selecting = false;
            //selectedShapesList.RemoveAt(0);
            //selectedElement = null;
        }

        //resizing shape
        private void resizingShape(object sender, PointerRoutedEventArgs e)
        {
            ////remove selected
            //if (selectedElement.Name == "Rectangle")
            //{
            //    paintSurface.Children.Remove(selectedElement);
            //}
            //else if (selectedElement.Name == "Ellipse")
            //{
            //    paintSurface.Children.Remove(selectedElement);
            //}
            //Shape shape = selectedShapesList.First();
            Location location = new Location();
            location.x = e.GetCurrentPoint(paintSurface).Position.X;
            location.y = e.GetCurrentPoint(paintSurface).Position.Y;
            location.width = selectedElement.Width;
            location.height = selectedElement.Height;
            Shape shape = new Shape(location.x, location.y, location.width, location.height);
            ICommand place = new Resize(shape, invoker, e, location, paintSurface, selectedElement);
            this.invoker.Execute(place);
            //this.invoker.Repaint();
            //type = "deselecting";
            //selecting = false;
            //selectedShapesList.RemoveAt(0);
            //selectedElement = null;
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
            //Reshape(paintSurface); //repaint
        }

        //redo click
        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
            invoker.Redo();
            //Reshape(paintSurface); //repaint
        }

        //save click
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
            Shape command = new Shape(0, 0, 0, 0);
            ICommand place = new Saved(command, paintSurface);
            invoker.Execute(place);
            invoker.Clear();
        }

        //load click
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
            Shape command = new Shape(0, 0, 0, 0);
            ICommand place = new Loaded(command, paintSurface);
            invoker.Execute(place);
            invoker.Clear();
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

        //reshape
        public void Reshape(Canvas paintSurface)
        {
            paintSurface.Children.Clear();
            foreach (FrameworkElement element in invoker.prev)
            {

                if (element.Name == "Rectangle")
                {
                    Rectangle newRectangle = new Rectangle(); //instance of new rectangle shape
                    newRectangle.Width = element.Width; //set width
                    newRectangle.Height = element.Height; //set height     
                    SolidColorBrush brush = new SolidColorBrush(); //brush
                    brush.Color = Windows.UI.Colors.Blue; //standard brush color is blue
                    newRectangle.Fill = brush; //fill color
                    newRectangle.Name = "Rectangle"; //attach name
                    Canvas.SetLeft(newRectangle, element.ActualOffset.X); //set left position
                    Canvas.SetTop(newRectangle, element.ActualOffset.Y); //set top position 
                    paintSurface.Children.Add(newRectangle);
                }
                else if (element.Name == "Ellipse")
                {
                    Ellipse newEllipse = new Ellipse(); //instance of new ellipse shape
                    newEllipse.Width = element.Width;
                    newEllipse.Height = element.Height;
                    SolidColorBrush brush = new SolidColorBrush();//brush
                    brush.Color = Windows.UI.Colors.Blue;//standard brush color is blue
                    newEllipse.Fill = brush;//fill color
                    newEllipse.Name = "Ellipse";//attach name
                    Canvas.SetLeft(newEllipse, element.ActualOffset.X);//set left position
                    Canvas.SetTop(newEllipse, element.ActualOffset.Y);//set top position
                    paintSurface.Children.Add(newEllipse);
                }
            }
        }

    }
}