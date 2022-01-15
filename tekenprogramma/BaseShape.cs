using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tekenprogramma
{
    class BaseShape
    {
        Shape shape;
        //Constructor
        //strategy pattern
        public BaseShape(Shape shape)
        {

            this.shape = shape;
        }
    }
}
