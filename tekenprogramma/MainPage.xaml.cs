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
        string type = "Rectangle";
        bool selecting = false;
        Rectangle backuprectangle;
        Ellipse backupellipse;

        public Invoker invoker = new Invoker();
        public List<Shape> selectedShapesList = new List<Shape>();
        public FrameworkElement selectedElement;

        public MainPage()
        {
            InitializeComponent();
        }

        private void Drawing_pressed(object sender, PointerRoutedEventArgs e)
        {
            //selecting
            if (selecting ==false)
            {
                FrameworkElement backupprep = e.OriginalSource as FrameworkElement;
                if (backupprep.Name == "Rectangle")
                {
                    Rectangle tmp = backupprep as Rectangle;
                    backuprectangle = tmp;
                    double top = (double)tmp.GetValue(Canvas.TopProperty);
                    double left = (double)tmp.GetValue(Canvas.LeftProperty);
                    double width = tmp.Width;
                    double height = tmp.Height;
                    Shape shape = new Shape(left, top, width, height);
                    ICommand select = new Select(shape, e);
                    this.invoker.Execute(select);
                    selecting = true;
                    selectedShapesList.Add(shape);
                    selectedElement = tmp;
                }
                else if (backupprep.Name == "Ellipse")
                {
                    Ellipse tmp = backupprep as Ellipse;
                    backupellipse = tmp;
                    double top = (double)tmp.GetValue(Canvas.TopProperty);
                    double left = (double)tmp.GetValue(Canvas.LeftProperty);
                    double width = tmp.Width;
                    double height = tmp.Height;
                    Shape shape = new Shape(left, top, width, height);
                    ICommand select = new Select(shape, e);
                    this.invoker.Execute(select);
                    selecting = true;
                    selectedShapesList.Add(shape);
                    selectedElement = tmp;
                }
                else
                {
                    //make shapes
                    if (type == "Rectangle")
                    {
                        MakeRectangle(sender, e);
                    }
                    else if (type == "Elipse")
                    {
                        MakeEllipse(sender, e);
                    }
                }
            }
            else
            {
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
            }          
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
        private void movingShape(object sender, PointerRoutedEventArgs e)
        {
            Shape shape = selectedShapesList.First();
            ICommand place = new Moving(shape, e, paintSurface, invoker, selectedElement);
            this.invoker.Execute(place);
            type = "deselecting";
            selecting = false;
            selectedShapesList.RemoveAt(0);
            selectedElement = null;
        }

        //resizing shape
        private void resizingShape(object sender, PointerRoutedEventArgs e)
        {
            Shape shape = selectedShapesList.First();
            ICommand place = new Resize(shape, e, paintSurface, invoker, selectedElement);
            this.invoker.Execute(place);
            type = "deselecting";
            selecting = false;
            selectedShapesList.RemoveAt(0);
            selectedElement = null;
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
            ICommand place = new Loaded(command, paintSurface);
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