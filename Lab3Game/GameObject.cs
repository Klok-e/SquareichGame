using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game
{
    public class GameObject : IRenderable
    {
        public Mesh mesh;
        public Vector2 pos;
        public Vector2 scale;
        public float rotation;
        public BasicEffect effect;
        public Texture2D texture2D;

        public void Render(GraphicsDevice device)
        {
            if (texture2D != effect.Texture)
                effect.Texture = texture2D;
            effect.World = Matrix.CreateScale(new Vector3(scale, 1f)) *
                           Matrix.CreateRotationZ(rotation) *
                           Matrix.CreateWorld(new Vector3(pos, 0f), Vector3.Forward, Vector3.Up);

            device.SetVertexBuffer(mesh.VertBuffer);
            device.Indices = mesh.IndBuffer;
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, mesh.IndBuffer.IndexCount / 3);
            }
        }
    }
}