using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    class QueryContainer
    {
        List<QuerySequence> data;

        QuerySequence added;
        QuerySequence query;

        Brain brain;
        Display display;

        int index;

        bool insertion;
        bool visible;

        public QueryContainer(Brain brain, Display display)
        {
            data = new List<QuerySequence>();

            this.brain = brain;
            this.display = display;
        }

        public void Add(QuerySequence query)
        {
            query.Load(brain);
            data.Add(query);

            if (this.query == null)
                this.query = query;
        }

        public void Simulate(int length)
        {
            foreach (QuerySequence query in data)
                brain.Simulate(query, length);
        }

        public void Show()
        {
            if (visible && query != null)
                display.Show(query);
        }

        public void Next()
        {
            if (++index == data.Count)
                index = 0;

            query = data[index];
            Constant.Query = query;
            Show();
        }

        public void Prev()
        {
            if (index == 0)
                index = data.Count;

            query = data[--index];
            Constant.Query = query;
            Show();
        }

        public void Higher()
        {
            if (added == null)
                query.IntervalUp();
            else
                added.IntervalUp();
        }

        public void Lower()
        {
            if (added == null)
                query.IntervalDown();
            else
                added.IntervalDown();
        }

        public void ChangeInsertion()
        {
            insertion = !insertion;

            if (insertion)
            {
                //TODO
                //added = new QuerySequence(10 * length + 1);
                display.Show(added);
            }
            else
                display.Show(query);
        }

        public bool Execute()
        {
            if(insertion)
                return added.Execute();

            return query.Execute();
        }

        public void Add(char key)
        {
            if(insertion)
                added.Add(key);
        }

        public void Erase()
        {
            added.Erase();
        }

        public void Space()
        {
            added.Space();
        }

        public void SetFrame(int frame)
        {
            query.SetFrame(frame);
        }

        public void Tick(double time)
        {
            query.Tick(time);
        }

        #region właściwości

        public QuerySequence Query
        {
            get
            {
                return query;
            }
        }

        public List<QuerySequence> Data
        {
            get
            {
                return data;
            }
        }

        public bool Insertion
        {
            get
            {
                return insertion;
            }
            set
            {
                insertion = value;
            }
        }

        public bool Visible
        {
            set
            {
                visible = value;
                Show();
            }
        }

        #endregion
    }
}
