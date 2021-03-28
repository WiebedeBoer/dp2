using System;
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

namespace tekenprogramma
{
    
    public class Shape
    {
        private double x;
        private double y;
        private double width;
        private double height;
        private bool selected;
        private bool drawed;

        private List<ICommand> actionsList = new List<ICommand>();
        private List<ICommand> redoList = new List<ICommand>();

        private List<FrameworkElement> undoList = new List<FrameworkElement>();
        private List<FrameworkElement> reList = new List<FrameworkElement>();

        public Invoker invoker;
        public Canvas paintSurface;
        public PointerRoutedEventArgs pet;

        Rectangle backuprectangle; //rectangle shape
        Ellipse backupellipse; //ellipse shape
        string type = "Rectangle"; //default shape
        bool moved = false; //moving

        //file IO
        private List<String> lines = new List<String>();
        //string path = @"c:\temp\MyTest.txt";

        string path = Directory.GetCurrentDirectory();

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
            this.selected = true;
        }

        // Deselects the shape
        public void deselect(PointerRoutedEventArgs e)
        {
            this.selected = false;
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

        //make rectangle
        public void makeRectangle(Invoker invoker, Canvas paintSurface)
        {
            this.drawed = false;
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
            paintSurface.Children.Add(newRectangle);
        }

        //make ellipse
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
            paintSurface.Children.Add(newEllipse);
        }

        //undo
        public void remove(Invoker invoker, Canvas paintSurface)
        {
            paintSurface.Children.Clear();
            this.drawed = false;
        }

        //moving shape
        public void moving(PointerRoutedEventArgs e, FrameworkElement element, Canvas paintSurface)
        { 
            x = e.GetCurrentPoint(paintSurface).Position.X;
            y = e.GetCurrentPoint(paintSurface).Position.Y;
            Canvas.SetLeft(element, x);
            Canvas.SetTop(element, y);
            this.undoList.Add(element);
        }

        public void undoMoving(Canvas paintSurface)
        {
            FrameworkElement element = this.undoList.Last();
            x = element.ActualOffset.X;
            y = element.ActualOffset.Y;
            Canvas.SetLeft(element, x);
            Canvas.SetTop(element, y);
            this.reList.Add(element);
        }

        public void redoMoving(Canvas paintSurface)
        {
            FrameworkElement element = this.reList.Last();
            x = element.ActualOffset.X;
            y = element.ActualOffset.Y;
            Canvas.SetLeft(element, x);
            Canvas.SetTop(element, y);
            this.undoList.Add(element);
        }

        //resize shape
        public void resize(PointerRoutedEventArgs e, FrameworkElement element, Canvas paintSurface)
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

        public void undoResize(Canvas paintSurface)
        {

        }

        public void redoResize(Canvas paintSurface)
        {

        }

        //saving
        public void saving(Canvas paintSurface)
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
        public void loading(Canvas paintSurface)
        {
            paintSurface.Children.Clear();
            string[] readText = File.ReadAllLines(path);
            foreach (string s in readText)
            {
                string[] line = Regex.Split(s, "\\s+");
                if (line[0] == "Ellipse")
                {
                    Ellipse ellipse = this.getEllipse(s);
                    paintSurface.Children.Add(ellipse);
                }
                else
                {
                    Rectangle rectangle = this.getRectangle(s);
                    paintSurface.Children.Add(rectangle);
                }
            }
            //return paintSurface;
        }

        public Ellipse getEllipse(String lines)
        {
            string[] line = Regex.Split(lines, "\\s+");
            Ellipse shape = new Ellipse();

            Canvas.SetLeft(shape, Convert.ToInt32(line[1]));
            Canvas.SetTop(shape, Convert.ToInt32(line[2]));
            shape.Width = Convert.ToInt32(line[3]);
            shape.Height = Convert.ToInt32(line[4]);

            return shape;
        }

        public Rectangle getRectangle(String lines)
        {
            string[] line = Regex.Split(lines, "\\s+");
            Rectangle shape = new Rectangle();

            Canvas.SetLeft(shape, Convert.ToInt32(line[1]));
            Canvas.SetTop(shape, Convert.ToInt32(line[2]));
            shape.Width = Convert.ToInt32(line[3]);
            shape.Height = Convert.ToInt32(line[4]);

            return shape;
        }



