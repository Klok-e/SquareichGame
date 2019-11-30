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
        private PhysicsObject _po;

        public Terrain(Mesh<VertexPositionTexture> mesh, Vector2 pos, Vector2 scale, float rotation,
                       Texture2D texture, World world, SuperCoolGame game)
        {
            _go = new GameObjectComponent(mesh, pos, scale, rotation);
            _po = new PhysicsObject(_go, world);
            _mat = game.CreateMaterial(MaterialType.RandomSample, texture);
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
            //_go.effect.Texture = _texture;
            //_go.effect.World = Matrix.CreateScale(new Vector3(_go.scale, 1f)) *
            //                   Matrix.CreateRotationZ(_go.rotation) *
            //                   Matrix.CreateWorld(new Vector3(_go.pos, 0f), Vector3.Forward, Vector3.Up);
            //_go.Render(device);
            _mat.Render(device, _go, time);
        }
    }
}