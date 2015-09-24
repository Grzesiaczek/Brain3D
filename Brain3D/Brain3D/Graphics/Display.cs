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

        HashSet<AnimatedElement> elements;
        HashSet<SpriteElement> sprites;

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

            elements = new HashSet<AnimatedElement>();
            sprites = new HashSet<SpriteElement>();
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

            Chart.InitializeAngles();                
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

            InitializeEffect();

            if(Number3D.Change && presentation is Animation)
            {
                numbers.Clear(false);
                numbers.Initialize();

                if(presentation is Animation)
                    numbers.Show();
            }

            buffer.Draw();
            numbers.Draw();
            test.Draw();

            batch.Begin();

            foreach (SpriteElement sprite in sprites)
                sprite.Draw();

            batch.End();
        }

        void InitializeEffect()
        {
            effect.View = Matrix.CreateLookAt(camera.Position, camera.Target, camera.Up);

            if (presentation is Graph)
                effect.Projection = Matrix.CreatePerspectiveFieldOfView(viewArea, Device.Viewport.AspectRatio, 40, 100);
            else
                effect.Projection = Matrix.CreatePerspectiveFieldOfView(viewArea, Device.Viewport.AspectRatio, 4, 6);

            effect2.Projection = Matrix.CreateOrthographicOffCenter(0, Device.Viewport.Width, Device.Viewport.Height, 0, 0, 2);
        }

        #endregion

        #region funkcje podstawowe

        public void Show(Presentation presentation)
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

            InitializeEffect();
            InitializeBuffers();
            ShowDisplay();
        }

        void ShowDisplay()
        {
            buffer.Show();
            numbers.Show();
            test.Show();
        }

        void InitializeBuffers()
        {
            buffer.Initialize();
            numbers.Initialize();
            test.Initialize();
            initialized = true;
        }

        public void HideDisplay()
        {
            buffer.Hide();
            numbers.Hide();
            test.Hide();
        }

        public void Clear()
        {
            if (elements != null)
            {
                Balancing.Instance.Stop();

                while (locked)
                    Thread.Sleep(2);

                foreach (AnimatedElement element in elements)
                    element.Remove();

                elements.Clear();
            }

            if (sprites != null)
                sprites.Clear();

            buffer.Clear();
            numbers.Clear();
            test.Clear();

            initialized = false;
        }

        #endregion

        #region funkcje odświeżające

        public void Moved()
        {
            if (locked || !initialized)
                return;

            locked = true;
            ThreadPool.QueueUserWorkItem(Moving);
        }

        void Moving(object state)
        {
            HashSet<AnimatedElement> elements = new HashSet<AnimatedElement>(this.elements);

            foreach (AnimatedElement element in elements)
                element.Move();

            locked = false;
        }

        public void Rotate()
        {
            ThreadPool.QueueUserWorkItem(Rotation);
        }

        void Rotation(object state)
        {
            HashSet<AnimatedElement> elements = new HashSet<AnimatedElement>(this.elements);

            foreach (AnimatedElement element in elements)
                element.Rotate();
        }

        public void ResizeWindow()
        {
            Width = Parent.Width - margin;
            Height = Parent.Height;

            if (controller != null)
                controller.resize();

            if(presentation != null)
                presentation.Resize();
        }

        #endregion

        #region funkcje dodatkowe

        public void Print(Presentation presentation)
        {
            ThreadPool.QueueUserWorkItem(Screen, presentation);
        }

        void Screen(object state)
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
            buffer.Draw();
            numbers.Draw();

            batch.Begin();

            foreach (SpriteElement sprite in sprites)
                sprite.Draw();

            batch.End();
            Device.SetRenderTarget(null);
            Thread.Sleep(2);

            title += "-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + (++counter).ToString() + ".png";
            FileStream stream = new FileStream(Path.Combine(Constant.Path, title), FileMode.Create, FileAccess.Write);
            target.SaveAsPng(stream, Width, Height);
            
            stream.Close();
        }

        public void SetMargin(int value)
        {
            margin = value;
        }

        #endregion

        #region obsługa zdarzeń myszy

        void mouseClick(object sender, MouseEventArgs e)
        {
            presentation.MouseClick(e.X, e.Y);
        }

        void mouseMove(object sender, MouseEventArgs e)
        {
            presentation.MouseMove(e.X, e.Y);
        }

        void mouseDown(object sender, MouseEventArgs e)
        {
            presentation.MouseDown(e.X, e.Y);
        }

        void mouseUp(object sender, MouseEventArgs e)
        {
            presentation.MouseUp(e.X, e.Y);
        }

        #endregion

        #region sterowanie kamerą

        public void MoveX(float value)
        {
            camera.MoveX(value);
            Rotate();
        }

        public void Rescale(Vector2 factor)
        {
            camera.Rescale(factor);
            Rotate();
        }

        public void MoveLeft()
        {
            camera.MoveLeft();
            Rotate();
        }

        public void MoveRight()
        {
            camera.MoveRight();
            Rotate();
        }

        public void Up()
        {
            camera.MoveUp();
            Rotate();
        }

        public void Down()
        {
            camera.MoveDown();
            Rotate();
        }

        public void Closer()
        {
            camera.Closer();
            Rotate();
        }

        public void Farther()
        {
            camera.Farther();
            Rotate();
        }

        public void Broaden()
        {
            if (viewArea >= 2.0f)
                return;

            viewArea += 0.01f;
        }

        public void Tighten()
        {
            if (viewArea <= 0.4f)
                return;

            viewArea -= 0.01f;
        }

        #endregion

        #region sterowanie zawartością

        public void Add(AnimatedElement element)
        {
            elements.Add(element);
        }

        public void Add(DrawableElement element)
        {
            if (element is Number3D)
                element.Buffer = numbers;
            else
                element.Buffer = buffer;
        }

        public void Add(SpriteElement element)
        {
            sprites.Add(element);
        }

        public void Remove(SpriteElement element)
        {
            sprites.Remove(element);
        }

        public void Show(Sequence sequence)
        {
            if (this.sequence != null)
                this.sequence.Hide();

            this.sequence = sequence;
            presentation.Refresh();
            sequence.Show();
        }

        public GraphicsBuffer Show(CreationHistory history)
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