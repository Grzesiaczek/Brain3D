using System;
using System.Collections.Generic;
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
        #region deklaracje

        BasicEffect effect;
        BasicEffect effect2;

        Camera camera;
        Comparer comparer;

        Presentation presentation;
        Sequence sequence;
        Controller controller;

        List<AnimatedElement> elements;
        List<SpriteElement> sprites;

        GraphicsBuffer buffer;
        GraphicsBuffer numbers;
        GraphicsBuffer test;

        SpriteBatch batch;
        Color background;

        float viewArea = 0.84f;
        int margin;

        bool initialized;
        bool locked;
        int counter;

        #endregion

        #region funkcje główne

        protected override void Initialize()
        {
            camera = new Camera(10);
            DrawableElement.Camera = camera;

            elements = new List<AnimatedElement>();
            sprites = new List<SpriteElement>();
            comparer = new Comparer();

            ContentManager content = new ContentManager(Services, "Content");
            DrawableElement.Content = content;

            Fonts.initialize(content);
            Application.Idle += delegate { Invalidate(); };

            background = Color.Gainsboro;
            batch = new SpriteBatch(Device);

            effect = new BasicEffect(Device);
            effect.World = Matrix.Identity;
            effect.VertexColorEnabled = true;

            effect2 = new BasicEffect(Device);
            effect2.World = Matrix.Identity;
            effect2.VertexColorEnabled = true;

            Device.BlendState = BlendState.Opaque;
            Device.SamplerStates[0] = SamplerState.LinearWrap;

            DrawableElement.Device = Device;
            DrawableElement.Effect = effect;
            DrawableElement.Display = this;
            SpriteElement.Batch = batch;

            BorderedDisk.initializeCircle(); 
            Number3D.initializePatterns();
            Pipe.initializePalettes();

            Chart.initializeAngles();                
            Tile.initializeTextures();

            controller = new Controller();
            controller.resize();

            buffer = new GraphicsBuffer(Device, effect);
            numbers = new GraphicsBuffer(Device, effect);
            test = new GraphicsBuffer(Device, effect2);

            MouseClick += new MouseEventHandler(mouseClick);
            MouseDoubleClick += new MouseEventHandler(mouseClick);
            MouseMove += new MouseEventHandler(mouseMove);

            MouseDown += new MouseEventHandler(mouseDown);
            MouseUp += new MouseEventHandler(mouseUp);
        }

        protected override void Draw()
        {
            Device.Clear(background);
            Device.DepthStencilState = DepthStencilState.Default;

            effect.View = Matrix.CreateLookAt(camera.Position, camera.Target, camera.Up);

            if(presentation is Graph)
                effect.Projection = Matrix.CreatePerspectiveFieldOfView(viewArea, Device.Viewport.AspectRatio, 40, 100);
            else
                effect.Projection = Matrix.CreatePerspectiveFieldOfView(viewArea, Device.Viewport.AspectRatio, 4, 6);

            effect2.Projection = Matrix.CreateOrthographicOffCenter(0, Device.Viewport.Width, Device.Viewport.Height, 0, 0, 2);

            if(Number3D.Change && presentation is Animation)
            {
                numbers.clear(false);
                numbers.initialize();

                if(presentation is Animation)
                    numbers.show();
            }

            buffer.draw();
            numbers.draw();
            test.draw();

            batch.Begin();

            foreach (SpriteElement sprite in sprites)
                sprite.draw();

            batch.End();
        }

        #endregion

        #region funkcje podstawowe

        public void show(Presentation presentation)
        {
            this.presentation = presentation;
            viewArea = 0.84f;

            if (presentation is Graph)
                camera = new Camera(Constant.Radius + 50);
            else if (presentation is Charting)
                camera = new Camera(new Vector3(3, -0.5f, 5.0f));
            else
                camera = new Camera(new Vector3(3, 0.25f, 5.0f));
            
            DrawableElement.Camera = camera;

            initialize();
            show();
        }

        void show()
        {
            buffer.show();
            numbers.show();
            test.show();
        }

        void initialize()
        {
            buffer.initialize();
            numbers.initialize();
            test.initialize();
            initialized = true;
        }

        public void hide()
        {
            buffer.hide();
            numbers.hide();
            test.hide();
        }

        public void clear()
        {
            if (elements != null)
            {
                Balancing.Instance.stop();

                while (locked)
                    Thread.Sleep(2);

                foreach (AnimatedElement element in elements)
                    element.remove();

                elements.Clear();
            }

            if (sprites != null)
                sprites.Clear();

            buffer.clear();
            numbers.clear();
            test.clear();

            initialized = false;
        }

        #endregion

        #region funkcje odświeżające

        public void move()
        {
            if (locked || !initialized)
                return;

            locked = true;
            ThreadPool.QueueUserWorkItem(moving);
        }

        void moving(object state)
        {
            lock (elements)
            foreach (AnimatedElement element in elements)
                element.move();

            locked = false;
        }

        public void rotate()
        {
            ThreadPool.QueueUserWorkItem(rotation);
        }

        void rotation(object state)
        {
            lock(elements)
            foreach (AnimatedElement element in elements)
                element.rotate();
        }

        public void resize()
        {
            Width = Parent.Width - margin;
            Height = Parent.Height;

            if (controller != null)
                controller.resize();

            if(presentation != null)
                presentation.resize();
        }

        #endregion

        #region funkcje dodatkowe

        public void print(Presentation presentation)
        {
            ThreadPool.QueueUserWorkItem(screen, presentation);
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
            Device.Clear(background);
            Device.DepthStencilState = DepthStencilState.Default;
            Device.RasterizerState = RasterizerState.CullNone;

            effect.CurrentTechnique.Passes[0].Apply();
            buffer.draw();
            numbers.draw();

            batch.Begin();

            foreach (SpriteElement sprite in sprites)
                sprite.draw();

            batch.End();
            Device.SetRenderTarget(null);
            Thread.Sleep(2);

            title += "-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + (++counter).ToString() + ".png";
            FileStream stream = new FileStream(Path.Combine(Constant.Path, title), FileMode.Create, FileAccess.Write);
            target.SaveAsPng(stream, Width, Height);
            
            stream.Close();
        }

        public void setMargin(int value)
        {
            margin = value;
        }

        #endregion

        #region obsługa zdarzeń myszy

        void mouseClick(object sender, MouseEventArgs e)
        {
            presentation.mouseClick(e.X, e.Y);
        }

        void mouseMove(object sender, MouseEventArgs e)
        {
            presentation.mouseMove(e.X, e.Y);
        }

        void mouseDown(object sender, MouseEventArgs e)
        {
            presentation.mouseDown(e.X, e.Y);
        }

        void mouseUp(object sender, MouseEventArgs e)
        {
            presentation.mouseUp(e.X, e.Y);
        }

        #endregion

        #region sterowanie kamerą

        public void moveX(float value)
        {
            camera.moveX(value);
            rotate();
        }

        public void rescale(Vector2 factor)
        {
            camera.rescale(factor);
            rotate();
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
            if (viewArea >= 2.0f)
                return;

            viewArea += 0.01f;
        }

        public void tighten()
        {
            if (viewArea <= 0.4f)
                return;

            viewArea -= 0.01f;
        }

        #endregion

        #region sterowanie zawartością

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

        public void remove(SpriteElement element)
        {
            sprites.Remove(element);
        }

        public void add(AnimatedElement element)
        {
            elements.Add(element);
        }

        public void show(Sequence sequence)
        {
            if (this.sequence != null)
                this.sequence.hide();

            this.sequence = sequence;
            sequence.show();
        }

        public GraphicsBuffer show(CreationHistory history)
        {
            return test;
        }

        public Color Background
        {
            set
            {
                background = value;
            }
        }

        #endregion
    }
}