using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    abstract class TimeLine : Presentation
    {
        protected int frame;
        protected float scale;

        protected TimeLine()
        {
            frame = 30;
            scale = 1;
        }

        public override void Show()
        {
            display.Show(this);
            ChangeFrame(frame);
            controller.ChangeState(frame, length);
        }

        public override void Hide()
        {
            balancing.Stop();
            display.Clear();
        }

        public override void Left()
        {
            Back();
        }

        public override void Right()
        {
            Forth();
        }

        public override void Back()
        {
            if (frame > 0)
                ChangeFrame(--frame);

            controller.ChangeFrame(frame);
        }

        public override void Forth()
        {
            if(frame < length)
                ChangeFrame(++frame);

            controller.ChangeFrame(frame);
        }

        public override void Broaden()
        {
            if (scale >= 2)
                return;

            rescale(scale + 0.1f);
        }

        public override void Tighten()
        {
            if (scale <= 0.3f)
                return;

            rescale(scale - 0.1f);
        }

        public override void Center()
        {
            frame = (int)(30 / scale);
            ChangeFrame(frame);
        }

        void rescale(float value)
        {
            scale = value;
            Rescale();
            ChangeFrame(frame);
        }

        protected virtual void Rescale() { }

        public override void ChangeFrame(int frame)
        {
            display.MoveX(0.1f * scale * frame);
            this.frame = frame;
        }
    }
}
