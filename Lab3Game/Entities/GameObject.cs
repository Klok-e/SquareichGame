using Lab3Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.Entities
{
    public class GameObjectComponent<T>
        where T : Effect
    {
        public Mesh<VertexPositionTexture> mesh;
        public Vector2 pos;
        public Vector2 scale;
        public float rotation;
        public T effect;

        public GameObjectComponent(Mesh<VertexPositionTexture> mesh, Vector2 pos, Vector2 scale, float rotation, T effect)
        {
            this.mesh = mesh;
            this.pos = pos;
            this.scale = scale;
            this.rotation = rotation;
            this.effect = effect;
        }

        public void Render(GraphicsDevice device)
        {
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