        //newEllipse.Height = Math.Abs(y - top);//set height
        //newEllipse.Width = Math.Abs(x - left);//set width
        //Canvas.SetLeft(newEllipse, returnSmallest(left, x));//set left position
        //Canvas.SetTop(newEllipse, returnSmallest(top, y));//set top position
        //newEllipse.PointerPressed += Drawing_pressed;
        //invoker.shapesList.Add(shape);
        //Rectangle.Content = paintSurface.Children[0].Opacity;

        //Shape shape = new Shape(this.x, this.y, this.width, this.height);

        ////if rectangle
        //if (type == "Rectangle")
        //{

        //    //Rectangle selRect = new Rectangle();
        //    //backuprectangle.Height = Convert.ToDouble(selRect.Height); //set width
        //    //backuprectangle.Width = Convert.ToDouble(selRect.Width); //set height


        //    //backuprectangle.Height = Convert.ToDouble(selRect.Height); //set width
        //    //backuprectangle.Width = Convert.ToDouble(selRect.Width); //set height
        //    Rectangle activeRec = (Rectangle)e.OriginalSource; // create the link between the sender rectangle
        //    paintSurface.Children.Remove(activeRec); // find the rectangle and remove it from the canvas
        //    //paintSurface.Children.Remove(backuprectangle);
        //    paintSurface.Children.Add(backuprectangle);

        //}
        ////else if ellipse
        //else if (type == "Ellipse")
        //{

        //    Ellipse selEllipse = new Ellipse();
        //    backupellipse.Height = Convert.ToDouble(selEllipse.Height); //set width
        //    backupellipse.Width = Convert.ToDouble(selEllipse.Width); //set height
        //    Ellipse activeEll = (Ellipse)e.OriginalSource; // create the link between the sender ellipse
        //    paintSurface.Children.Remove(activeEll); // find the ellipse and remove it from the canvas
        //    //paintSurface.Children.Remove(backupellipse);
        //    paintSurface.Children.Add(backupellipse);
        //}

        //if (element.Name == "Rectangle")
        //{


        //}
        //else if (element.Name == "Ellipse")
        //{
        //    Canvas.SetLeft(element, x);
        //    Canvas.SetTop(element, y);

        //}

        //Rectangle activeRec = (Rectangle)e.OriginalSource; // create the link between the sender rectangle
        //paintSurface.Children.Remove(activeRec); // find the rectangle and remove it from the canvas
        //paintSurface.Children.Remove(backuprectangle);
        //paintSurface.Children.Add(backuprectangle);
        //Ellipse activeEll = (Ellipse)e.OriginalSource; // create the link between the sender ellipse
        //paintSurface.Children.Remove(activeEll); // find the ellipse and remove it from the canvas
        //paintSurface.Children.Remove(backupellipse);
        //paintSurface.Children.Add(backupellipse);

        /*
        public void undoMoving(PointerRoutedEventArgs e)
        {
            this.undo();
            if (type == "Rectangle")
            {
                Rectangle activeRec = (Rectangle)e.OriginalSource; // create the link between the sender rectangle
                paintSurface.Children.Remove(activeRec); // find the rectangle and remove it from the canvas
                this.makeRectangle(invoker, paintSurface);
            }
            else if (type == "Ellipse")
            {
                Ellipse activeEll = (Ellipse)e.OriginalSource; // create the link between the sender ellipse
                paintSurface.Children.Remove(activeEll); // find the ellipse and remove it from the canvas
                this.makeEllipse(invoker, paintSurface);
            }
        }        

        public void redoMoving(PointerRoutedEventArgs e)
        {
            this.redo();
            this.moving(e);
        }
        */

        ////undo
        //public void undo()
        //{
        //    int LastInList = actionsList.Count - 1;
        //    ICommand lastcommand = actionsList[LastInList];
        //    redoList.Add(lastcommand); //add to redo list
        //    actionsList.RemoveAt(LastInList); //remove from undo list   
        //}
        ////redo
        //public void redo()
        //{
        //    int LastInList = redoList.Count - 1;
        //    ICommand lastcommand = redoList[LastInList]; //find last command
        //    actionsList.Add(lastcommand); //add to undo list
        //    redoList.RemoveAt(LastInList); //remove from redo list
        //}

