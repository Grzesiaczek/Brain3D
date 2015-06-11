using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    abstract class CreatedElement : Mouse
    {
        protected AnimatedElement element;
        protected bool created;

        public virtual void show()
        {
            element.show();
        }

        public virtual void hide()
        {
            created = false;
            element.hide();
        }

        public void hover()
        {
            element.hover();
        }

        public void activate()
        {
            element.activate();
        }

        public void idle()
        {
            element.idle();
        }

        public virtual void click(int x, int y) { }

        public void move(int x, int y)
        {
            element.move(x, y);
        }

        public void push(int x, int y)
        {
            element.push(x, y);
        }

        public bool cursor(int x, int y)
        {
            return element.cursor(x, y);
        }

        public bool moved(int x, int y)
        {
            return element.moved(x, y);
        }
    }
}
