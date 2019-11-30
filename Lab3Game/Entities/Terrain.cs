using Lab3Game.Components;
using Lab3Game.CustomEffects;
using Lab3Game.Interfaces;
using Lab3Game.Materials;
using Lab3Game.Materials.Abstract;
using Lab3Game.ResourceManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;

namespace Lab3Game.Entities
{
    public class Terrain : IRenderable
    {
        private GameObjectComponent _go;
        private MaterialComponent _mat;
        private PhysicsObjectComponent _po;

        public float Layer => 0f;

        public Terrain(Mesh<VertexPositionTexture> mesh, Vector2 pos, Vector2 scale, float rotation,
                       Texture2D texture, SuperCoolGame game)
        {
            _go = new GameObjectComponent(mesh, pos, scale, rotation);
            _po = new PhysicsObjectComponent(game.World, _go, BodyType.Static, Category.Cat2, Category.Cat1);
            _mat = game.CreateMaterial(MaterialType.RandomSample, texture);
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
            _mat.Render(device, _go, time, Layer);
        }
    }
}