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
        GraphicsDevice device;

        VertexPositionColor[] vdata;
        int[] idata;

        List<DrawableElement> elements;
        List<VertexPositionColor[]> vertices;
        Dictionary<int[], bool> indices;

        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        int offset;
        int vertex;
        int index;

        bool initialized;
        bool refreshed;
        bool refresh;

        public GraphicsBuffer(GraphicsDevice device)
        {
            elements = new List<DrawableElement>();
            vertices = new List<VertexPositionColor[]>();
            indices = new Dictionary<int[], bool>();
            
            this.device = device;

            offset = 0;
            vertex = 0;
            index = 0;
        }

        public void initialize()
        {
            foreach (DrawableElement element in elements)
                element.initialize();

            vdata = new VertexPositionColor[vertex];
            
            if (vertex == 0)
                return;

            int count = 0;

            foreach (VertexPositionColor[] data in vertices)
                for (int i = 0; i < data.Length; i++)
                    vdata[count++] = data[i];

            vertexBuffer = new VertexBuffer(device, typeof(VertexPositionColor), vertex, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vdata);

            refreshIndices();
            initialized = true;
        }

        public void clear(bool all = true)
        {
            vertices.Clear();
            indices.Clear();

            if(all)
                elements.Clear();

            offset = 0;
            vertex = 0;
            index = 0;

            initialized = false;
        }

        public void draw()
        {
            if (!initialized || !refreshed)
                return;

            if (refresh)
                refreshIndices();

            device.Indices = indexBuffer;
            device.SetVertexBuffer(vertexBuffer);
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);

            device.Indices = null;
            device.SetVertexBuffer(null);
        }

        public void refreshVertices()
        {
            if (initialized)
                vertexBuffer.SetData<VertexPositionColor>(vdata);
        }

        void refreshIndices()
        {
            if (index == 0)
            {
                refreshed = false;
                return;
            }

            idata = new int[index];
            int count = 0;

            foreach (int[] data in indices.Keys)
            {
                if (!indices[data])
                    continue;

                for (int i = 0; i < data.Length; i++)
                    idata[count++] = data[i];
            }

            indexBuffer = new IndexBuffer(device, typeof(int), index, BufferUsage.WriteOnly);
            indexBuffer.SetData<int>(idata);

            refresh = false;
            refreshed = true;
        }

        public int add(VertexPositionColor[] vertices, int[] indices)
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

        public void add(DrawableElement element)
        {
            elements.Add(element);
        }

        public void show(int[] data)
        {
            if (indices[data])
                return;

            indices[data] = true;
            index += data.Length;
            refresh = true;
        }

        public void hide(int[] data)
        {
            if (!indices[data])
                return;

            indices[data] = false;
            index -= data.Length;
            refresh = true;
        }

        public void show()
        {
            index = 0;

            foreach(int[] data in indices.Keys.ToList())
            {
                indices[data] = true;
                index += data.Length;
            }

            refresh = true;
        }

        public void hide()
        {
            index = 0;

            foreach (int[] data in indices.Keys.ToList())
                indices[data] = false;

            refresh = true;
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
