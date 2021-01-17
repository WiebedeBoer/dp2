using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tekenprogramma
{

    public class Shape
    {
        public void Create()
        {


        }
    }

    //public class MouseBinding : System.Windows.Input.InputBinding;
    public class CreateRectangle
    {
        private Shape rectangle;
        public int leftCoord;
        public int topCoord;
        public int rightCoord;
        public int bottomCoord;
        //private string actionType;
        public string actionType { get; set; }

        public CreateRectangle()
        {
            actionType = actionType;
        }


        public void execute()
        {
            rectangle.Create();
        }

        public void OnPointerPressed()
        {
            this.actionType = "selected";
        }
    }

    public class CreateElipse
    {
        private Shape elipse;
        private int leftCoord;
        private int topCoord;
        private int rightCoord;
        private int bottomCoord;
        //private string actionType;
        public string actionType { get; set; }

        public CreateElipse(Shape elipse)
        {
            this.elipse = elipse;
        }

        public void execute()
        {
            elipse.Create();
        }

        public void OnPointerPressed()
        {
            this.actionType = "selected";
        }
    }

    class CreateShape
    {
    }

}
