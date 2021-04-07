﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using Windows.Storage;
using Windows.ApplicationModel.Activation;

namespace tekenprogramma
{
    public class Location
    {
        public double x;
        public double y;
        public double width;
        public double height;
    }


    public class Shape
    {
        public double x;
        public double y;
        public double width;
        public double height;

        public Invoker invoker;
        public Canvas paintSurface;

        public FrameworkElement backelement; //back element
        public FrameworkElement prevelement;
        public FrameworkElement nextelement; //next element

        public FrameworkElement lastelement;
        public FrameworkElement firstelement;
        string type = "Rectangle"; //default shape type

        private List<FrameworkElement> undoList = new List<FrameworkElement>();
        private List<FrameworkElement> redoList = new List<FrameworkElement>();
        //private List<Location> undoLocList = new List<Location>();
        //private List<Location> redoLocList = new List<Location>();

        //file IO
        public string fileText { get; set; }     

        //shape
        public Shape(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        // Selects the shape
        public void select(PointerRoutedEventArgs e)
        {
            //this.selected = true;
        }

        // Deselects the shape
        public void deselect(PointerRoutedEventArgs e)
        {
            //this.selected = false;
        }

        //give smallest
        public double returnSmallest(double first, double last)
        {
            if (first < last)
            {
                return last - first;
            }
            else
            {
                return first - last;
            }
        }

        //create rectangle
        public void makeRectangle(Invoker invoker, Canvas paintSurface)
        {
            this.type = "Rectangle";
            Rectangle newRectangle = new Rectangle(); //instance of new rectangle shape
            newRectangle.Width = width; //set width
            newRectangle.Height = height; //set height     
            SolidColorBrush brush = new SolidColorBrush(); //brush
            brush.Color = Windows.UI.Colors.Blue; //standard brush color is blue
            newRectangle.Fill = brush; //fill color
            newRectangle.Name = "Rectangle"; //attach name
            Canvas.SetLeft(newRectangle, x); //set left position
            Canvas.SetTop(newRectangle, y); //set top position 
            paintSurface.Children.Add(newRectangle); //add
            backelement = newRectangle;
        }

        //create ellipse
        public void makeEllipse(Invoker invoker, Canvas paintSurface)
        {
            this.type = "Ellipse";
            Ellipse newEllipse = new Ellipse(); //instance of new ellipse shape
            newEllipse.Width = width;
            newEllipse.Height = height;  
            SolidColorBrush brush = new SolidColorBrush();//brush
            brush.Color = Windows.UI.Colors.Blue;//standard brush color is blue
            newEllipse.Fill = brush;//fill color
            newEllipse.Name = "Ellipse";//attach name
            Canvas.SetLeft(newEllipse, x);//set left position
            Canvas.SetTop(newEllipse, y);//set top position
            paintSurface.Children.Add(newEllipse); //add
            backelement = newEllipse;
        }

        //undo create removal
        public void remove(Invoker invoker, Canvas paintSurface)
        {
            if (this.type == "Rectangle")
            {
                // if the click source is a rectangle then we will create a new rectangle
                // and link it to the rectangle that sent the click event
                Rectangle activeRec = (Rectangle)backelement; // create the link between the sender rectangle
                paintSurface.Children.Remove(activeRec); // find the rectangle and remove it from the canvas
            }
            else if (this.type == "Ellipse")
            {
                // if the click source is a rectangle then we will create a new ellipse
                // and link it to the rectangle that sent the click event
                Ellipse activeEll = (Ellipse)backelement; // create the link between the sender ellipse
                paintSurface.Children.Remove(activeEll); // find the ellipse and remove it from the canvas
            }
        }

        //undo move removal
        public void removal(Invoker invoker, Canvas paintSurface, FrameworkElement element)
        {
            if (element.Name == "Rectangle")
            {
                // if the click source is a rectangle then we will create a new rectangle
                // and link it to the rectangle that sent the click event
                Rectangle activeRec = (Rectangle)element; // create the link between the sender rectangle
                paintSurface.Children.Remove(activeRec); // find the rectangle and remove it from the canvas
            }
            else if (element.Name == "Ellipse")
            {
                // if the click source is a rectangle then we will create a new ellipse
                // and link it to the rectangle that sent the click event
                Ellipse activeEll = (Ellipse)element; // create the link between the sender ellipse
                paintSurface.Children.Remove(activeEll); // find the ellipse and remove it from the canvas
            }
        }

        //new moving shape
        public void creator(Invoker invoker, Canvas paintSurface, FrameworkElement element)
        {
            //create at new location
            if (element.Name == "Rectangle")
            {
                Rectangle newRectangle = new Rectangle(); //instance of new rectangle shape
                newRectangle.Width = element.Width; //set width
                newRectangle.Height = element.Height; //set height     
                SolidColorBrush brush = new SolidColorBrush(); //brush
                brush.Color = Windows.UI.Colors.Yellow; //standard brush color is blue
                newRectangle.Fill = brush; //fill color
                newRectangle.Name = "Rectangle"; //attach name
                Canvas.SetLeft(newRectangle, element.ActualOffset.X);
                Canvas.SetTop(newRectangle, element.ActualOffset.Y);
                paintSurface.Children.Add(newRectangle); //add
            }
            else if (element.Name == "Ellipse")
            {
                Ellipse newEllipse = new Ellipse(); //instance of new ellipse shape
                newEllipse.Width = element.Width;
                newEllipse.Height = element.Height;
                SolidColorBrush brush = new SolidColorBrush();//brush
                brush.Color = Windows.UI.Colors.Yellow;//standard brush color is blue
                newEllipse.Fill = brush;//fill color
                newEllipse.Name = "Ellipse";//attach name
                Canvas.SetLeft(newEllipse, element.ActualOffset.X);
                Canvas.SetTop(newEllipse, element.ActualOffset.Y);
                paintSurface.Children.Add(newEllipse); //add
            }
        }

        //new moving shape
        public void moving(Invoker invoker, Canvas paintSurface, Location location, FrameworkElement element)
        {
            //create at new location
            if (element.Name == "Rectangle")
            {
                Rectangle newRectangle = new Rectangle(); //instance of new rectangle shape
                newRectangle.Width = location.width; //set width
                newRectangle.Height = location.height; //set height     
                SolidColorBrush brush = new SolidColorBrush(); //brush
                brush.Color = Windows.UI.Colors.Yellow; //standard brush color is blue
                newRectangle.Fill = brush; //fill color
                newRectangle.Name = "Rectangle"; //attach name
                Canvas.SetLeft(newRectangle, location.x);
                Canvas.SetTop(newRectangle, location.y);
                paintSurface.Children.Add(newRectangle); //add
                backelement = newRectangle;
                nextelement = element;
            }
            else if (element.Name == "Ellipse")
            {
                Ellipse newEllipse = new Ellipse(); //instance of new ellipse shape
                newEllipse.Width = location.width;
                newEllipse.Height = location.height;
                SolidColorBrush brush = new SolidColorBrush();//brush
                brush.Color = Windows.UI.Colors.Yellow;//standard brush color is blue
                newEllipse.Fill = brush;//fill color
                newEllipse.Name = "Ellipse";//attach name
                Canvas.SetLeft(newEllipse, location.x);//set left position
                Canvas.SetTop(newEllipse, location.y);//set top position
                paintSurface.Children.Add(newEllipse); //add
                backelement = newEllipse;
                nextelement = element;
            }
            //remove previous
            removal(invoker, paintSurface, element);
        }

        //undo moving shape
        public void undoMoving(Invoker invoker, Canvas paintSurface)
        {
            if (backelement.Name == "Rectangle")
            {
                // if the click source is a rectangle then we will create a new rectangle
                // and link it to the rectangle that sent the click event
                Rectangle activeRec = (Rectangle)backelement; // create the link between the sender rectangle
                paintSurface.Children.Remove(activeRec); // find the rectangle and remove it from the canvas
                backelement = nextelement;
                nextelement = backelement;
            }
            else if (backelement.Name == "Ellipse")
            {
                // if the click source is a rectangle then we will create a new ellipse
                // and link it to the rectangle that sent the click event
                Ellipse activeEll = (Ellipse)backelement; // create the link between the sender ellipse
                paintSurface.Children.Remove(activeEll); // find the ellipse and remove it from the canvas
                backelement = nextelement;
                nextelement = backelement;
            }
            //create previous
            creator(invoker, paintSurface, nextelement);
        }

        //redo moving shape
        public void redoMoving(Invoker invoker, Canvas paintSurface, Location location, FrameworkElement element)
        {
            //create at new location
            if (element.Name == "Rectangle")
            {
                Rectangle newRectangle = new Rectangle(); //instance of new rectangle shape
                newRectangle.Width = location.width; //set width
                newRectangle.Height = location.height; //set height     
                SolidColorBrush brush = new SolidColorBrush(); //brush
                brush.Color = Windows.UI.Colors.Yellow; //standard brush color is blue
                newRectangle.Fill = brush; //fill color
                newRectangle.Name = "Rectangle"; //attach name
                Canvas.SetLeft(newRectangle, location.x);
                Canvas.SetTop(newRectangle, location.y);
                paintSurface.Children.Add(newRectangle); //add
                backelement = newRectangle;
                nextelement = element;
            }
            else if (element.Name == "Ellipse")
            {
                Ellipse newEllipse = new Ellipse(); //instance of new ellipse shape
                newEllipse.Width = location.width;
                newEllipse.Height = location.height;
                SolidColorBrush brush = new SolidColorBrush();//brush
                brush.Color = Windows.UI.Colors.Yellow;//standard brush color is blue
                newEllipse.Fill = brush;//fill color
                newEllipse.Name = "Ellipse";//attach name
                Canvas.SetLeft(newEllipse, location.x);//set left position
                Canvas.SetTop(newEllipse, location.y);//set top position
                paintSurface.Children.Add(newEllipse); //add
                backelement = newEllipse;
                nextelement = element;
            }
            //remove previous
            removal(invoker, paintSurface, nextelement);
        }

        //resize shape
        public void resize(Invoker invoker, PointerRoutedEventArgs e, FrameworkElement element, Canvas paintSurface, Location location)
        {
            double ex = e.GetCurrentPoint(paintSurface).Position.X;
            double ey = e.GetCurrentPoint(paintSurface).Position.Y;
            double lw = Convert.ToDouble(element.ActualOffset.X); //set width
            double lh = Convert.ToDouble(element.ActualOffset.Y); //set height
            double w = returnSmallest(ex,lw);
            double h = returnSmallest(ey,lh);
            element.Width = w;
            element.Height = h;
        }

        //undo resize
        public void undoResize(Invoker invoker, Canvas paintSurface)
        {
            //paintSurface.Children.Clear(); //repaint
            //FrameworkElement prevelement = this.undoList.Last();
            //backelement.Width = prevelement.Width;
            //backelement.Height = prevelement.Height;
            //x = prevelement.ActualOffset.X;
            //y = prevelement.ActualOffset.Y;
            //Canvas.SetLeft(prevelement, x);
            //Canvas.SetTop(prevelement, y);
            //this.reList.Add(backelement);
            //paintSurface.Children.Clear(); //repaint
            FrameworkElement element = this.undoList.Last();
            //backelement.Width = prevelement.Width;
            //backelement.Height = prevelement.Height;
            //x = prevelement.ActualOffset.X;
            //y = prevelement.ActualOffset.Y;
            //Canvas.SetLeft(prevelement, x);
            //Canvas.SetTop(prevelement, y);
            this.redoList.Add(element);
        }

        //redo resize
        public void redoResize(Invoker invoker, Canvas paintSurface)
        {
            //paintSurface.Children.Clear(); //repaint
            //FrameworkElement prevelement = this.reList.Last();
            //backelement.Width = prevelement.Width;
            //backelement.Height = prevelement.Height;
            //x = prevelement.ActualOffset.X;
            //y = prevelement.ActualOffset.Y;
            //Canvas.SetLeft(prevelement, x);
            //Canvas.SetTop(prevelement, y);
            //this.undoList.Add(backelement);
            //paintSurface.Children.Clear(); //repaint
            FrameworkElement element = this.redoList.Last();
            this.undoList.Add(element);
        }

        //saving
        public async void saving(Canvas paintSurface)
        {

            try
            {
                string lines ="";

                foreach (FrameworkElement child in paintSurface.Children)
                {
                    if (child is Rectangle)
                    {
                        double top = (double)child.GetValue(Canvas.TopProperty);
                        double left = (double)child.GetValue(Canvas.LeftProperty);
                        string str = "rectangle " + left + " " + top + " " + child.Width + " " + child.Height + "\n";
                        lines += str;
                    }
                    else
                    {
                        double top = (double)child.GetValue(Canvas.TopProperty);
                        double left = (double)child.GetValue(Canvas.LeftProperty);
                        string str = "ellipse " + left + " " + top + " " + child.Width + " " + child.Height + "\n";
                        lines += str;
                    }
                }
                //create and write to file
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("dp2data.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(sampleFile, lines);
            }
            catch (System.IO.FileNotFoundException)
            {
                fileText = "File not found.";
            }
            catch (System.IO.FileLoadException)
            {
                fileText = "File Failed to load.";
            }
            catch (System.IO.IOException e)
            {
                fileText = "File IO error " + e;
            }
            catch (Exception err)
            {
                fileText = err.Message;
            }

        }

        //loading
        public async void loading(Canvas paintSurface)
        {
            //clear previous canvas
            paintSurface.Children.Clear();
            //read file
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile saveFile = await storageFolder.GetFileAsync("dp2data.txt");
            string text = await Windows.Storage.FileIO.ReadTextAsync(saveFile);
            //load shapes
            string[] readText = Regex.Split(text, "\\n+");
            foreach (string s in readText)
            {
                if (s.Length >2)
                {
                    string[] line = Regex.Split(s, "\\s+");
                    if (line[0] == "Ellipse")
                    {
                        this.getEllipse(s,paintSurface);
                    }
                    else
                    {
                        this.getRectangle(s,paintSurface);
                    }
                }
            }
        }

        //load ellipse
        public void getEllipse(String lines, Canvas paintSurface)
        {
            string[] line = Regex.Split(lines, "\\s+");

            double x = Convert.ToDouble(line[1]);
            double y = Convert.ToDouble(line[2]);
            double width = Convert.ToDouble(line[3]);
            double height = Convert.ToDouble(line[4]);

            Ellipse newEllipse = new Ellipse(); //instance of new ellipse shape
            newEllipse.Width = width;
            newEllipse.Height = height;
            SolidColorBrush brush = new SolidColorBrush();//brush
            brush.Color = Windows.UI.Colors.Blue;//standard brush color is blue
            newEllipse.Fill = brush;//fill color
            newEllipse.Name = "Ellipse";//attach name
            Canvas.SetLeft(newEllipse, x);//set left position
            Canvas.SetTop(newEllipse, y);//set top position
            paintSurface.Children.Add(newEllipse);
        }

        //load rectangle
        public void getRectangle(String lines,Canvas paintSurface)
        {
            string[] line = Regex.Split(lines, "\\s+");

            double x = Convert.ToDouble(line[1]);
            double y = Convert.ToDouble(line[2]);
            double width = Convert.ToDouble(line[3]);
            double height = Convert.ToDouble(line[4]);

            Rectangle newRectangle = new Rectangle(); //instance of new rectangle shape
            newRectangle.Width = width; //set width
            newRectangle.Height = height; //set height     
            SolidColorBrush brush = new SolidColorBrush(); //brush
            brush.Color = Windows.UI.Colors.Blue; //standard brush color is blue
            newRectangle.Fill = brush; //fill color
            newRectangle.Name = "Rectangle"; //attach name
            Canvas.SetLeft(newRectangle, x); //set left position
            Canvas.SetTop(newRectangle, y); //set top position 
            paintSurface.Children.Add(newRectangle);
        }

    }

}
