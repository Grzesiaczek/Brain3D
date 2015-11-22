using System;
using System.Collections.Generic;

namespace Brain3D
{
    class Presentation
    {
        #region deklaracje

        protected static BrainContainer container;
        protected static Controller controller;
        protected static Display display;
        protected static int length;

        protected Balancing balancing;

        protected bool started;
        protected bool visible;

        protected List<Mouse> mouses;

        protected Mouse active;
        protected Mouse shifted;

        public static event EventHandler IntervalChanged;
        public static event EventHandler LoadBrain;
        public static event EventHandler QueryAccepted;

        #endregion

        public Presentation()
        {
            mouses = new List<Mouse>();

            LoadBrain += BrainLoaded;
            balancing = Balancing.Instance;
        }

        public static void Reload()
        {
            LoadBrain(null, null);
        }

        protected virtual void BrainLoaded(object sender, EventArgs e) { }

        public virtual void ChangeInsertion()
        {
            QueryContainer.ChangeInsertion();
            controller.Insertion = QueryContainer.Insertion;
        }

        public virtual void Start() { }

        public virtual void Stop() { }

        public virtual void Resize() { }

        public virtual void BalanceSynapses() { }

        #region obsługa zdarzeń myszy

        public virtual void MouseClick(int x, int y) { }

        public virtual void MouseMove(int x, int y)
        {
            if (shifted != null)
            {
                shifted.Move(x, y);
                return;
            }

            Mouse hover = null;

            foreach (Mouse mouse in mouses)
            {
                if (mouse.Cursor(x, y))
                {
                    hover = mouse;
                    break;
                }
            }

            if (active != null && active != hover)
            {
                active.Idle();
                active = null;
            }

            if (hover != null && hover != active)
            {
                hover.Hover();
                active = hover;
            }
        }

        public virtual void MouseDown(int x, int y)
        {
            if (active != null)
            {
                shifted = active;
                shifted.Push(x, y);
            }
        }

        public virtual void MouseUp(int x, int y)
        {
            if (shifted != null)
            {
                shifted = null;
            }
        }

        #endregion

        #region funkcje obsługi kontenera zapytań


        public virtual void Enter()
        {
            if (!QueryContainer.Insertion)
            {
                QueryContainer.Execute();
                IntervalChanged(this, null);
            }
            else if (QueryContainer.Execute())
            {
                ChangeInsertion();
            }
        }

        public virtual void Space()
        {
            if (QueryContainer.Insertion)
            {
                QueryContainer.Space();
            }
        }

        public virtual void Add(char key)
        {
            QueryContainer.Add(key);
        }

        public virtual void Erase()
        {
            QueryContainer.Erase();
        }

        public virtual void Higher()
        {
            QueryContainer.Higher();
        }

        public virtual void Lower()
        {
            QueryContainer.Lower();
        }

        public virtual void NextQuery()
        {
            QueryContainer.Next();
        }

        public virtual void PreviousQuery()
        {
            QueryContainer.Previous();
        }

        public virtual void NextBrain()
        {
            Hide();
            container.NextBrain();
            Show();
            ShowQuery();
        }

        public virtual void PreviousBrain()
        {
            Hide();
            container.PreviousBrain();
            Show();
            ShowQuery();
        }

        public virtual void ShowQuery()
        {
            if (this is Creation)
            {
                QueryContainer.Visible = false;
            }
            else
            {
                QueryContainer.Visible = true;
            }
        }

        #endregion

        #region funkcje sterujące

        public virtual void Back() { }

        public virtual void Forth() { }

        public virtual void ChangeFrame(int frame) { }

        public virtual void ChangePace(int pace) { }

        public virtual void Delete() { }

        public virtual void Show() { }

        public virtual void Hide() { }

        public virtual void Refresh() { }

        public virtual void Left() { }

        public virtual void Right() { }

        public virtual void Up() { }

        public virtual void Down() { }

        public virtual void Closer() { }

        public virtual void Farther() { }

        public virtual void Broaden() { }

        public virtual void Tighten() { }

        public virtual void Center() { }

        #endregion

        #region właściwości

        protected static Brain Brain
        {
            get
            {
                return container.Brain;
            }
        }

        public static BrainContainer BrainContainer
        {
            set
            {
                container = value;
            }
        }

        public static QueryContainer QueryContainer
        {
            get
            {
                return container.QueryContainer;
            }
        }

        public QuerySequence CurrentQuery
        {
            get
            {
                return QueryContainer.Query;
            }
        }

        public static Controller Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = value;
            }
        }

        public static Display Display
        {
            get
            {
                return display;
            }
            set
            {
                display = value;
            }
        }

        public static int Length
        {
            set
            {
                length = value;
            }
        }

        public bool Started
        {
            get
            {
                return started;
            }
        }

        #endregion
    }
}