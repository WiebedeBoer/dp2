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
using System.Windows;
using Windows.Storage;
using Windows.ApplicationModel.Activation;

namespace tekenprogramma
{
    //location class
    public class Location
    {
        public double x;
        public double y;
        public double width;
        public double height;
    }

    //shape class
    public class Shape
    {
        //variables
        public double x;
        public double y;
        public double width;
        public double height;
        //elements
        public FrameworkElement prevelement; //prev element
        public FrameworkElement nextelement; //next element
        public FrameworkElement selectedElement; //selected element
        //state 0 and 1
        //public List<FrameworkElement> doElements = new List<FrameworkElement>();
        //public List<FrameworkElement> undoElements = new List<FrameworkElement>();
        //state 2
        //public List<FrameworkElement> selectElements = new List<FrameworkElement>(); //2a
        //public List<FrameworkElement> unselectElements = new List<FrameworkElement>(); //2b
        //state 3
        //public List<FrameworkElement> movedElements = new List<FrameworkElement>(); //3a
        //public List<FrameworkElement> unmovedElements = new List<FrameworkElement>(); //3b

        //Uses the PaintSurface
        //public Canvas surface;

        //Create a list of actions to undo/redo
        //public ArrayList<ArrayList<BaseShape>> undo = new ArrayList<ArrayList<BaseShape>>();
        //public ArrayList<ArrayList<BaseShape>> redo = new ArrayList<ArrayList<BaseShape>>();

        //public List<FrameworkElement> undo = new List<FrameworkElement>();
        //public List<FrameworkElement> redo = new List<FrameworkElement>();
        //public List<FrameworkElement> drawn = new List<FrameworkElement>();
        //public List<List<FrameworkElement>> undo = new List<List<FrameworkElement>>();
        //public List<List<FrameworkElement>> redo = new List<List<FrameworkElement>>();

