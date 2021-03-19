using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Input;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace tekenprogramma
{
    //interface command
    public interface ICommand
    {
        void Execute();
        void Undo();
        void Redo();
    }

    //class invoker
    public class Invoker
    {

        private List<ICommand> actionsList = new List<ICommand>();
        private List<ICommand> redoList = new List<ICommand>();

        public Invoker()
        {
            this.actionsList = new List<ICommand>();
            this.redoList = new List<ICommand>();

        }

        public void Execute(ICommand cmd)
        {
            actionsList.Add(cmd);
            redoList.Clear();
            cmd.Execute();
        }

        public void Undo()
        {
            if (actionsList.Count >= 1)
            {
                ICommand cmd = (ICommand)actionsList;
                cmd.Undo();
                actionsList.RemoveAt(0);
                redoList.Add(cmd);
            }
        }

        public void Redo()
        {
            if (redoList.Count >= 1)
            {
                ICommand cmd = (ICommand)redoList;
                cmd.Redo();
                redoList.RemoveAt(0);
                actionsList.Add(cmd);
            }
        }

    }

    //class commands
    public class Commands
    {

        private double cpx;
        private double cpy;
        private double top;
        private double left;

        Rectangle backuprectangle; //rectangle shape
        Ellipse backupellipse; //ellipse shape
        string type = "Rectangle"; //default shape
        bool moving = false;

        private List<ICommand> actionsList = new List<ICommand>();
        private List<ICommand> redoList = new List<ICommand>();

        public Invoker invoker;

        //file IO
        private List<String> lines = new List<String>();
        //string path = @"c:\temp\MyTest.txt";

        string path = Directory.GetCurrentDirectory();

        //give smallest
        public double ReturnSmallest(double first, double last)
        {
            if (first < last)
            {
                return first;
            }
            else
            {
                return last;
            }
        }

        //rectangle
        public void PlaceRectangle(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement backupprep = e.OriginalSource as FrameworkElement;
            if (backupprep.Name == "Rectangle")
            {
                Rectangle tmp = backupprep as Rectangle;
                backuprectangle = tmp;
                type = "Rectangle";
            }
            else if (backupprep.Name == "Ellipse")
            {
                Ellipse tmp = backupprep as Ellipse;
                backupellipse = tmp;
                type = "Ellipse";
            }
        }

        public void undoPlaceRectangle()
        {
            this.Undo();
        }

        public void redoPlaceRectangle()
        {
            this.Redo();
        }


        public void MakeRectangle(double left, double top, Canvas paintSurface)
        {
            
            Rectangle newRectangle = new Rectangle(); //instance of new rectangle shape
            newRectangle.Height = Math.Abs(cpy - top); //set height
            newRectangle.Width = Math.Abs(cpx - left); //set width
            SolidColorBrush brush = new SolidColorBrush(); //brush
            brush.Color = Windows.UI.Colors.Blue; //standard brush color is blue
            newRectangle.Fill = brush; //fill color
            newRectangle.Name = "Rectangle"; //attach name
            Canvas.SetLeft(newRectangle, ReturnSmallest(left, cpx)); //set left position
            Canvas.SetTop(newRectangle, ReturnSmallest(top, cpy)); //set top position 
            //newRectangle.PointerPressed += Drawing_pressed;
            paintSurface.Children.Add(newRectangle);
            //Rectangle.Content = paintSurface.Children[0].Opacity;
        }

        public void undoRectangle(Canvas paintSurface)
        {
            this.Undo();
            Rectangle newRectangle = new Rectangle();
            paintSurface.Children.Remove(newRectangle);
        }

        public void redoRectangle(double left, double top, Canvas paintSurface)
        {
            this.Redo();
            this.MakeRectangle(left, top, paintSurface);
        }

        //ellipse
        public void PlaceEllipse(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement backupprep = e.OriginalSource as FrameworkElement;
            if (backupprep.Name == "Rectangle")
            {
                Rectangle tmp = backupprep as Rectangle;
                backuprectangle = tmp;
                type = "Rectangle";
            }
            else if (backupprep.Name == "Ellipse")
            {
                Ellipse tmp = backupprep as Ellipse;
                backupellipse = tmp;
                type = "Ellipse";
            }
        }

        public void undoPlaceEllipse()
        {
            this.Undo();
        }

        public void redoPlaceEllipse()
        {
            this.Redo();
        }

        public void MakeEllipse(double left, double top, Canvas paintSurface, Invoker invoker)
        {
            
            Ellipse newEllipse = new Ellipse(); //instance of new ellipse shape
            newEllipse.Height = Math.Abs(cpy - top);//set height
            newEllipse.Width = Math.Abs(cpx - left);//set width
            SolidColorBrush brush = new SolidColorBrush();//brush
            brush.Color = Windows.UI.Colors.Blue;//standard brush color is blue
            newEllipse.Fill = brush;//fill color
            newEllipse.Name = "Ellipse";//attach name
            Canvas.SetLeft(newEllipse, ReturnSmallest(left, cpx));//set left position
            Canvas.SetTop(newEllipse, ReturnSmallest(top, cpy));//set top position
            //newEllipse.PointerPressed += Drawing_pressed;
            paintSurface.Children.Add(newEllipse);
        }

        public void undoEllipse(Canvas paintSurface)
        {
            this.Undo();
            Ellipse newEllipse = new Ellipse();
            paintSurface.Children.Remove(newEllipse);
        }

        public void redoEllipse(double left, double top, Canvas paintSurface, Invoker invoker)
        {
            this.Redo();
            this.MakeEllipse(left, top, paintSurface,invoker);
        }

        //resize
        public void Resize(object sender, PointerRoutedEventArgs e, Canvas paintSurface)
        {
            //if rectangle
            if (type == "Rectangle")
            {

                Rectangle selRect = new Rectangle();
                backuprectangle.Height = Convert.ToDouble(selRect.Height); //set width
                backuprectangle.Width = Convert.ToDouble(selRect.Width); //set height
                Rectangle activeRec = (Rectangle)e.OriginalSource; // create the link between the sender rectangle
                paintSurface.Children.Remove(activeRec); // find the rectangle and remove it from the canvas
                //paintSurface.Children.Remove(backuprectangle);
                paintSurface.Children.Add(backuprectangle);

            }
            //else if ellipse
            else if (type == "Ellipse")
            {

                Ellipse selEllipse = new Ellipse();
                backupellipse.Height = Convert.ToDouble(selEllipse.Height); //set width
                backupellipse.Width = Convert.ToDouble(selEllipse.Width); //set height
                Ellipse activeEll = (Ellipse)e.OriginalSource; // create the link between the sender ellipse
                paintSurface.Children.Remove(activeEll); // find the ellipse and remove it from the canvas
                //paintSurface.Children.Remove(backupellipse);
                paintSurface.Children.Add(backupellipse);
            }
        }

        public void undoResize(object sender, PointerRoutedEventArgs e, Canvas paintSurface)
        {
            this.Undo();
            if (type =="Rectangle")
            {
                Rectangle activeRec = (Rectangle)e.OriginalSource; // create the link between the sender rectangle
                paintSurface.Children.Remove(activeRec); // find the rectangle and remove it from the canvas
                this.MakeRectangle(left, top, paintSurface);
            }
            else if (type =="Ellipse")
            {
                Ellipse activeEll = (Ellipse)e.OriginalSource; // create the link between the sender ellipse
                paintSurface.Children.Remove(activeEll); // find the ellipse and remove it from the canvas
                this.MakeEllipse(left, top, paintSurface,invoker);
            }
            
        }

        public void redoResize(object sender, PointerRoutedEventArgs e, Canvas paintSurface)
        {
            this.Redo();
            this.Resize(sender,e,paintSurface);
        }

        //undo
        public void Undo()
        {
            int LastInList = actionsList.Count - 1;
            ICommand lastcommand = actionsList[LastInList];
            redoList.Add(lastcommand); //add to redo list
            actionsList.RemoveAt(LastInList); //remove from undo list   
        }
        //redo
        public void Redo()
        {
            int LastInList = redoList.Count - 1;
            ICommand lastcommand = redoList[LastInList]; //find last command
            actionsList.Add(lastcommand); //add to undo list
            redoList.RemoveAt(LastInList); //remove from redo list
        }

        //moving
        public void Moving(object sender, PointerRoutedEventArgs e, Canvas paintSurface)
        {

            cpx = e.GetCurrentPoint(paintSurface).Position.X;
            cpy = e.GetCurrentPoint(paintSurface).Position.Y;
            if (type == "Rectangle")
            {
                Canvas.SetLeft(backuprectangle, cpx);
                Canvas.SetTop(backuprectangle, cpy);
                Rectangle activeRec = (Rectangle)e.OriginalSource; // create the link between the sender rectangle
                paintSurface.Children.Remove(activeRec); // find the rectangle and remove it from the canvas
                //paintSurface.Children.Remove(backuprectangle);
                paintSurface.Children.Add(backuprectangle);
            }
            else if (type == "Ellipse")
            {
                Canvas.SetLeft(backupellipse, cpx);
                Canvas.SetTop(backupellipse, cpy);
                Ellipse activeEll = (Ellipse)e.OriginalSource; // create the link between the sender ellipse
                paintSurface.Children.Remove(activeEll); // find the ellipse and remove it from the canvas
                //paintSurface.Children.Remove(backupellipse);
                paintSurface.Children.Add(backupellipse);
            }
            moving = !moving;
        }

        public void undoMoving(object sender, PointerRoutedEventArgs e, Canvas paintSurface)
        {
            this.Undo();
            if (type == "Rectangle")
            {
                Rectangle activeRec = (Rectangle)e.OriginalSource; // create the link between the sender rectangle
                paintSurface.Children.Remove(activeRec); // find the rectangle and remove it from the canvas
                this.MakeRectangle(left, top, paintSurface);
            }
            else if (type == "Ellipse")
            {
                Ellipse activeEll = (Ellipse)e.OriginalSource; // create the link between the sender ellipse
                paintSurface.Children.Remove(activeEll); // find the ellipse and remove it from the canvas
                this.MakeEllipse(left, top, paintSurface,invoker);
            }
        }

        public void redoMoving(object sender, PointerRoutedEventArgs e, Canvas paintSurface)
        {
            this.Redo();
            this.Moving(sender, e, paintSurface);
        }

        //saving
        public void Saving(Canvas paintSurface)
        {   
            if (!File.Exists(path))
            {
                foreach (FrameworkElement child in paintSurface.Children)
                {
                    if (child is Rectangle)
                    {
                        double top = (double)child.GetValue(Canvas.TopProperty);
                        double left = (double)child.GetValue(Canvas.LeftProperty);
                        string str = "rectangle " + left + " " + top + " " + child.Width + " " + child.Height + "\n";
                        //string str = "rectangle " + child.Width + " " + child.Height + "\n";
                        lines.Add(str);
                    }
                    else
                    {
                        double top = (double)child.GetValue(Canvas.TopProperty);
                        double left = (double)child.GetValue(Canvas.LeftProperty);
                        string str = "ellipse " + left + " " + top + " " + child.Width + " " + child.Height + "\n";
                        //string str = "ellipse " + child.Width + " " + child.Height + "\n";
                        lines.Add(str);
                    }        
                }
                // Create a file to write to.
                File.WriteAllLines(path, lines);
            }
        }

        //loading
        public Canvas Loading(Canvas paintSurface)
        {

            string[] readText = File.ReadAllLines(path);
            foreach (string s in readText)
            {
                string[] line = Regex.Split(s, "\\s+");
                if (line[0] == "Ellipse")
                {
                    Ellipse ellipse = this.GetEllipse(s);
                    paintSurface.Children.Add(ellipse);
                }
                else
                {
                    Rectangle rectangle = this.GetRectangle(s);
                    paintSurface.Children.Add(rectangle);
                }
            }
            return paintSurface;
        }

        public Ellipse GetEllipse(String lines)
        {
            string[] line = Regex.Split(lines, "\\s+");
            Ellipse shape = new Ellipse();

            Canvas.SetLeft(shape, Convert.ToInt32(line[1]));
            Canvas.SetTop(shape, Convert.ToInt32(line[2]));
            shape.Width = Convert.ToInt32(line[3]);
            shape.Height = Convert.ToInt32(line[4]);
            
            return shape;
        }

        public Rectangle GetRectangle(String lines)
        {
            string[] line = Regex.Split(lines,"\\s+");
            Rectangle shape = new Rectangle();

            Canvas.SetLeft(shape, Convert.ToInt32(line[1]));
            Canvas.SetTop(shape, Convert.ToInt32(line[2]));
            shape.Width = Convert.ToInt32(line[3]);
            shape.Height = Convert.ToInt32(line[4]);

            return shape;
        }

        public void Selecting()
        {

        }

        public void Deselecting()
        {

        }

    }



    //class moving
    public class Moving : ICommand
    {
        private Invoker invoker;
        private Commands mycommand;
        private object sender;
        private PointerRoutedEventArgs e;
        private Canvas paintSurface;

        public Moving(Invoker invoker, object sender, PointerRoutedEventArgs e)
        {
            this.invoker = invoker;
            this.sender = sender;
            this.e = e;
        }

        public void Execute()
        {
            mycommand.Moving(sender,e,paintSurface);
        }

        public void Undo()
        {
            mycommand.undoMoving(sender, e, paintSurface);
        }

        public void Redo()
        {
            mycommand.redoMoving(sender, e, paintSurface);
        }
    }

    //class resize
    public class Resize : ICommand
    {
        private Invoker invoker;
        private Commands mycommand;
        private object sender;
        private PointerRoutedEventArgs e;
        private Canvas paintSurface;

        public Resize(Invoker invoker, object sender, PointerRoutedEventArgs e)
        {
            this.invoker = invoker;
            this.sender = sender;
            this.e = e;
        }

        public void Execute()
        {
            mycommand.Resize(sender, e, paintSurface);
        }

        public void Undo()
        {
            mycommand.undoResize(sender, e, paintSurface);
        }

        public void Redo()
        {
            mycommand.redoResize(sender, e, paintSurface);
        }
    }

    //class place rectangle
    public class PlaceRectangles : ICommand
    {
        private Invoker invoker;
        private Commands mycommand;
        private object sender;
        private PointerRoutedEventArgs e;

        public PlaceRectangles(Invoker invoker, object sender, PointerRoutedEventArgs e)
        {
            this.invoker = invoker;
            this.sender = sender;
            this.e = e;
        }

        public void Execute()
        {
            mycommand.PlaceRectangle(sender,e);
        }

        public void Undo()
        {
            mycommand.undoPlaceRectangle();
        }

        public void Redo()
        {
            mycommand.redoPlaceRectangle();
        }
    }

    //class make rectangle
    public class MakeRectangles : ICommand
    {
        private Invoker invoker;
        private Commands mycommand;
        private Canvas paintSurface;
        private double left;
        private double top;

        public MakeRectangles(double left, double top, Canvas paintSurface)
        {
            this.mycommand = mycommand;
            this.left = left;
            this.top = top;
            this.paintSurface = paintSurface;
        }

        public void Execute()
        {
            mycommand.MakeRectangle(left,top,paintSurface);
        }

        public void Undo()
        {
            mycommand.undoRectangle(paintSurface);
        }

        public void Redo()
        {
            mycommand.redoRectangle(left,top,paintSurface);
        }
    }

    //class make ellipse
    public class MakeEllipses : ICommand
    {
        private Invoker invoker;
        private Commands mycommand;
        private Canvas paintSurface;
        private double left;
        private double top;

        public MakeEllipses(Commands mycommand, double left, double top, Canvas paintSurface, Invoker invoker)
        {
            this.mycommand = mycommand;
            this.left = left;
            this.top = top;
            this.paintSurface = paintSurface;
            this.invoker = invoker;
        }

        public void Execute()
        {
            this.mycommand.MakeEllipse(this.left, this.top, this.paintSurface,this.invoker);
        }

        public void Undo()
        {
            this.mycommand.undoEllipse(this.paintSurface);
        }

        public void Redo()
        {
            this.mycommand.redoEllipse(this.left, this.top, this.paintSurface,this.invoker);
        }
    }

    //class place ellipse
    public class PlaceEllipses : ICommand
    {
        private Invoker invoker;
        private Commands mycommand;
        private object sender;
        private PointerRoutedEventArgs e;

        public PlaceEllipses(Invoker invoker, object sender, PointerRoutedEventArgs e)
        {
            this.invoker = invoker;
            this.sender = sender;
            this.e = e;
        }

        public void Execute()
        {
            mycommand.PlaceEllipse(sender,e);
        }

        public void Undo()
        {
            mycommand.undoPlaceEllipse();
        }

        public void Redo()
        {
            mycommand.redoPlaceEllipse();
        }
    }

    //class saving
    public class Saved : ICommand
    {
        private Commands mycommand;
        private Canvas paintSurface;

        public Saved(Canvas paintSurface)
        {
            this.mycommand = mycommand;
            this.paintSurface = paintSurface;
        }

        public void Execute()
        {
            mycommand.Saving(paintSurface);
        }

        public void Undo()
        {
            paintSurface.Children.Clear();
        }

        public void Redo()
        {
            paintSurface.Children.Clear();
        }
    }

    public class Loaded : ICommand
    {
        private Commands mycommand;
        private Canvas paintSurface;

        public Loaded(Canvas paintSurface)
        {
            this.mycommand = mycommand;
            this.paintSurface = paintSurface;
        }

        public void Execute()
        {
            mycommand.Loading(paintSurface);
        }

        public void Undo()
        {
            paintSurface.Children.Clear();
        }

        public void Redo()
        {
            paintSurface.Children.Clear();
        }
    }

    /*
    //class undo
    public class Undo : ICommand
    {
        private Commands mycommand;

        public Undo(Commands mycommand)
        {
            this.mycommand = mycommand;
        }

        public void Execute()
        {
            mycommand.Undo();
        }
    }

    //class redo
    public class Redo : ICommand
    {
        private Commands mycommand;

        public Redo(Commands mycommand)
        {
            this.mycommand = mycommand;
        }

        public void Execute()
        {
            mycommand.Redo();
        }
    }
    */



}