        /*
        public void undoResize(PointerRoutedEventArgs e)
        {
            this.undo();
            if (type == "Rectangle")
            {
                Rectangle activeRec = (Rectangle)e.OriginalSource; // create the link between the sender rectangle
                paintSurface.Children.Remove(activeRec); // find the rectangle and remove it from the canvas
                this.makeRectangle(invoker, paintSurface);
            }
            else if (type == "Ellipse")
            {
                Ellipse activeEll = (Ellipse)e.OriginalSource; // create the link between the sender ellipse
                paintSurface.Children.Remove(activeEll); // find the ellipse and remove it from the canvas
                this.makeEllipse(invoker, paintSurface);
            }
        }
        

        public void redoResize(PointerRoutedEventArgs e)
        {
            this.redo();
            this.resize(e);
        }
        */

        /*
        // Places the shape
        public void place(Canvas paintsurface, Invoker invoker)
        {
            this.invoker = invoker;
            this.paintSurface = paintSurface;
            this.drawed = true;

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
        */

        // Removes the shape
        //public void removeRectangle(Invoker invoker, Canvas paintSurface)
        //{
        //    //Rectangle activeRec = (Rectangle)e.OriginalSource; // create the link between the sender rectangle
        //    //Rectangle activeRec = (Rectangle)pet.OriginalSource;
        //    //Rectangle activeRec = invoker;
        //    //invoker = invoker;
        //    //List<Shape> removelist = invoker.shapesList;
        //    List<ICommand> removelist = invoker.actionsList;
        //    //removelist.RemoveAt(0);
        //    //ICommand cmd = removelist.First();
        //    //Rectangle activeRec
        //    //paintSurface.Children.Remove(cmd);
        //    //Shape cmd = removelist.First();
        //    //Shape shape = cmd.shape;
        //    //paintSurface = paintSurface;

        //    //paintSurface.Children.Add(newEllipse);
        //    //paintSurface.Children.Remove(activeRec);
        //    //paintSurface.Children.Remove(backuprectangle);
        //    paintSurface.Children.Clear();
        //    //this.reshape(paintSurface,removelist);
        //    this.drawed = false;
        //}

        //public void reshape(Canvas paintSurface, List<ICommand> removeList)
        //{
        //    foreach (ICommand cmd in removeList)
        //    {
        //        cmd.Execute();
        //    }

        //    paintSurface.Children.Clear();
        //    //List<Shape> removelist = removeList;
        //    //foreach (Shape shape in removeList)
        //    //{
        //    //    //Shape shape = (Shape)command;
        //    //    if (shape.type == "Rectangle")
        //    //    {
        //    //        Rectangle newShape = new Rectangle();
        //    //        newShape.Width = width; //set width
        //    //        newShape.Height = height; //set height  
        //    //        Canvas.SetLeft(newShape, shape.x); //set left position
        //    //        Canvas.SetTop(newShape, shape.y); //set top position 
        //    //        paintSurface.Children.Add(newShape);

        //    //    }
        //    //    else if (shape.type == "Ellipse")
        //    //    {
        //    //        Rectangle newShape = new Rectangle();
        //    //        newShape.Width = width; //set width
        //    //        newShape.Height = height; //set height  
        //    //        Canvas.SetLeft(newShape, shape.x); //set left position
        //    //        Canvas.SetTop(newShape, shape.y); //set top position 
        //    //        paintSurface.Children.Add(newShape);
        //    //    }
        //    //}
        //    //return paintSurface;
        //}


        //public void removeEllipse(Invoker invoker, Canvas paintSurface)
        //{
        //    //Ellipse activeEll = (Ellipse)e.OriginalSource; // create the link between the sender ellipse
        //    //paintSurface.Children.Remove(activeEll);
        //    //paintSurface.Children.Remove(backupellipse);
        //    paintSurface.Children.Clear();
        //    this.drawed = false;
        //}


    }

    /*
    public class MakeRectangle
    {
        public MakeRectangle()
        {
        }
    }

    public class MakeEllipse
    {
        public MakeEllipse()
        {
        }
    }
    */
}
