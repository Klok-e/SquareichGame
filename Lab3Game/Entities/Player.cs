using System;
using Lab3Game.Components;
using Lab3Game.Interfaces;
using Lab3Game.Materials;
using Lab3Game.Materials.Abstract;
using Lab3Game.ResourceManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace Lab3Game.Entities
{
    public class Player : IRenderable, IUpdatable
    {
        private GameObjectComponent _go;
        private MaterialComponent _mat;
        private PhysicsObjectComponent _po;

        public Player(Vector2 pos, Vector2 scale, float rotation, SuperCoolGame game)
        {
            _go = new GameObjectComponent(Models.Instance.quad, pos, scale, rotation, 0.1f);
            _mat = game.CreateMaterial(MaterialType.Basic, Textures.Instance.player);
            _po = new PhysicsObjectComponent(game.World, _go, BodyType.Dynamic, Category.Cat1, Category.Cat2);
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
            _mat.Render(device, _go, time);
        }

        public void FixedUpdate(GameTime gameTime)
        {
        }

        public void LateFixedUpdate(GameTime gameTime)
        {
            _go.pos = _po.Body.Position;
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}