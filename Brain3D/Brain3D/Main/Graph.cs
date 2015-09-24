using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    abstract class Graph : Presentation
    {
        protected static bool balanced;

        public override void Hide()
        {
            visible = false;
            display.Clear();
            Stop();
        }

        public override void Left()
        {
            display.MoveLeft();
        }

        public override void Right()
        {
            display.MoveRight();
        }

        public override void Up()
        {
            display.Up();
        }

        public override void Down()
        {
            display.Down();
        }

        public override void Closer()
        {
            display.Closer();
        }

        public override void Farther()
        {
            display.Farther();
        }

        public override void Broaden()
        {
            display.Broaden();
        }

        public override void Tighten()
        {
            display.Tighten();
        }
    }
}
