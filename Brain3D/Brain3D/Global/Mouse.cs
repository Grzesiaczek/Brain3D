using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    interface Mouse
    {
        void hover();

        void idle();

        void click(int x, int y);

        void move(int x, int y);

        void push(int x, int y);

        bool cursor(int x, int y);

        bool moved(int x, int y);
    }
}
