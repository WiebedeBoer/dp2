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

    //class make rectangle command
    public class MakeRectangles : ICommand
    {
        private Shape shape;
        private Invoker invoker;
        private Canvas paintSurface;

        public MakeRectangles(Shape shape, Invoker invoker, Canvas paintSurface)
        {
            this.shape = shape;
            this.invoker = invoker;
            this.paintSurface = paintSurface;
        }

        public void Execute()
        {
            this.shape.MakeRectangle(this.invoker, this.paintSurface);
        }

        public void Undo()
        {
            this.shape.Remove(this.invoker, this.paintSurface);
        }

        public void Redo()
        {
            this.shape.Add(this.invoker, this.paintSurface);
        }
    }

    //class make ellipse command
    public class MakeEllipses : ICommand
    {
        private Shape shape;
        private Invoker invoker;
        private Canvas paintSurface;

        public MakeEllipses(Shape shape, Invoker invoker, Canvas paintSurface)
        {
            this.shape = shape;
            this.invoker = invoker;
            this.paintSurface = paintSurface;
        }

        public void Execute()
        {
            this.shape.MakeEllipse(this.invoker, this.paintSurface);
        }

        public void Undo()
        {
            this.shape.Remove(this.invoker, this.paintSurface);
        }

        public void Redo()
        {
            this.shape.Add(this.invoker, this.paintSurface);
        }
    }

    //class moving command
    public class Moving : ICommand
    {
        private Shape shape;
        private Invoker invoker;
        private Canvas paintSurface;
        private FrameworkElement element;
        private Location location;

        public Moving(Shape shape, Invoker invoker, Location location, Canvas paintSurface, FrameworkElement element)
        {

            this.shape = shape;
            this.invoker = invoker;
            this.paintSurface = paintSurface;
            this.element = element;
            this.location = location;
        }

        public void Execute()
        {
            this.shape.Moving(this.invoker, this.paintSurface, this.location, this.element);
        }

        public void Undo()
        {
            this.shape.MoveBack(this.invoker, this.paintSurface);
        }

        public void Redo()
        {
            this.shape.MoveAgain(this.invoker, this.paintSurface);
        }
    }

    //class resize command
    public class Resize : ICommand
    {
        private Shape shape;
        private Invoker invoker;
        private Canvas paintSurface;
        private FrameworkElement element;
        private Location location;
        private PointerRoutedEventArgs e;

        public Resize(Shape shape, Invoker invoker, PointerRoutedEventArgs e, Location location, Canvas paintSurface, FrameworkElement element)
        {
            this.shape = shape;
            this.invoker = invoker;
            this.paintSurface = paintSurface;
            this.element = element;
            this.location = location;
            this.e = e;
        }

        public void Execute()
        {
            this.shape.Resize(this.invoker, this.e, this.element,this.paintSurface, this.location);
        }

        public void Undo()
        {
            this.shape.MoveBack(this.invoker, this.paintSurface);
        }

        public void Redo()
        {
            this.shape.MoveAgain(this.invoker, this.paintSurface);
        }
    }

    //class select command
    public class Select : ICommand
    {
        private Shape shape; 
        private PointerRoutedEventArgs e;
        private Invoker invoker;
        private Canvas paintSurface;
        

        public Select(Shape shape, PointerRoutedEventArgs e, Invoker invoker, Canvas paintSurface)
        {
            this.e = e;
            this.shape = shape;
            this.paintSurface = paintSurface;
            this.invoker = invoker;
        }

        public void Execute()
        {
            this.shape.Select(this.e, this.invoker, this.paintSurface);
        }

        public void Undo()
        {
            this.shape.Deselect(this.invoker, this.paintSurface);
        }

        public void Redo()
        {
            this.shape.Reselect(this.invoker, this.paintSurface);
        }
    }

    //class saving command
    public class Saved : ICommand
    {
        private Shape mycommand;
        private Canvas paintSurface;

        public Saved(Shape mycommand, Canvas paintSurface)
        {
            this.mycommand = mycommand;
            this.paintSurface = paintSurface;
        }

        public void Execute()
        {
            this.mycommand.Saving(this.paintSurface);
        }

        public void Undo()
        {
            //placeholder
        }

        public void Redo()
        {
            //placeholder
        }
    }

    //class load command
    public class Loaded : ICommand
    {
        private Shape mycommand;
        private Canvas paintSurface;
        private Invoker invoker;

        public Loaded(Shape mycommand, Canvas paintSurface, Invoker invoker)
        {
            this.mycommand = mycommand;
            this.paintSurface = paintSurface;
            this.invoker = invoker;
        }

        public void Execute()
        {
            this.mycommand.Loading(this.paintSurface,this.invoker);
        }

        public void Undo()
        {
            //placeholder
        }

        public void Redo()
        {
            //placeholder
        }
    }

}
