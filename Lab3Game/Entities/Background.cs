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
    public class Background : IRenderable
    {
        private GameObjectComponent _go;
        private MaterialComponent _mat;

        public Background(Vector2 pos, Vector2 scale, SuperCoolGame game)
        {
            _go = new GameObjectComponent(Models.Instance.quad, pos, scale, 0f);
            _mat = game.CreateMaterial(MaterialType.Cloud, Textures.Instance.none);
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
            //_go.effect.World = Matrix.CreateScale(new Vector3(_go.scale, 1f)) *
            //                   Matrix.CreateRotationZ(_go.rotation) *
            //                   Matrix.CreateWorld(new Vector3(_go.pos, -5f), Vector3.Forward, Vector3.Up);
            //_go.effect.TimeFromStart = (float) time.TotalGameTime.TotalSeconds;
            //_go.Render(device);
            _mat.Render(device, _go, time);
        }
    }
}