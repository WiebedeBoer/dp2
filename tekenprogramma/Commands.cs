using System;
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
    //interface command
    public interface ICommand
    {
        void Execute();
        void Undo();
        void Redo();
    }

    //class commands
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

        //create rectangle
        public void MakeRectangle()
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
        }

        public void undoRectangle()
        {

        }

        public void redoRectangle()
        {

        }

        //create ellipse
        public void MakeEllipse()
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
        }

        public void undoEllipse()
        {

        }

        public void redoEllipse()
        {

        }

        /*
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
        */

        //resize
        public void Resize()
        {
            //if rectangle
            if (type == "Rectangle")
            {
                //double top = Canvas.GetTop(c as FrameworkElement);
                //double left = Canvas.GetLeft(c as FrameworkElement);
                //double width = (c as FrameworkElement).Width;
                //double height = (c as FrameworkElement).Height;
                Rectangle selRect = new Rectangle();
                backuprectangle.Height = Convert.ToDouble(selRect.Height); //set width
                backuprectangle.Width = Convert.ToDouble(selRect.Width); //set height

            }
            //else if ellipse
            else if (type == "Ellipse")
            {
                //double top = Canvas.GetTop(c as FrameworkElement);
                //double left = Canvas.GetLeft(c as FrameworkElement);
                //double width = (c as FrameworkElement).Width;
                //double height = (c as FrameworkElement).Height;
                Ellipse selEllipse = new Ellipse();
                backupellipse.Height = Convert.ToDouble(selEllipse.Height); //set width
                backupellipse.Width = Convert.ToDouble(selEllipse.Width); //set height

            }
        }

        public void undoResize()
        {

        }

        public void redoResize()
        {

        }

        //moving
        public void Moving()
        {
            //cpx = e.GetCurrentPoint(paintSurface).Position.X; //x coordinate canvas
            //cpy = e.GetCurrentPoint(paintSurface).Position.Y; //y coordinate canvas
            //double top = Canvas.GetTop(c as FrameworkElement);
            //double left = Canvas.GetLeft(c as FrameworkElement);
            if (type == "Rectangle")
            {
                Canvas.SetLeft(backuprectangle, left); //left
                Canvas.SetTop(backuprectangle, top); //top
                //paintSurface.Children.Remove(backuprectangle); //remove the backup
                //paintSurface.Children.Add(backuprectangle); //add the new backup shape
            }
            else if (type == "Ellipse")
            {
                Canvas.SetLeft(backupellipse, left);
                Canvas.SetTop(backupellipse, top);
                //paintSurface.Children.Remove(backupellipse); //remove the backup
                //paintSurface.Children.Add(backupellipse); //add the new backup shape
            }
            moving = !moving;
        }

        public void undoMoving()
        {

        }

        public void redoMoving()
        {

        }

        //saving
        public void Saving()
        {

        }

        //loading
        public void Loading()
        {

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


    //class moving
    public class Moving : ICommand
    {
        private Commands mycommand;

        public Moving(Commands mycommand)
        {
            this.mycommand = mycommand;
        }

        public void Execute()
        {
            mycommand.Moving();
        }

        public void Undo()
        {
            mycommand.undoMoving();
        }

        public void Redo()
        {
            mycommand.redoMoving();
        }
    }

    //class resize
    public class Resize : ICommand
    {
        private Commands mycommand;

        public Resize(Commands mycommand)
        {
            this.mycommand = mycommand;
        }

        public void Execute()
        {
            mycommand.Resize();
        }

        public void Undo()
        {
            mycommand.undoResize();
        }

        public void Redo()
        {
            mycommand.redoResize();
        }
    }

    //class make rectangle
    public class MakeRectangles : ICommand
    {
        private Commands mycommand;

        public MakeRectangles(Commands mycommand)
        {
            this.mycommand = mycommand;
        }

        public void Execute()
        {
            mycommand.MakeRectangle();
        }

        public void Undo()
        {
            mycommand.undoRectangle();
        }

        public void Redo()
        {
            mycommand.redoRectangle();
        }
    }

    //class make ellipse
    public class MakeEllipses : ICommand
    {
        private Commands mycommand;

        public MakeEllipses(Commands mycommand)
        {
            this.mycommand = mycommand;
        }

        public void Execute()
        {
            mycommand.MakeEllipse();
        }

        public void Undo()
        {
            mycommand.undoEllipse();
        }

        public void Redo()
        {
            mycommand.redoEllipse();
        }
    }

    public class Saved : ICommand
    {
        private Commands mycommand;

        public Saved(Commands mycommand)
        {
            this.mycommand = mycommand;
        }

        public void Execute()
        {
            mycommand.Saving();
        }

        public void Undo()
        {
            mycommand.Saving();
        }

        public void Redo()
        {
            mycommand.Saving();
        }
    }

    public class Loaded : ICommand
    {
        private Commands mycommand;

        public Loaded(Commands mycommand)
        {
            this.mycommand = mycommand;
        }

        public void Execute()
        {
            mycommand.Loading();
        }

        public void Undo()
        {
            mycommand.Loading();
        }

        public void Redo()
        {
            mycommand.Loading();
        }
    }
}
