using Lab3Game.CustomEffects;
using Lab3Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;

namespace Lab3Game.Entities
{
    public class GameObjectComponent
    {
        public readonly Mesh<VertexPositionTexture> mesh;
        public Vector2 pos;
        public Vector2 scale;
        public float rotation;

        public GameObjectComponent(Mesh<VertexPositionTexture> mesh, Vector2 pos, Vector2 scale, float rotation)
        {
            this.mesh = mesh;
            this.pos = pos;
            this.scale = scale;
            this.rotation = rotation;
        }
    }
}