using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game
{
    public class Models
    {
        public static Models Instance => _instance ??= new Models();

        private static Models _instance;

        public readonly VertexPositionTexture[] QuadVerts;
        public readonly short[] QuadInds;

        private Models()
        {
            var (quadVerts, quadInds) = GetQuad();
            QuadVerts = quadVerts;
            QuadInds = quadInds;
        }

        private (VertexPositionTexture[], short[]) GetQuad()
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
    }
}