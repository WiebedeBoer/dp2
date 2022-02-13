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
        //state 0, remove
        public List<FrameworkElement> removedElements = new List<FrameworkElement>(); //0
        //state 1, draw
        public List<FrameworkElement> drawnElements = new List<FrameworkElement>(); //1
        //state 2, select
        public List<FrameworkElement> selectElements = new List<FrameworkElement>(); //2a
        public List<FrameworkElement> unselectElements = new List<FrameworkElement>(); //2b
        //state 3, move
        public List<FrameworkElement> movedElements = new List<FrameworkElement>(); //3a
        public List<FrameworkElement> unmovedElements = new List<FrameworkElement>(); //3b
        //state 4, undo redo
        public List<FrameworkElement> undoElements = new List<FrameworkElement>(); //4a
        public List<FrameworkElement> redoElements = new List<FrameworkElement>(); //4b

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

    }
}
