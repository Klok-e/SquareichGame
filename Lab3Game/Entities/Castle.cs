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
    public class Castle : IRenderable
    {
        private GameObjectComponent _top;
        private MaterialComponent _mat;
        private Terrain _bottom;

        public float Health { get; private set; }

        public Castle(float health, Vector2 pos, Vector2 scale, World world, SuperCoolGame game)
        {
            _top = new GameObjectComponent(Models.Instance.quad, pos + Vector2.UnitY * scale / 2f,
                scale, 0f);
            _mat = game.CreateMaterial(MaterialType.Basic, Textures.Instance.castle);

            _bottom = new Terrain(Models.Instance.quad,
                pos - Vector2.UnitY * scale / 2f - Vector2.UnitX,
                scale + Vector2.UnitX * 2f, 0f, Textures.Instance.rocks, world, game);
            Health = health;
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
            _mat.Render(device, _top, time);
            _bottom.Render(device, time);
        }
    }
}