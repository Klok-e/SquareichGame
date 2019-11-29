using Lab3Game.CustomEffects;
using Lab3Game.Interfaces;
using Lab3Game.ResourceManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.Entities
{
    public class Castle : IRenderable
    {
        private GameObjectComponent<BasicEffect> _top;
        private Terrain _bottom;

        public float Health { get; private set; }

        public Castle(float health, Vector2 pos, Vector2 scale)
        {
            _top = new GameObjectComponent<BasicEffect>(Models.Instance.quad, pos + Vector2.UnitY * scale / 2f,
                scale, 0f,
                Effects.Instance.basicEffect);

            _bottom = new Terrain(Models.Instance.quad,
                pos - Vector2.UnitY * scale / 2f - Vector2.UnitX,
                scale + Vector2.UnitX * 2f, 0f, Textures.Instance.rocks);
            Health = health;
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
            _top.effect.Texture = Textures.Instance.castle;
            _top.effect.World = Matrix.CreateScale(new Vector3(_top.scale, 1f)) *
                                Matrix.CreateRotationZ(_top.rotation) *
                                Matrix.CreateWorld(new Vector3(_top.pos, 0f), Vector3.Forward, Vector3.Up);
            _top.Render(device);

            _bottom.Render(device, time);
        }
    }
}