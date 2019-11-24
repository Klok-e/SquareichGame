using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game
{
    public class Models
    {
        public static Models Instance { get; private set; }

        public readonly Mesh quad;

        public readonly Mesh triangle;

        private Models(GraphicsDevice device)
        {
            var (quadVerts, quadInds) = GetQuad();
            quad = new Mesh(device, quadVerts, quadInds);

            var (triVerts, triInds) = GetTriangle();
            triangle = new Mesh(device, triVerts, triInds);
        }

        public static void Initialize(GraphicsDevice device)
        {
            Instance = new Models(device);
        }

        private static (VertexPositionTexture[], short[]) GetQuad()
        {
            var vertices = new List<VertexPositionTexture>();
            var indices = new List<short>();

            // create quad
            vertices.Add(new VertexPositionTexture
            {
                Position = new Vector3(-0.5f, -0.5f, 0),
                TextureCoordinate = new Vector2(0f, 0f)
            });
            vertices.Add(new VertexPositionTexture
            {
                Position = new Vector3(-0.5f, 0.5f, 0),
                TextureCoordinate = new Vector2(0f, 1f)
            });
            vertices.Add(new VertexPositionTexture
            {
                Position = new Vector3(0.5f, 0.5f, 0),
                TextureCoordinate = new Vector2(1f, 1f)
            });
            vertices.Add(new VertexPositionTexture
            {
                Position = new Vector3(0.5f, -0.5f, 0),
                TextureCoordinate = new Vector2(1f, 0f)
            });

            indices.Add(0);
            indices.Add(1);
            indices.Add(2);
            indices.Add(0);
            indices.Add(2);
            indices.Add(3);

            return (vertices.ToArray(), indices.ToArray());
        }

        private static (VertexPositionTexture[], short[]) GetTriangle()
        {
            var vertices = new List<VertexPositionTexture>();
            var indices = new List<short>();

            // create quad
            vertices.Add(new VertexPositionTexture
            {
                Position = new Vector3(-0.5f, -0.5f, 0),
                TextureCoordinate = new Vector2(0f, 0f)
            });
            vertices.Add(new VertexPositionTexture
            {
                Position = new Vector3(0.5f, -0.5f, 0),
                TextureCoordinate = new Vector2(0f, 1f)
            });
            vertices.Add(new VertexPositionTexture
            {
                Position = new Vector3(0f, 0.5f, 0),
                TextureCoordinate = new Vector2(1f, 1f)
            });

            indices.Add(0);
            indices.Add(2);
            indices.Add(1);

            return (vertices.ToArray(), indices.ToArray());
        }
    }
}