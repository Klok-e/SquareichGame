using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.Components
{
    public class GameObjectComponent
    {
        public readonly Mesh<VertexPositionTexture> mesh;
        public Vector2 pos;
        public Vector2 scale;
        public float rotation;
        public float layer;

        public GameObjectComponent(Mesh<VertexPositionTexture> mesh, Vector2 pos, Vector2 scale, float rotation,
                                   float layer)
        {
            this.mesh = mesh;
            this.pos = pos;
            this.scale = scale;
            this.rotation = rotation;
            this.layer = layer;
        }
    }
}