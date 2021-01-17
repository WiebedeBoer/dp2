using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tekenprogramma
{
    public class Receiver
    {

        private List<ICommand> action = new List<ICommand>();

        public void takeOrder(ICommand order)
        {
            action.Add(order);
        }

        public void placeOrders()
        {

            foreach (ICommand order in action)
            {
                order.Execute();
            }
            action.Clear();
        }
    }

    //private string action;

    //public Receiver(string action)
    //{
    //    this.action = action;
    //}

    //public void Actions(string action)
    //{
    //this.action = action;
    //if (action == "create rectangle")
    //{
    //MakeRectangle.Execute();
    //}
    //else if(action =="create elipse"){
    //MakeEllipse.Execute();
    //}            
    //}


}
