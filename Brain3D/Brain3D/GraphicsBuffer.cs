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
        List<int[]> indices;

        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        int offset;
        int vertex;
        int index;

        bool initialized;

        public GraphicsBuffer(GraphicsDevice device)
        {
            vertices = new List<VertexPositionColor[]>();
            indices = new List<int[]>();
            elements = new List<DrawableElement>();
            this.device = device;

            offset = 0;
            vertex = 0;
            index = 0;
        }

        public void initialize()
        {
            clear();

            foreach (DrawableElement element in elements)
                element.initialize();

            vdata = new VertexPositionColor[vertex];
            idata = new int[index];

            if (vertex == 0)
                return;

            int count = 0;

            foreach (VertexPositionColor[] data in vertices)
                for (int i = 0; i < data.Length; i++)
                    vdata[count++] = data[i];

            count = 0;

            foreach (int[] data in indices)
                for (int i = 0; i < data.Length; i++)
                    idata[count++] = data[i];

            vertexBuffer = new VertexBuffer(device, typeof(VertexPositionColor), vertex, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(device, typeof(int), index, BufferUsage.WriteOnly);

            vertexBuffer.SetData<VertexPositionColor>(vdata);
            indexBuffer.SetData<int>(idata);

            initialized = true;
        }

        public void clear()
        {
            vertices.Clear();
            indices.Clear();

            offset = 0;
            vertex = 0;
            index = 0;
        }

        public void draw()
        {
            if (!initialized)
                return;

            device.Indices = indexBuffer;
            device.SetVertexBuffer(vertexBuffer);
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);

            device.Indices = null;
            device.SetVertexBuffer(null);
        }

        public void refresh()
        {
            foreach (DrawableElement element in elements) ;
                //element.refresh();

            vertexBuffer.SetData<VertexPositionColor>(vdata);
        }

        public int add(VertexPositionColor[] vertices, int[] indices)
        {
            this.vertices.Add(vertices);
            this.indices.Add(indices);

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

        public VertexPositionColor[] Vertices
        {
            get
            {
                return vdata;
            }
        }
    }
}
