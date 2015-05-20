using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    abstract class CreatedElement
    {
        protected AnimatedElement element;

        public virtual void show()
        {
            element.show();
        }

        public void hide()
        {
            element.hide();
        }
    }
}
