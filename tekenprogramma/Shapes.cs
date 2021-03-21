using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;

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

        // Places the shape
        public void place()
        {
            this.drawed = true;
        }

        // Removes the shape
        public void remove()
        {
            this.drawed = false;
        }
    }
    
    
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
}
