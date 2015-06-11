using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class CreationHistory : GraphicsElement
    {
        List<CreationData> history;
        AnimatedSynapse state;
        Vector3 position;

        Rect background;
        Rect border;

        public CreationHistory(AnimatedSynapse state)
        {
            history = new List<CreationData>();
            this.state = state;
        }

        public void add(CreationData data)
        {
            history.Add(data);
        }

        public void show(int x, int y, int frame)
        {
            int interval = 40;

            int width = 160;
            int height = 10;

            int size = 4;
            int size2 = 2 * size;

            GraphicsBuffer buffer = display.show(this);
            buffer.clear();

            position = new Vector3(x, y, -0.2f);

            foreach (CreationData data in history)
            {
                if (data.Frame > frame)
                    break;

                data.Position = position + new Vector3(0, height, 0.1f);
                data.show(buffer);
                height += interval;
            }

            height += 6;

            if (position.X + width + 20 > device.Viewport.Width)
                position.X -= width;

            if (position.Y + height + 40 > device.Viewport.Height)
                position.Y -= height;

            background = new Rect(position + new Vector3(size, size, 0.1f), new Vector3(width - size2, height - size2, -0.2f), Color.PeachPuff);
            background.Buffer = buffer;

            border = new Rect(position, new Vector3(160, height, -0.3f), Color.MediumPurple);
            border.Buffer = buffer;

            buffer.initialize();
            buffer.show();
        }

        public void hide()
        {
            foreach (CreationData data in history)
                data.hide();

            background.hide();
            border.hide();
        }
    }
}
