using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game
{
    public class Mesh<T>
        where T : struct, IVertexType
    {
        private readonly List<T> _vertices = new List<T>();
        private readonly List<short> _indices = new List<short>();
        private bool _buffersChanged = true;

        public IReadOnlyList<T> Vertices => _vertices;

        public VertexBuffer VertBuffer
        {
            get
            {
                if (!_buffersChanged)
                    return _vertBuffer;

                SetUpBuffers();
                return _vertBuffer;
            }
        }

        private VertexBuffer _vertBuffer;

        public IndexBuffer IndBuffer
        {
            get
            {
                if (!_buffersChanged)
                    return _indBuffer;

                SetUpBuffers();
                return _indBuffer;
            }
        }

        private IndexBuffer _indBuffer;

        private readonly GraphicsDevice _device;

        public Mesh(GraphicsDevice device)
        {
            _device = device;
        }

        public Mesh(GraphicsDevice device, IEnumerable<T> vertices, IEnumerable<short> indices) :
            this(device)
        {
            _vertices.AddRange(vertices);
            _indices.AddRange(indices);
        }

        public void AddVert(T vert)
        {
            _buffersChanged = true;
            _vertices.Add(vert);
        }

        public void AddInd(short ind)
        {
            _buffersChanged = true;
            _indices.Add(ind);
        }

        public void Clear()
        {
            _buffersChanged = true;
            _indices.Clear();
            _vertices.Clear();
        }

        private void SetUpBuffers()
        {
            _buffersChanged = false;

            // index
            _indBuffer = new IndexBuffer(_device, typeof(short), _indices.Count,
                BufferUsage.WriteOnly);
            _indBuffer.SetData(_indices.ToArray());

            // vertex
            _vertBuffer = new VertexBuffer(_device, typeof(T), _vertices.Count,
                BufferUsage.WriteOnly);
            _vertBuffer.SetData(_vertices.ToArray());
        }
    }
}