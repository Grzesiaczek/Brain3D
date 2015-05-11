﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Fonts;
using Nuclex.Graphics;

namespace Brain3D
{
    class Display : GraphicsDeviceControl
    {
        BasicEffect effect;
        Camera camera;
        Comparer comparer;

        Animation animation;
        Creation creation;
        Sequence sequence;

        Text2D state;
        Text2D fps;

        List<AnimatedElement> elements;
        List<DateTime> times = new List<DateTime>();

        float viewArea = 1.5f;
        int margin;

        int count = 0;
        bool moved;
        bool refreshed;

        int frame;
        int frames;
        int offset;

        TrackBar trackBar;

        GraphicsBuffer buffer;
        GraphicsBuffer values;

        protected override void Initialize()
        {
            camera = new Camera(Constant.Radius + 10);
            //camera = new Camera(new Vector3(2, 0, 2));
            DrawableElement.Camera = camera;

            elements = new List<AnimatedElement>();
            comparer = new Comparer();

            DrawableElement.Content = new ContentManager(Services, "Content");
            Application.Idle += delegate { Invalidate(); };

            Text2D.Batch = new SpriteBatch(Device);
            SpriteFont font = new ContentManager(Services, "Content").Load<SpriteFont>("Sequence");

            state = new Text2D("", font, new Vector2(Width - 120, 20), Color.DarkBlue, 60);
            fps = new Text2D("FPS: 0", font, new Vector2(Width - 100, 50), Color.DarkMagenta);

            effect = new BasicEffect(Device);
            effect.VertexColorEnabled = true;

            effect.World = Matrix.Identity;
            effect.View = Matrix.CreateLookAt(camera.Position, camera.Target, camera.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(viewArea, Device.Viewport.AspectRatio, 1, 80);

            Device.BlendState = BlendState.Opaque;
            Device.DepthStencilState = DepthStencilState.Default;
            Device.RasterizerState = RasterizerState.CullClockwise;
            Device.SamplerStates[0] = SamplerState.LinearWrap;

            DrawableElement.Device = Device;
            DrawableElement.Effect = effect;
            DrawableElement.Display = this;

            Circle.initialize();
            Text3D.load();
            SequenceElement.initialize();

            buffer = new GraphicsBuffer(Device);
            values = new GraphicsBuffer(Device);
        }

        protected override void Draw()
        {
            Device.DepthStencilState = DepthStencilState.Default;
            Device.Clear(Color.CornflowerBlue);
            Device.RasterizerState = RasterizerState.CullNone;

            effect.World = Matrix.Identity;
            effect.View = Matrix.CreateLookAt(camera.Position, camera.Target, camera.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(viewArea, Device.Viewport.AspectRatio, 1, 60);

            if (moved)
            {
                refresh();
                moved = false;
            }

            if (refreshed)
            {
                buffer.refresh();
                values.refresh();
                refreshed = false;
            }

            effect.CurrentTechnique.Passes[0].Apply();
            buffer.draw();
            values.draw();

            if (sequence != null)
                sequence.draw();

            times.Add(DateTime.Now);

            if (count > 100)
            {
                TimeSpan time = times[count] - times[count - 100];
                double span = time.TotalMilliseconds;
                fps.Text = "FPS: " + (int)(100000 / span);
            }
            else if (count != 0)
            {
                TimeSpan time = times[count] - times[0];
                double span = time.TotalMilliseconds;
                fps.Text = "FPS: " + (int)(1000 * count / span);
            }

            state.draw();
            fps.draw();

            count++;
        }

        public void add(DrawableElement element)
        {
            if (element is Text3D)
                element.Buffer = values;
            else
                element.Buffer = buffer;
        }

        public void add(Animation animation)
        {
            this.animation = animation;
        }

        public void add(Creation creation)
        {
            this.creation = creation;
        }

        public void show(Sequence sequence)
        {
            this.sequence = sequence;
        }

        public void initialize()
        {
            buffer.initialize();
            values.initialize();
        }

        public void resize()
        {
            Width = Parent.Width - margin;
            Height = Parent.Height;

            if (state == null)
                return;

            state.Location = new Vector2(Width - 100, 20);
            fps.Location = new Vector2(Width - 100, 60);
        }

        public void clear()
        {
            if(elements != null)
                elements.Clear();
        }

        public void move()
        {
            moved = true;
        }

        public void changeFrame(int frame)
        {
            this.frame = frame;
            trackBar.Value = frame;
            state.Text = frame.ToString() + "/" + frames;
        }

        public void changeState(int frame, int frames)
        {
            this.frames = frames;
            trackBar.Maximum = frames;
            changeFrame(frame);
        }

        public void refresh()
        {
            ThreadPool.QueueUserWorkItem(refreshment);
        }

        void refreshment(object state)
        {
            List<AnimatedElement> sorted = new List<AnimatedElement>();

            foreach (AnimatedElement element in elements)
            {
                element.refresh();
                sorted.Add(element);
            }

            sorted.Sort(comparer);
            //elements = sorted;
            refreshed = true;
        }

        public void setMargin(int value)
        {
            margin = value;
        }

        #region sterowanie widokiem

        public void left()
        {
            camera.moveLeft();
            refresh();
        }

        public void right()
        {
            camera.moveRight();
            refresh();
        }

        public void up()
        {
            camera.moveUp();
            refresh();
        }

        public void down()
        {
            camera.moveDown();
            refresh();
        }

        public void closer()
        {
            camera.closer();
            refresh();
        }

        public void farther()
        {
            camera.farther();
            refresh();
        }

        public void broaden()
        {
            if (viewArea >= 2.5f)
                return;

            viewArea += 0.01f;
        }

        public void tighten()
        {
            if (viewArea <= 0.5f)
                return;

            viewArea -= 0.01f;
        }

        #endregion

        public Dictionary<object, object> loadFrame(CreationFrame frame, int index)
        {
            return animation.loadFrame(frame, index);
        }

        public void loadTrackBar(TrackBar trackBar)
        {
            this.trackBar = trackBar;
        }

        public void add(AnimatedElement element)
        {
            elements.Add(element);
        }

        public void remove(AnimatedElement element)
        {
            elements.Remove(element);
        }
    }
}