        //file IO error
        public string fileText { get; set; }   
        //shape creator
        public Shape(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        //
        //undo redo of create
        //

        //undo create, state 0
        public void Remove(Invoker invoker, Canvas paintSurface)
        {
            //remove previous
            prevelement = invoker.drawnElements.Last(); //1f  //err 3
            invoker.removedElements.Add(prevelement); //0+
            invoker.drawnElements.RemoveAt(invoker.drawnElements.Count() - 1); //1-
            //repaint surface
            Repaint(invoker, paintSurface, false);
        }

        //redo create, state 1
        public void Add(Invoker invoker, Canvas paintSurface)
        {
            //create next
            nextelement = invoker.removedElements.Last(); //0f
            invoker.drawnElements.Add(nextelement); //1+
            invoker.removedElements.RemoveAt(invoker.removedElements.Count() - 1); //0-
            //repaint surface
            Repaint(invoker, paintSurface, false);
        }

        //
        //creation
        //

        //create rectangle, state 1
        public void MakeRectangle(Invoker invoker, Canvas paintSurface)
        {
            Rectangle newRectangle = new Rectangle(); //instance of new rectangle shape
            newRectangle.AccessKey = invoker.executer.ToString(); //access key
            newRectangle.Width = width; //set width
            newRectangle.Height = height; //set height     
            SolidColorBrush brush = new SolidColorBrush(); //brush
            brush.Color = Windows.UI.Colors.Blue; //standard brush color is blue
            newRectangle.Fill = brush; //fill color
            newRectangle.Name = "Rectangle"; //attach name
            Canvas.SetLeft(newRectangle, x); //set left position
            Canvas.SetTop(newRectangle, y); //set top position 
            invoker.drawnElements.Add(newRectangle); //add to drawn, 1+
            //repaint surface
            Repaint(invoker, paintSurface, false); //repaint           
        }

        //create ellipse, state 1
        public void MakeEllipse(Invoker invoker, Canvas paintSurface)
        {
            Ellipse newEllipse = new Ellipse(); //instance of new ellipse shape
            newEllipse.AccessKey = invoker.executer.ToString(); //access key
            newEllipse.Width = width; //set width
            newEllipse.Height = height; //set height
            SolidColorBrush brush = new SolidColorBrush();//brush
            brush.Color = Windows.UI.Colors.Blue;//standard brush color is blue
            newEllipse.Fill = brush;//fill color
            newEllipse.Name = "Ellipse";//attach name
            Canvas.SetLeft(newEllipse, x);//set left position
            Canvas.SetTop(newEllipse, y);//set top position   
            invoker.drawnElements.Add(newEllipse); //add to drawn, 1+
            //repaint surface
            Repaint(invoker, paintSurface, false);
        }

        //
        //selecting
        //

        // Selects the shape, state 2
        public void Select(PointerRoutedEventArgs e, Invoker invoker, Canvas paintSurface)
        {
            selectedElement = e.OriginalSource as FrameworkElement;//2af
            selectedElement.Opacity = 0.6; //fill opacity
            invoker.selectElements.Add(selectedElement); //2a+
        }

        // Deselects the shape, state 1
        public void Deselect(Invoker invoker, Canvas paintSurface)
        {
            selectedElement = invoker.selectElements.Last(); //2af  //er1
            selectedElement.Opacity = 1; //fill opacity
            invoker.selectElements.RemoveAt(invoker.selectElements.Count() - 1); //2a-
            invoker.unselectElements.Add(selectedElement); //2b+
        }

        // Reselects the shape, state 2
        public void Reselect(Invoker invoker, Canvas paintSurface)
        {
            selectedElement = invoker.unselectElements.Last(); //2bf
            selectedElement.Opacity = 0.6; //fill opacity
            invoker.unselectElements.RemoveAt(invoker.unselectElements.Count() - 1); //2b-
            invoker.selectElements.Add(selectedElement); //2a+
        }

        //
        //moving, state 3
        //

        //new moving shape
        public void Moving(Invoker invoker, Canvas paintSurface, Location location, FrameworkElement clickedelement)
        {
            //remove selected element from drawn
            KeyNumber(clickedelement, invoker); //1-
            //add selected to unselect
            invoker.unselectElements.Add(clickedelement); //2b+
            //remove from selected
            invoker.selectElements.RemoveAt(invoker.selectElements.Count() -1); //2a-
            //add to moved
            invoker.movedElements.Add(clickedelement); //3a+
            //create at new location
            if (clickedelement.Name == "Rectangle")
            {
                Rectangle newRectangle = new Rectangle(); //instance of new rectangle shape
                newRectangle.AccessKey = invoker.executer.ToString();
                newRectangle.Width = location.width; //set width
                newRectangle.Height = location.height; //set height     
                SolidColorBrush brush = new SolidColorBrush(); //brush
                brush.Color = Windows.UI.Colors.Yellow; //standard brush color is blue
                newRectangle.Fill = brush; //fill color
                newRectangle.Name = "Rectangle"; //attach name
                Canvas.SetLeft(newRectangle, location.x);//set left position
                Canvas.SetTop(newRectangle, location.y); //set top position
                //add new to drawn
                invoker.drawnElements.Add(newRectangle); //1+
                //add undo
                invoker.unmovedElements.Add(newRectangle); //3b+
            }
            else if (clickedelement.Name == "Ellipse")
            {
                Ellipse newEllipse = new Ellipse(); //instance of new ellipse shape
                newEllipse.AccessKey = invoker.executer.ToString();
                newEllipse.Width = location.width;
                newEllipse.Height = location.height;
                SolidColorBrush brush = new SolidColorBrush();//brush
                brush.Color = Windows.UI.Colors.Yellow;//standard brush color is blue
                newEllipse.Fill = brush;//fill color
                newEllipse.Name = "Ellipse";//attach name
                Canvas.SetLeft(newEllipse, location.x);//set left position
                Canvas.SetTop(newEllipse, location.y);//set top position
                //add new to drawn
                invoker.drawnElements.Add(newEllipse); //1+
                //add undo
                invoker.unmovedElements.Add(newEllipse); //3b+
            }
            //repaint surface
            Repaint(invoker, paintSurface, true);
        }

        //
        //undo redo of move and resize
        //

        //move back element, state 2
        public void MoveBack(Invoker invoker, Canvas paintSurface)
        {
            //shuffle unselected
            prevelement = invoker.movedElements.Last();
            invoker.unselectElements.RemoveAt(invoker.unselectElements.Count() - 1); //2b-
            invoker.selectElements.Add(prevelement); //2a+
            //shuffle moved
            nextelement = invoker.unmovedElements.Last();
            invoker.movedElements.RemoveAt(invoker.movedElements.Count() - 1); //3a-
            invoker.unmovedElements.RemoveAt(invoker.unmovedElements.Count() - 1); //3b-
            //add redo
            invoker.undoElements.Add(nextelement); //4a+
            invoker.redoElements.Add(prevelement); //4b+
            //remove and add to drawn
            KeyNumber(nextelement, invoker); //1-
            invoker.drawnElements.Add(prevelement); //1+
            //repaint surface
            Repaint(invoker, paintSurface, false);  
        }

        //move back element, state 3
        public void MoveAgain(Invoker invoker, Canvas paintSurface)
        {
            //shuffle selected
            nextelement = invoker.undoElements.Last();           
            invoker.unselectElements.Add(nextelement); //2b+
            invoker.selectElements.RemoveAt(invoker.selectElements.Count() - 1); //2a-
            //shuffle moved
            prevelement = invoker.redoElements.Last(); 
            invoker.movedElements.Add(prevelement); //3a+
            invoker.unmovedElements.Add(nextelement); //3b+
            //undo redo
            invoker.undoElements.RemoveAt(invoker.undoElements.Count() - 1); ; //4a-
            invoker.redoElements.RemoveAt(invoker.redoElements.Count() - 1); ; //4b-
            //remove and add to drawn
            KeyNumber(prevelement, invoker); //1-
            invoker.drawnElements.Add(nextelement); //1+
            //repaint surface
            Repaint(invoker, paintSurface, false);
        }

        //
        //resizing
        //

        //resize shape, state 3
        public void Resize(Invoker invoker, PointerRoutedEventArgs e, FrameworkElement clickedelement, Canvas paintSurface, Location location)
        {
            //remove selected element from drawn
            KeyNumber(clickedelement, invoker); //1-
            //add selected to unselect
            invoker.unselectElements.Add(clickedelement); //2b+
            //remove from selected
            invoker.selectElements.RemoveAt(invoker.selectElements.Count() - 1); //2a-
            //add to moved
            invoker.movedElements.Add(clickedelement); //3a+
            //calculate size
            double ex = e.GetCurrentPoint(paintSurface).Position.X;
            double ey = e.GetCurrentPoint(paintSurface).Position.Y;
            double lw = Convert.ToDouble(clickedelement.ActualOffset.X); //set width
            double lh = Convert.ToDouble(clickedelement.ActualOffset.Y); //set height
            double w = ReturnSmallest(ex,lw);
            double h = ReturnSmallest(ey,lh);
            //create at new size
            if (clickedelement.Name == "Rectangle")
            {
                Rectangle newRectangle = new Rectangle(); //instance of new rectangle shape
                newRectangle.AccessKey = invoker.executer.ToString();
                newRectangle.Width = w; //set width
                newRectangle.Height = h; //set height     
                SolidColorBrush brush = new SolidColorBrush(); //brush
                brush.Color = Windows.UI.Colors.Yellow; //standard brush color is blue
                newRectangle.Fill = brush; //fill color
                newRectangle.Name = "Rectangle"; //attach name
                Canvas.SetLeft(newRectangle, lw); //set left position
                Canvas.SetTop(newRectangle, lh);//set top position
                //add to drawn
                invoker.drawnElements.Add(newRectangle); //1+
                //add undo
                invoker.unmovedElements.Add(newRectangle); //3b+
            }
            else if (clickedelement.Name == "Ellipse")
            {
                Ellipse newEllipse = new Ellipse(); //instance of new ellipse shape
                newEllipse.AccessKey = invoker.executer.ToString();
                newEllipse.Width = w; //set width
                newEllipse.Height = h; //set height 
                SolidColorBrush brush = new SolidColorBrush();//brush
                brush.Color = Windows.UI.Colors.Yellow;//standard brush color is blue
                newEllipse.Fill = brush;//fill color
                newEllipse.Name = "Ellipse";//attach name
                Canvas.SetLeft(newEllipse, lw);//set left position
                Canvas.SetTop(newEllipse, lh);//set top position
                //add to drawn
                invoker.drawnElements.Add(newEllipse); //1+
                //add undo
                invoker.unmovedElements.Add(newEllipse); //3b+
            }
            //repaint surface
            Repaint(invoker, paintSurface, true);
        }

        //
        //miscellaneous
        //

        //give smallest double
        public double ReturnSmallest(double first, double last)
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

        //remove selected element by access key
        public void KeyNumber(FrameworkElement selectedelement, Invoker invoker)
        {
            int inc = 0;
            int number = 0;
            foreach (FrameworkElement drawn in invoker.drawnElements)
            {
                if (drawn.AccessKey == selectedelement.AccessKey)
                {
                    number = inc;
                }
                inc++;
            }
            //remove from drawn
            invoker.drawnElements.RemoveAt(number);
            //add to removed
            //invoker.removedElements.Add(element);
            //this.undoElements.Add(element);
            //invoker.movedElements.Add(element);
        }

        //repaint surface with drawn elements
        public void Repaint(Invoker invoker, Canvas paintSurface, bool moved)
        {
            //clear surface
            paintSurface.Children.Clear();
            //redraw
            if (moved == true)
            {
                //List<FrameworkElement> redrawn = invoker.undo.ElementAt(invoker.undo.Count() - 1);
                //foreach (FrameworkElement drawelement in redrawn)
                foreach (FrameworkElement drawelement in invoker.drawnElements)
                {
                    paintSurface.Children.Add(drawelement); //add
                }
            }
            else {
                //List<FrameworkElement> redrawn = invoker.undo.ElementAt(invoker.undo.Count() - 1);
                //foreach (FrameworkElement drawelement in redrawn)
                foreach (FrameworkElement drawelement in invoker.drawnElements)
                {
                    paintSurface.Children.Add(drawelement); //add
                }
            }

        }

        //
        //saving
        //

        //saving to file
        public async void Saving(Canvas paintSurface)
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
            //file errors
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

        //
        //loading
        //

        //loading from file
        public async void Loading(Canvas paintSurface, Invoker invoker)
        {
            //clear previous canvas
            paintSurface.Children.Clear();
            //clear elements
            invoker.drawnElements.Clear();
            invoker.removedElements.Clear();
            invoker.selectElements.Clear();
            invoker.unselectElements.Clear();
            //invoker.movedElements.Clear();
            invoker.executer = 0;
            invoker.counter = 0;
            //read file
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile saveFile = await storageFolder.GetFileAsync("dp2data.txt");
            string text = await Windows.Storage.FileIO.ReadTextAsync(saveFile);
            //load shapes
            string[] readText = Regex.Split(text, "\\n+");
            int i = 0;
            //make groups and shapes
            foreach (string s in readText)
            {
                if (s.Length > 2)
                {
                    invoker.executer++;
                    i++;
                    string notabs = s.Replace("\t", "");
                    string[] line = Regex.Split(notabs, "\\s+");
                    //remake shapes
                    if (line[0] == "ellipse")
                    {
                        Shape shape = new Shape(Convert.ToDouble(line[1]), Convert.ToDouble(line[2]), Convert.ToDouble(line[3]), Convert.ToDouble(line[4]));
                        ICommand place = new MakeEllipses(shape, invoker, paintSurface);
                        invoker.Execute(place);
                    }
                    else if (line[0] == "rectangle")
                    {
                        Shape shape = new Shape(Convert.ToDouble(line[1]), Convert.ToDouble(line[2]), Convert.ToDouble(line[3]), Convert.ToDouble(line[4]));
                        ICommand place = new MakeRectangles(shape, invoker, paintSurface);
                        invoker.Execute(place);
                    }
                }
            }
        }


    }

}
