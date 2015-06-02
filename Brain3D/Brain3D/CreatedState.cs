using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class CreatedState : CreatedElement
    {
        AnimatedState state;
        CreationHistory history;

        bool active;

        public CreatedState(AnimatedState state)
        {
            history = new CreationHistory(state);
            this.state = state;
            element = state;
        }

        public void add(CreationData data)
        {
            history.add(data);
        }

        public void setChange(float source, float target)
        {
            state.setChange(source, target);
        }

        public void setFactor(float factor)
        {
            state.setFactor(factor);
        }

        public void setValue(float value)
        {
            state.setValue(value);
        }

        public override void click(int x, int y)
        {
            if (!active)
            {
                history.show(x, y, 20);
                active = true;
            }
            else
            {
                history.hide();
                active = false;
            }
        }
    }
}