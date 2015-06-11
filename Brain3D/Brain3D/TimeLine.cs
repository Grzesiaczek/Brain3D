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

        public override void show()
        {
            display.show(this);
            changeFrame(frame);
            controller.changeState(frame, length);
        }

        public override void hide()
        {
            balancing.stop();
            display.clear();
        }

        public override void left()
        {
            back();
        }

        public override void right()
        {
            forth();
        }

        public override void back()
        {
            if (frame > 0)
                changeFrame(--frame);

            controller.changeFrame(frame);
        }

        public override void forth()
        {
            if(frame < length)
                changeFrame(++frame);

            controller.changeFrame(frame);
        }

        public override void broaden()
        {
            if (scale >= 2)
                return;

            rescale(scale + 0.1f);
        }

        public override void tighten()
        {
            if (scale <= 0.4f)
                return;

            rescale(scale - 0.1f);
        }

        public override void center()
        {
            frame = (int)(30 / scale);
            changeFrame(frame);
        }

        void rescale(float value)
        {
            scale = value;
            rescale();
            changeFrame(frame);
        }

        protected virtual void rescale() { }

        public override void changeFrame(int frame)
        {
            display.moveX(0.1f * scale * frame);
            this.frame = frame;
        }
    }
}
