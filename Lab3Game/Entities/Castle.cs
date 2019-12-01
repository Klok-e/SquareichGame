using System;
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
    public class Castle : IRenderable, IActor
    {
        private GameObjectComponent _top;
        private MaterialComponent _mat;
        private PhysicsObjectComponent _po;
        private Terrain _bottom;
        private SuperCoolGame _game;

        public float Layer => 0f;

        public float Health { get; private set; }

        public Castle(float health, Vector2 pos, Vector2 scale, SuperCoolGame game)
        {
            _top = new GameObjectComponent(Models.Instance.quad, pos + Vector2.UnitY * scale / 2f,
                scale, 0f);
            _po = new PhysicsObjectComponent(game.World, _top, BodyType.Static,
                Category.Cat2 | Category.Cat3, Category.Cat1);
            _mat = game.CreateMaterial(MaterialType.Basic, Textures.Instance.castle);
            _po.Body.Tag = this;

            _bottom = new Terrain(Models.Instance.quad,
                pos - Vector2.UnitY * scale / 2f - Vector2.UnitX,
                scale + Vector2.UnitX * 2f, 0f, Textures.Instance.rocks, game);
            _bottom.SetBodyTag(this);
            Health = health;
            _game = game;
        }

        public void GetDamage(float amount)
        {
            Health -= amount;
            if (Health < 0f)
            {
                Console.WriteLine("You lose!");
                _game.Unregister(this);
                _game.SetTimeout(() =>
                {
                    _po.Body.Enabled = false;
                    _bottom.SetBodyEnabled(false);
                }, 0);
            }
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
            _mat.Render(device, _top, time, Layer);
            _bottom.Render(device, time);
        }
    }
}