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
        public Label3D(String data, Vector3 position)
        {
            this.position = position;
            text = font.Fill(data);
            color = Color.DarkBlue;

            if (text.Width > 40)
                scale = 1.6f / text.Width;
            else
                scale = 0.04f;
            
            shift = new Vector3(-text.Width / 2, -0.7f / scale, 0);
            constant = Matrix.CreateTranslation(shift) * Matrix.CreateScale(scale) * Matrix.CreateRotationY((float)Math.PI);
            refresh();
        }
    }
}
