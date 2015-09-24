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
            element.Show();
        }

        public virtual void Hide()
        {
            created = false;
            element.Hide();
        }

        public void Hover()
        {
            element.Hover();
        }

        public void activate()
        {
            element.Activate();
        }

        public void Idle()
        {
            element.Idle();
        }

        public virtual void Click(int x, int y) { }

        public void Move(int x, int y)
        {
            element.Move(x, y);
        }

        public void Push(int x, int y)
        {
            element.Push(x, y);
        }

        public bool Cursor(int x, int y)
        {
            return element.Cursor(x, y);
        }

        public bool Moved(int x, int y)
        {
            return element.Moved(x, y);
        }
    }
}
