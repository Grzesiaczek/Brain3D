using System;
using System.Diagnostics;
using System.Threading;

namespace Brain3D
{
    abstract class Graph : Presentation
    {
        Stopwatch stopwatch;

        protected bool paused;

        protected static bool balanced;

        public static event EventHandler AnimationStop;
        public static event EventHandler BalanceFinished;

        public Graph()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public override void Hide()
        {
            visible = false;
            display.Clear();
            Stop();
        }

        public override void Left()
        {
            display.MoveLeft();
        }

        public override void Right()
        {
            display.MoveRight();
        }

        public override void Up()
        {
            display.Up();
        }

        public override void Down()
        {
            display.Down();
        }

        public override void Closer()
        {
            display.Closer();
        }

        public override void Farther()
        {
            display.Farther();
        }

        public override void Broaden()
        {
            display.Broaden();
        }

        public override void Tighten()
        {
            display.Tighten();
        }

        void Timer(object state)
        {
            while (started)
            {
                if (stopwatch.Elapsed.TotalMilliseconds > 20)
                {
                    stopwatch.Restart();
                    Tick();
                }

                Thread.Sleep(2);
            }
        }

        protected virtual void Tick() { }

        public override void Start()
        {
            started = true;
            paused = false;
            ThreadPool.QueueUserWorkItem(Timer);
        }

        public override void Stop()
        {
            if (started)
            {
                started = false;
                paused = true;
                AnimationStop(this, null);
            }
        }

        public override void Enter()
        {
            base.Enter();

            if (!QueryContainer.Insertion)
            {
                ChangeFrame(0);
            }
            else if (QueryContainer.Execute())
            {
                controller.ChangeFrame(0);
                ChangeFrame(0);
            }
        }

        protected void BalanceUpdate(object sender, EventArgs e)
        {
            display.Moved();
        }

        protected void BalanceEnded(object sender, EventArgs e)
        {
            balanced = true;
            controller.Idle();
            BalanceFinished(this, new EventArgs());
        }
    }
}
