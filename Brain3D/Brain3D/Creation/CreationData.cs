using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class CreationData : GraphicsElement
    {
        #region deklaracje

        CreationFrame frame;
        CreatedSynapse state;

        Change value;
        Change factor;

        StateDisk before;
        StateDisk after;

        Text2D number;
        Text2D change;

        bool visible;

        #endregion

        public CreationData(CreatedSynapse state, CreationFrame frame, Change value, Change factor)
        {
            this.state = state;
            this.frame = frame;
            this.value = value;
            this.factor = factor;

            int n = (int)(100 * value.Value);

            before = new StateDisk(value, factor.Start);
            after = new StateDisk(value.Finish, factor.Finish);

            number = new Text2D(frame.Frame.ToString(), Fonts.SpriteVerdana, Vector2.Zero, Color.DarkSlateBlue, 10);

            if(value.Value > 0)
                change = new Text2D(n.ToString(), Fonts.SpriteVerdana, Vector2.Zero, Color.Green, 10);
            else
                change = new Text2D((-n).ToString(), Fonts.SpriteVerdana, Vector2.Zero, Color.Red, 10);
        }

        #region logika

        public void Tick(float scale)
        {
            state.setChange(value.Start, value.Start + value.Value * scale);
            state.setFactor(factor.Start + factor.Value * scale);
        }

        public void Set()
        {
            state.setChange(value.Start, value.Finish);
            state.setFactor(factor.Finish);
        }

        public void Execute()
        {
            state.setValue(value.Finish);
            state.setFactor(factor.Finish);
        }

        public void Undo()
        {
            state.setValue(value.Start);
            state.setFactor(factor.Start);
        }

        #endregion

        #region wyświetlanie

        public void Show(GraphicsBuffer buffer)
        {
            before.Scale = 1;
            after.Scale = 1;

            before.Buffer = buffer;
            after.Buffer = buffer;

            display.Add(number);
            display.Add(change);

            visible = true;
        }

        public void Hide()
        {
            if (!visible)
                return;

            before.Remove();
            after.Remove();

            number.Hide();
            change.Hide();
        }

        #endregion

        public Vector3 Position
        {
            set
            {
                before.Position = new Vector3(70, 20, 0) + value;
                after.Position = new Vector3(130, 20, 0) + value;

                number.Location = new Vector2(20 + value.X, 10 + value.Y);
                change.Location = new Vector2(90 + value.X, 10 + value.Y);
            }
        }

        public int Frame
        {
            get
            {
                return frame.Frame;
            }
        }
    }
}
