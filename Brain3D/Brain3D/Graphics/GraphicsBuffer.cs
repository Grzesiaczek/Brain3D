using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class GraphicsBuffer
    {
        BasicEffect effect;
        GraphicsDevice device;

        VertexPositionColor[] vdata;
        int[] idata;

        List<DrawableElement> elements;
        List<VertexPositionColor[]> vertices;

        Dictionary<int[], bool> indices;
        HashSet<int[]> blocked;

        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        Object locker = new Object();

        int offset;
        int vertex;
        int index;

        bool initialized;
        bool refreshed;
        bool refresh;

        public GraphicsBuffer(GraphicsDevice device, BasicEffect effect)
        {
            elements = new List<DrawableElement>();
            vertices = new List<VertexPositionColor[]>();

            indices = new Dictionary<int[], bool>();
            blocked = new HashSet<int[]>();

            this.device = device;
            this.effect = effect;

            offset = 0;
            vertex = 0;
            index = 0;
        }

        public void Initialize()
        {
            foreach (DrawableElement element in elements)
                element.Initialize();

            vdata = new VertexPositionColor[vertex];
            
            if (vertex == 0)
                return;

            int count = 0;

            foreach (VertexPositionColor[] data in vertices)
                for (int i = 0; i < data.Length; i++)
                    vdata[count++] = data[i];

            vertexBuffer = new VertexBuffer(device, typeof(VertexPositionColor), vertex, BufferUsage.WriteOnly);

            refreshIndices();
            initialized = true;
        }

        public void Clear(bool all = true)
        {
            vertices.Clear();
            indices.Clear();

            if (all)
            {
                foreach (DrawableElement element in elements)
                    element.Buffer = null;

                elements.Clear();
            }

            offset = 0;
            vertex = 0;
            index = 0;

            initialized = false;
        }

        public void Draw()
        {
            if (!initialized || !refreshed)
                return;

            if (refresh)
                refreshIndices();

            lock(vdata)
                vertexBuffer.SetData<VertexPositionColor>(vdata);

            device.RasterizerState = RasterizerState.CullNone;
            effect.CurrentTechnique.Passes[0].Apply();

            device.Indices = indexBuffer;
            device.SetVertexBuffer(vertexBuffer);
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);

            device.Indices = null;
            device.SetVertexBuffer(null);
        }

        void refreshIndices()
        {
            if (index == 0)
            {
                refreshed = false;
                return;
            }

            lock (locker)
            {
                idata = new int[index];
                int count = 0;

                List<int[]> keys = new List<int[]>(indices.Keys);

                foreach (int[] data in keys)
                {
                    if (!indices[data])
                        continue;

                    for (int i = 0; i < data.Length; i++)
                        idata[count++] = data[i];
                }

                indexBuffer = new IndexBuffer(device, typeof(int), index, BufferUsage.WriteOnly);
                indexBuffer.SetData<int>(idata);
            }

            refresh = false;
            refreshed = true;
        }

        public int Add(VertexPositionColor[] vertices, int[] indices)
        {
            this.vertices.Add(vertices);
            this.indices.Add(indices, false);

            for (int i = 0; i < indices.Length; i++)
                indices[i] += offset;

            vertex += vertices.Length;
            index += indices.Length;

            int result = offset;
            offset += vertices.Length;
            return result;
        }

        public void Add(DrawableElement element)
        {
            elements.Add(element);
        }

        public void Show(int[] data)
        {
            lock (locker)
            {
                if (indices[data])
                    return;

                indices[data] = true;
                index += data.Length;
                refresh = true;
            }
        }

        public void Hide(int[] data)
        {
            lock (locker)
            {
                if (!indices[data])
                    return;

                indices[data] = false;
                index -= data.Length;
                refresh = true;
            }
        }

        public void Show()
        {
            lock (locker)
            {
                index = 0;

                foreach (int[] data in indices.Keys.ToList())
                {
                    if (blocked.Contains(data))
                        continue;

                    indices[data] = true;
                    index += data.Length;
                }

                refresh = true;
            }
        }

        public void Hide()
        {
            lock (locker)
            {
                foreach (int[] data in indices.Keys.ToList())
                    indices[data] = false;

                refresh = true;
            }
        }

        public void Block(int[] item)
        {
            blocked.Add(item);
        }

        public VertexPositionColor[] Vertices
        {
            get
            {
                return vdata;
            }
        }
    }
}
