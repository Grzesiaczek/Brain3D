using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Fonts;
using Nuclex.Graphics;

namespace Brain3D
{
    class Fonts
    {
        static SpriteFont spriteArial;
        static SpriteFont spriteCalibri;
        static SpriteFont spriteVerdana;

        static VectorFont vectorArial;
        static VectorFont vectorCalibri;
        
        public static void initialize(ContentManager content)
        {
            spriteArial = content.Load<SpriteFont>("spriteArial");
            spriteCalibri = content.Load<SpriteFont>("spriteCalibri");
            spriteVerdana = content.Load<SpriteFont>("spriteVerdana");

            vectorArial = content.Load<VectorFont>("vectorArial");
            vectorCalibri = content.Load<VectorFont>("vectorCalibri");
        }

        public static SpriteFont SpriteArial
        {
            get
            {
                return spriteArial;
            }
        }

        public static SpriteFont SpriteCalibri
        {
            get
            {
                return spriteCalibri;
            }
        }

        public static SpriteFont SpriteVerdana
        {
            get
            {
                return spriteVerdana;
            }
        }

        public static VectorFont VectorArial
        {
            get
            {
                return vectorArial;
            }
        }

        public static VectorFont VectorCalibri
        {
            get
            {
                return vectorCalibri;
            }
        }
    }
}
