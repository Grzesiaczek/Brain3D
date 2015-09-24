using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class DottedLine : DrawableComposite
    {
        public DottedLine(Vector3 source, Vector3 target, Color color, float size, int segments)
        {
            Vector3 segment = (target - source) / (2 * segments - 1);
            Vector3 interval = 2 * segment;
            Vector3 position = source;

            for (int i = 0; i < segments; i++, position += interval)
                drawables.Add(new Line(position, position + segment, color, size));
        }
    }
}
