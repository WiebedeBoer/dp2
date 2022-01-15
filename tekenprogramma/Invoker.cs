using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace tekenprogramma
{
    //class invoker
    public class Invoker
    {
        public List<ICommand> actionsList = new List<ICommand>();
        public List<ICommand> redoList = new List<ICommand>();
        //state 0
        public List<FrameworkElement> removedElements = new List<FrameworkElement>(); //0
        //state 1
        public List<FrameworkElement> drawnElements = new List<FrameworkElement>(); //1
        //state 2
        public List<FrameworkElement> selectElements = new List<FrameworkElement>(); //2a
        public List<FrameworkElement> unselectElements = new List<FrameworkElement>(); //2b
        //state 3
        public List<FrameworkElement> movedElements = new List<FrameworkElement>(); //3a
        public List<FrameworkElement> unmovedElements = new List<FrameworkElement>(); //3b
        //state 4
        public List<FrameworkElement> undoElements = new List<FrameworkElement>(); //4a
        public List<FrameworkElement> redoElements = new List<FrameworkElement>(); //4b
        //undo redo shapes
        //public List<List<FrameworkElement>> undo = new List<List<FrameworkElement>>();
        //public List<List<FrameworkElement>> redo = new List<List<FrameworkElement>>();

        public int counter = 0;
        public int executer = 0;

        public Invoker()
        {
            this.actionsList = new List<ICommand>();
            this.redoList = new List<ICommand>();
        }

        //execute
        public void Execute(ICommand cmd)
        {
            actionsList.Add(cmd);
            redoList.Clear();
            cmd.Execute();
            counter++;
            executer++;
        }

        //undo
        public void Undo()
        {
            if (actionsList.Count >= 1)
            {
                ICommand cmd = actionsList.Last();
                actionsList.RemoveAt(actionsList.Count - 1);
                redoList.Add(cmd);
                cmd.Undo();
                counter--;
            }
        }

        //redo
        public void Redo()
        {
            if (redoList.Count >= 1)
            {
                ICommand cmd = redoList.Last();
                actionsList.Add(cmd);
                redoList.RemoveAt(redoList.Count - 1);
                cmd.Redo();
                counter++;
            }
        }

        ////repaint
        //public void Repaint()
        //{
        //    //repaint actions
        //    foreach (ICommand icmd in actionsList)
        //    {
        //        icmd.Execute();
        //    }
        //}

        ////clear
        //public void Clear()
        //{
        //    actionsList.Clear();
        //}

    }
}
