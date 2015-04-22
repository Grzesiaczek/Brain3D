using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nuclex.Fonts;
using Nuclex.Graphics;

namespace Brain3D
{
    class Label3D : Text3D
    {
        public Label3D(String data, Vector3 position) : base(data, position)
        {
            scale = 0.05f;
            this.position = position;
            
            shift = new Vector3(-text.Width / 2, -text.Height * 0.3f -40, 0);
            constant = Matrix.CreateTranslation(shift) * Matrix.CreateScale(scale) * Matrix.CreateRotationY((float)Math.PI);
            refresh();
        }
    }
}
