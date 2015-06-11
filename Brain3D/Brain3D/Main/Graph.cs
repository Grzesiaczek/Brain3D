using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    abstract class Graph : Presentation
    {
        protected static bool balanced;

        public override void hide()
        {
            visible = false;
            display.clear();
            stop();
        }

        public override void left()
        {
            display.left();
        }

        public override void right()
        {
            display.right();
        }

        public override void up()
        {
            display.up();
        }

        public override void down()
        {
            display.down();
        }

        public override void closer()
        {
            display.closer();
        }

        public override void farther()
        {
            display.farther();
        }

        public override void broaden()
        {
            display.broaden();
        }

        public override void tighten()
        {
            display.tighten();
        }
    }
}
