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

        public List<FrameworkElement> prev = new List<FrameworkElement>();
        public List<FrameworkElement> next = new List<FrameworkElement>();

        public int counter = 0;

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
            }
        }

        //repaint
        public void Repaint()
        {
            //repaint actions
            foreach (ICommand icmd in actionsList)
            {
                icmd.Execute();
            }
        }

        //clear
        public void Clear()
        {
            actionsList.Clear();
        }

    }
}
