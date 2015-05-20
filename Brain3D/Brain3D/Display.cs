using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    using Color = Microsoft.Xna.Framework.Color;

    class Display : GraphicsDeviceControl
    {
        BasicEffect effect;
        Camera camera;
        Comparer comparer;

        Animation animation;
        Sequence sequence;
        Controller controller;

        List<AnimatedElement> elements;
        List<SpriteElement> sprites;

        GraphicsBuffer buffer;
        GraphicsBuffer numbers;
        SpriteBatch batch;

        float viewArea = 1.5f;
        int margin;

        bool initialized;
        bool locked;
        bool moved;
        bool refresh;

        //public Display

        protected override void Initialize()
        {
            camera = new Camera(Constant.Radius + 10);
            DrawableElement.Camera = camera;

            elements = new List<AnimatedElement>();
            sprites = new List<SpriteElement>();
            comparer = new Comparer();

            DrawableElement.Content = new ContentManager(Services, "Content");
            Application.Idle += delegate { Invalidate(); };

            controller = new Controller(this);
            batch = new SpriteBatch(Device);

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
            SpriteElement.Batch = batch;

            Number3D.initializePatterns();
            Pipe.initializePalettes();
            Chart.initializeAngles();

            Circle.initialize();
            BorderedDisk.initializeCircle();
            
            Tile.initializeTextures();
            Branch.initializeTexture();

            buffer = new GraphicsBuffer(Device);
            numbers = new GraphicsBuffer(Device);
            /*
            Pipe pipe = new Pipe(new Circle(Vector3.Zero, 2, 4), new Circle(new Vector3(16, 16, -16), 2, 4), 1);
            pipe.rotate();
            pipe.Buffer = buffer;*/
        }

        protected override void Draw()
        {
            Device.DepthStencilState = DepthStencilState.Default;
            Device.Clear(Color.CornflowerBlue);
            Device.RasterizerState = RasterizerState.CullNone;

            effect.World = Matrix.Identity;
            effect.View = Matrix.CreateLookAt(camera.Position, camera.Target, camera.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(viewArea, Device.Viewport.AspectRatio, 1, 60);

            if(Number3D.Change)
            {
                numbers.clear(false);
                numbers.initialize();
            }

            buffer.refreshVertices();
            numbers.refreshVertices();

            draw();
        }

        void screen(object state)
        {
            String title = "";

            if (state is Animation)
                title = "simulation";

            if (state is Creation)
                title = "creation";

            if (state is Charting)
                title = "chart";

            if (state is Tree)
                title = "tree";

            RenderTarget2D target = new RenderTarget2D(Device, Width, Height, false, Device.DisplayMode.Format, DepthFormat.Depth24, 4, RenderTargetUsage.PlatformContents);

            Device.SetRenderTarget(target);
            Device.Clear(Color.CornflowerBlue);
            Device.RasterizerState = RasterizerState.CullNone;

            Device.SetRenderTarget(null);
            FileStream stream = new FileStream(Path.Combine(Constant.Path, title), FileMode.Create, FileAccess.Write);

            title += "-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".png";
            target.SaveAsPng(stream, Width, Height);
            stream.Close();
        }

        void draw()
        {
            effect.CurrentTechnique.Passes[0].Apply();
            buffer.draw();
            numbers.draw();

            batch.Begin();

            foreach (SpriteElement sprite in sprites)
                sprite.draw();

            batch.End();
        }

        public void print(Presentation presentation)
        {
            ThreadPool.QueueUserWorkItem(screen, presentation);
        }

        public void show()
        {
            buffer.show();
            numbers.show();
        }

        public void hide()
        {
            buffer.hide();
            numbers.hide();
        }

        public void initialize()
        {
            buffer.initialize();
            numbers.initialize();

            initialized = true;
        }

        public void resize()
        {
            Width = Parent.Width - margin;
            Height = Parent.Height;
            
            if(controller != null)
                controller.Location = new Vector2(Width - 100, 60);
        }

        public void clear()
        {
            if(elements != null)
                elements.Clear();

            if (sprites != null)
                sprites.Clear();

            buffer.clear();
            numbers.clear();

            initialized = false;
        }

        public void move()
        {
            if (locked || !initialized)
                return;

            locked = true;
            ThreadPool.QueueUserWorkItem(moving);
        }

        void moving(object state)
        {
            foreach (AnimatedElement element in elements)
                element.move();

            locked = false;
            moved = true;
        }

        public void repaint()
        {
            refresh = true;
        }

        public void rotate()
        {
            ThreadPool.QueueUserWorkItem(rotation);
        }

        void rotation(object state)
        {
            foreach (AnimatedElement element in elements)
                element.rotate();

            refresh = true;
        }

        public void setMargin(int value)
        {
            margin = value;
        }

        #region sterowanie kamerą

        public void change(bool charting)
        {
            if (charting)
                camera = new Camera(new Vector3(2, 0, 2));
            else
                camera = new Camera(Constant.Radius + 10);

            DrawableElement.Camera = camera;
        }

        public void left()
        {
            camera.moveLeft();
            rotate();
        }

        public void right()
        {
            camera.moveRight();
            rotate();
        }

        public void up()
        {
            camera.moveUp();
            rotate();
        }

        public void down()
        {
            camera.moveDown();
            rotate();
        }

        public void closer()
        {
            camera.closer();
            rotate();
        }

        public void farther()
        {
            camera.farther();
            rotate();
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

        public void add(DrawableElement element)
        {
            if (element is Number3D)
                element.Buffer = numbers;
            else
                element.Buffer = buffer;
        }

        public void add(SpriteElement element)
        {
            sprites.Add(element);
        }

        public void add(Animation animation)
        {
            this.animation = animation;
        }

        public void show(Sequence sequence)
        {
            this.sequence = sequence;
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