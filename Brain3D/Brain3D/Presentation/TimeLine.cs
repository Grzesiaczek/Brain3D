﻿using System;

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
            controller.Show(this);
            visible = true;
        }

        public override void Hide()
        {
            balancing.Stop();
            display.Clear();
            visible = false;
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
            {
                ChangeFrame(--frame);
            }

            controller.ChangeFrame(frame);
        }

        public override void Forth()
        {
            if (frame < length)
            {
                ChangeFrame(++frame);
            }

            controller.ChangeFrame(frame);
        }

        public override void Broaden()
        {
            if (scale < 2)
            {
                Rescale(scale + 0.1f);
            }
        }

        public override void Tighten()
        {
            if (scale > 0.3f)
            {
                Rescale(scale - 0.1f);
            }
        }

        public override void Center()
        {
            frame = (int)(30 / scale);
            ChangeFrame(frame);
        }

        void Rescale(float value)
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
