using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain3D
{
    class CreationHistory
    {
        AnimatedState synapse;

        public CreationHistory(AnimatedState active)
        {
            synapse = active;
        }

        protected void initializeGraphics()
        {
            /*Height = Math.Min(synapse.History.Count, 4) * 36 + 40;
            Width = 164;

            int x = Location.X;
            int y = Location.Y;

            if (Parent.Width - Location.X < Width)
                x = Location.X - Width;

            if (Parent.Height - Location.Y < Height)
                y = Location.Y - Height;

            if (x != Location.X || y != Location.Y)
                Location = new Point(x, y);*/

            //base.initializeGraphics();
        }

        public void show()
        {
            /*base.show();
            Controls.Clear();

            for (int i = Math.Max(0, synapse.History.Count - 4), j = 0; i < synapse.History.Count; i++, j++)
            {
                CreationData cd = synapse.History[i];
                Controls.Add(cd);
                cd.Location = new Point(2, j * 36 + 40);
                cd.show();
            }*/
        }
    }
}
