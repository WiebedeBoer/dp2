﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Input;
using System.Collections.Generic;
using System.Linq;

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
                    //selectingShape(sender, e, tmp);
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
                    //selectingShape(sender, e, tmp);
                    selectedElement = tmp;
                }
                else
                {
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
                if (type == "Move")
                {
                    movingShape(sender, e);
                }
                else if (type == "Resize")
                {
                    resizingShape(sender, e);
                }
            }          
        }

        //selecting shape
        private void selectingShape(object sender, PointerRoutedEventArgs e, FrameworkElement tmp)
        {
            double top = (double)tmp.GetValue(Canvas.TopProperty);
            double left = (double)tmp.GetValue(Canvas.LeftProperty);
            double width = tmp.Width;
            double height = tmp.Height;
            Shape shape = new Shape(left, top, width, height);
            ICommand select = new Select(shape, e);
            this.invoker.Execute(select);
            selecting = true;
            selectedShapesList.Add(shape);
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
            Shape command = null;
            ICommand place = new Saved(command, paintSurface);
            invoker.Execute(place);
        }

        //load click
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
            
            Shape command = null;
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

        //paintSurface.Children.Clear();
        //return paintSurface;

        //front_canvas.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0,0,0,0));
        //public double ReturnSmallest(double first, double last)
        //{
        //    if(first < last)
        //    {
        //        return first;
        //    }
        //    else
        //    {
        //        return last;
        //    }
        //}


        /*

        //cpx = e.GetCurrentPoint(paintSurface).Position.X;
        //cpy = e.GetCurrentPoint(paintSurface).Position.Y;
        if (type == "Rectangle")
        {
            //MakeRectangle(e.GetCurrentPoint(paintSurface).Position.X, e.GetCurrentPoint(paintSurface).Position.Y);
            Shape shape = new Shape(e.GetCurrentPoint(paintSurface).Position.X, e.GetCurrentPoint(paintSurface).Position.Y, 50, 50);
            ICommand place = new MakeRectangles(shape, this.invoker, paintSurface);
            this.invoker.Execute(place);
        }
        else
        {
            //MakeEllipse(e.GetCurrentPoint(paintSurface).Position.X, e.GetCurrentPoint(paintSurface).Position.Y);
            Shape shape = new Shape(e.GetCurrentPoint(paintSurface).Position.X, e.GetCurrentPoint(paintSurface).Position.Y, 50, 50);
            ICommand place = new MakeEllipses(shape, this.invoker, paintSurface);
            this.invoker.Execute(place);
        }
        */

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
        //Commands command = null;
        //command = new Ellipse(left,top);
        //ICommand place = new MakeEllipses(command, left, top,paintSurface,invoker);
        //invoker.Execute(place);

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
        //Commands command = null;
        //ICommand place = new MakeRectangles(command,left,top,paintSurface,invoker);
        //invoker.Execute(place);

        //if (type == "Rectangle")
        //{
        //    //paintSurface.Children.Remove(backuprectangle);
        //    //paintSurface.Children.Add(backuprectangle);

        //}
        //else if (type == "Ellipse")
        //{
        //    //paintSurface.Children.Remove(backupellipse);
        //    //backupellipse.Height = Convert.ToDouble(Height.Text);
        //    //backupellipse.Width = Convert.ToDouble(Width.Text);
        //    //paintSurface.Children.Add(backupellipse);
        //}

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
        //Commands command = null;
        //ICommand place = new Moving(command, invoker, sender, e);
        //invoker.Execute(place);


    }
}