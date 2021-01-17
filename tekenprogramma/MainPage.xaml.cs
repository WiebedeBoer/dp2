using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Input;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace tekenprogramma
{

    //main
    public sealed partial class MainPage : Page
    {
        //mainpage class variables
        string type = "Rectangle"; //default shape
        double cpx;
        double cpy;
        bool firstcp = true;
        bool moving = false;
        Rectangle backuprectangle; //rectangle shape
        Ellipse backupellipse; //ellipse shape
        //string actionType ="create"; //default action
        private List<ICommand> actionsList = new List<ICommand>();
        private List<ICommand> redoList = new List<ICommand>();
      
        public MainPage()
        {
            InitializeComponent();
        }

        //pressing on canvas
        private void Drawing_pressed(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement backupprep = e.OriginalSource as FrameworkElement;
            //if shape is rectangle
            if (backupprep.Name == "Rectangle")
            {
                Rectangle tmp = backupprep as Rectangle;
                backuprectangle = tmp;
                type = "Rectangle";
            }
            //else if shape is ellipse
            else if(backupprep.Name == "Ellipse")
            {
                Ellipse tmp = backupprep as Ellipse;
                backupellipse = tmp;
                type = "Ellipse";
            }
            //if move
            if (moving)
            {
                cpx = e.GetCurrentPoint(paintSurface).Position.X; //x coordinate canvas
                cpy = e.GetCurrentPoint(paintSurface).Position.Y; //y coordinate canvas
                if (type == "Rectangle")
                {
                    //Canvas.SetLeft(backuprectangle, cpx); //left
                    //Canvas.SetTop(backuprectangle, cpy); //top
                    Commands maker = new Commands();
                    Moving moving = new Moving(maker);
                    Receiver receiver = new Receiver();
                    receiver.takeOrder(moving);
                    paintSurface.Children.Remove(backuprectangle); //remove the backup
                    paintSurface.Children.Add(backuprectangle); //add the new backup shape
                    actionsList.Add(moving); //add command to list
                }
                else if(type == "Ellipse")
                {
                    //Canvas.SetLeft(backupellipse, cpx);
                    //Canvas.SetTop(backupellipse, cpy);
                    Commands maker = new Commands();
                    Moving moving = new Moving(maker);
                    Receiver receiver = new Receiver();
                    receiver.takeOrder(moving);
                    paintSurface.Children.Remove(backupellipse); //remove the backup
                    paintSurface.Children.Add(backupellipse); //add the new backup shape
                    actionsList.Add(moving); //add command to list
                }
                moving = !moving;
            }
            //else create
            else
            {
                if (firstcp)
                {
                    cpx = e.GetCurrentPoint(paintSurface).Position.X; //left position
                    cpy = e.GetCurrentPoint(paintSurface).Position.Y; //top position
                }
                else
                {
                    if (type == "Rectangle")
                    {
                        MakeRectangle(e.GetCurrentPoint(paintSurface).Position.X, e.GetCurrentPoint(paintSurface).Position.Y); //create rectangle
                    }
                    else
                    {
                        MakeEllipse(e.GetCurrentPoint(paintSurface).Position.X, e.GetCurrentPoint(paintSurface).Position.Y); //create ellipse
                    }
                }
                firstcp = !firstcp;
            }
        }

        //make a rectangle
        public void MakeRectangle(double left, double top)
        {
            Commands maker = new Commands();
            MakeRectangle makerec = new MakeRectangle(maker);
            Rectangle newRectangle = new Rectangle(); //instance of new rectangle shape
            Receiver receiver = new Receiver();          
            receiver.takeOrder(makerec);           
            newRectangle.PointerPressed += Drawing_pressed;
            paintSurface.Children.Add(newRectangle); //add shape to canvas
            actionsList.Add(makerec); //add command to list
        }

        //make an ellipse
        public void MakeEllipse(double left, double top)
        {
            Commands maker = new Commands();
            MakeEllipse makeelip = new MakeEllipse(maker);
            Ellipse newEllipse = new Ellipse(); //instance of new ellipse shape
            Receiver receiver = new Receiver();
            receiver.takeOrder(makeelip);           
            newEllipse.PointerPressed += Drawing_pressed;
            paintSurface.Children.Add(newEllipse); //add shape to canvas    
            actionsList.Add(makeelip); //add command to list
        }

        //moving shape
        private void Move_Click(object sender, RoutedEventArgs e)
        {
            moving = !moving;
        }

        //resize shape
        private void Resize_Click(object sender, RoutedEventArgs e)
        {
            //if rectangle
            if (type == "Rectangle")
            {
                paintSurface.Children.Remove(backuprectangle);
                //backuprectangle.Height = Convert.ToDouble(Height.Text); //set width
                //backuprectangle.Width = Convert.ToDouble(Width.Text); //set height
                Commands maker = new Commands();
                Resize doresize = new Resize(maker);
                //Rectangle backuprectangle = new Rectangle(); //instance of resized shape
                Receiver receiver = new Receiver();
                receiver.takeOrder(doresize);
                paintSurface.Children.Add(backuprectangle); //add to canvas
                actionsList.Add(doresize); //add command to list
            }
            //else if ellipse
            else if (type == "Ellipse")
            {
                paintSurface.Children.Remove(backupellipse);
                //backupellipse.Height = Convert.ToDouble(Height.Text); //set width
                //backupellipse.Width = Convert.ToDouble(Width.Text); //set height
                Commands maker = new Commands();
                Resize doresize = new Resize(maker);
                //Ellipse backupellipse = new Ellipse(); //instance of resized shape
                Receiver receiver = new Receiver();
                receiver.takeOrder(doresize);
                paintSurface.Children.Add(backupellipse); //add to canvas
                actionsList.Add(doresize); //add command to list
            }
        }

        //first or last click
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
            type = button.Name; //fetch button name to put into type
        }

        //rectangle
        private void Rectangle_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name; //fetch button name to put into type
        }

        //ornament
        private void Ornament_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name; //fetch button name to put into type
        }

        //group
        private void Group_Click(object sender, RoutedEventArgs e)
        {

        }

        //undo
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
           int LastInList = actionsList.Count - 1;
           ICommand lastcommand = actionsList[LastInList];
            
            Commands maker = new Commands();
            Undo undid = new Undo(maker);
            Receiver receiver = new Receiver();
            receiver.takeOrder(undid);

            redoList.Add(lastcommand); //add to redo list
            actionsList.RemoveAt(LastInList); //remove from undo list                           
        }

        //redo
        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            int LastInList = redoList.Count - 1;
            ICommand lastcommand = redoList[LastInList]; //find last command

            Commands maker = new Commands();
            Redo redid = new Redo(maker);
            Receiver receiver = new Receiver();
            receiver.takeOrder(redid);

            actionsList.Add(lastcommand); //add to undo list
            redoList.RemoveAt(LastInList); //remove from redo list
        }

        //save
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string[] lines;
            var list = new List<string>();
            var fileStream = new FileStream("file.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    foreach (var c in paintSurface.Children.OfType<Rectangle>())
                    {
                        double top = Canvas.GetTop(c as FrameworkElement);
                        double left = Canvas.GetLeft(c as FrameworkElement);
                        double width = (c as FrameworkElement).Width;
                        double height = (c as FrameworkElement).Height;
                        line = "rectangle" + top +" "+ left + " " + width + " " + height;
                        list.Add(line);
                    }

                    foreach (var c in paintSurface.Children.OfType<Ellipse>())
                    {
                        double top = Canvas.GetTop(c as FrameworkElement);
                        double left = Canvas.GetLeft(c as FrameworkElement);
                        double width = (c as FrameworkElement).Width;
                        double height = (c as FrameworkElement).Height;
                        line = "ellipse" + top + " " + left + " " + width + " " + height;
                        list.Add(line);
                    }
                }
            }
            lines = list.ToArray();
        }

        //load
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            string[] lines;
            var list = new List<string>();
            var fileStream = new FileStream("file.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    list.Add(line);
                    //if rectangle
                    if (line.Contains("rectangle"))
                    {
                        string[] words = line.Split(' ');
                        double top = Convert.ToDouble(words[1]);
                        double left = Convert.ToDouble(words[2]);
                        double width = Convert.ToDouble(words[3]);
                        double height = Convert.ToDouble(words[4]);
                        //backuprectangle.Top = Convert.ToDouble(top); //set top
                        //backuprectangle.Left = Convert.ToDouble(left); //set left
                        Canvas.SetLeft(backuprectangle, left); //left
                        Canvas.SetTop(backuprectangle, top); //top
                        backuprectangle.Height = Convert.ToDouble(height); //set width
                        backuprectangle.Width = Convert.ToDouble(width); //set height
                        paintSurface.Children.Add(backuprectangle); //add to canvas
                    }
                    //else if ellipse
                    else if (line.Contains("ellipse"))
                    {
                        string[] words = line.Split(' ');
                        double top = Convert.ToDouble(words[1]);
                        double left = Convert.ToDouble(words[2]);
                        double width = Convert.ToDouble(words[3]);
                        double height = Convert.ToDouble(words[4]);
                        //backupellipse.Top = Convert.ToDouble(top); //set top
                        //backupellipse.Left = Convert.ToDouble(left); //set left
                        Canvas.SetLeft(backupellipse, left); //left
                        Canvas.SetTop(backupellipse, top); //top
                        backupellipse.Height = Convert.ToDouble(height); //set width
                        backupellipse.Width = Convert.ToDouble(width); //set height
                        paintSurface.Children.Add(backupellipse); //add to canvas
                    }
                }
            }
            lines = list.ToArray();
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