using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    using Color = System.Drawing.Color;
    using Rectangle = Microsoft.Xna.Framework.Rectangle;

    abstract public class GraphicsDeviceControl : Control
    {
        GraphicsDeviceService graphicsDeviceService;
        ServiceContainer services = new ServiceContainer();

        Stopwatch limiter;

        #region Initialization

        protected override void OnCreateControl()
        {
            if (!DesignMode)
            {
                graphicsDeviceService = GraphicsDeviceService.AddRef(Handle, ClientSize.Width, ClientSize.Height);
                services.AddService<IGraphicsDeviceService>(graphicsDeviceService);

                Initialize();
            }

            limiter = new Stopwatch();
            limiter.Start();

            base.OnCreateControl();
        }

        protected override void Dispose(bool disposing)
        {
            if (graphicsDeviceService != null)
            {
                graphicsDeviceService.Release(disposing);
                graphicsDeviceService = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Paint

        protected override void OnPaint(PaintEventArgs e)
        {
            string beginDrawError = BeginDraw();

            if (string.IsNullOrEmpty(beginDrawError))
            {
                while (limiter.ElapsedMilliseconds < 10)
                {
                    Thread.Sleep(1);
                }

                limiter.Restart();
                Draw();
                EndDraw();
            }
            else
            {
                PaintUsingSystemDrawing(e.Graphics, beginDrawError);
            }
        }

        string BeginDraw()
        {
            if (graphicsDeviceService == null)
            {
                return Text + "\n\n" + GetType();
            }

            string deviceResetError = HandleDeviceReset();

            if (!string.IsNullOrEmpty(deviceResetError))
            {
                return deviceResetError;
            }

            Viewport viewport = new Viewport();

            viewport.X = 0;
            viewport.Y = 0;

            viewport.Width = ClientSize.Width;
            viewport.Height = ClientSize.Height;

            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            Device.Viewport = viewport;

            return null;
        }

        void EndDraw()
        {
            try
            {
                Rectangle sourceRectangle = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
                Device.Present(sourceRectangle, null, this.Handle);
            }
            catch
            {

            }
        }

        string HandleDeviceReset()
        {
            bool deviceNeedsReset = false;

            switch (Device.GraphicsDeviceStatus)
            {
                case GraphicsDeviceStatus.Lost:
                    return "Graphics device lost";

                case GraphicsDeviceStatus.NotReset:
                    deviceNeedsReset = true;
                    break;

                default:
                    PresentationParameters pp = Device.PresentationParameters;

                    deviceNeedsReset = (ClientSize.Width > pp.BackBufferWidth) || (ClientSize.Height > pp.BackBufferHeight);
                    break;
            }

            if (deviceNeedsReset)
            {
                try
                {
                    graphicsDeviceService.ResetDevice(ClientSize.Width, ClientSize.Height);
                }
                catch (Exception e)
                {
                    return "Graphics device reset failed\n\n" + e;
                }
            }

            return null;
        }

        protected virtual void PaintUsingSystemDrawing(Graphics graphics, string text)
        {
            graphics.Clear(Color.Gainsboro);

            using (Brush brush = new SolidBrush(Color.Black))
            {
                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    graphics.DrawString(text, Font, brush, ClientRectangle, format);
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        #endregion

        protected abstract void Initialize();
        protected abstract void Draw();

        public GraphicsDevice Device
        {
            get
            {
                return graphicsDeviceService.GraphicsDevice;
            }
        }

        public ServiceContainer Services
        {
            get
            {
                return services;
            }
        }
    }
}
