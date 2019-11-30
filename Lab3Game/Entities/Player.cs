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
    public class Player : IRenderable, IUpdatable, IMovable, IPosition
    {
        private GameObjectComponent _go;
        private MaterialComponent _mat;
        private PhysicsObjectComponent _po;

        private SuperCoolGame _game;

        private const float forceMult = 5f;
        private const float torqueMult = 5f;
        private const float dotInvert = 20f;

        public float Layer => 0.1f;
        public Vector2 Position => _go.pos;

        public Player(Vector2 pos, Vector2 scale, float rotation, SuperCoolGame game)
        {
            _go = new GameObjectComponent(Models.Instance.quad, pos, scale, rotation);
            _mat = game.CreateMaterial(MaterialType.Basic, Textures.Instance.player);
            _po = new PhysicsObjectComponent(game.World, _go, BodyType.Dynamic, Category.Cat1, Category.Cat2);
            _po.Body.IgnoreGravity = true;
            _po.Body.LinearDamping = 5f;
            _game = game;
        }

        public void ApplyForce(Vector2 force)
        {
            force.Normalize();
            var vel = _po.Body.LinearVelocity;
            // for better responsiveness
            var angle = dotInvert - Vector2.Dot(force, vel);
            _po.Body.ApplyForce(force * forceMult * angle);
        }

        public void RotateTo(Vector2 pos)
        {
            var (x, y) = pos - _go.pos;
            var angle = MathF.Atan2(y, x) + MathHelper.PiOver2;

            if (angle > _go.rotation)
                while (angle - _go.rotation > MathHelper.Pi)
                    angle -= MathHelper.TwoPi;
            else
                while (angle - _go.rotation < -MathHelper.Pi)
                    angle += MathHelper.TwoPi;

            //Console.WriteLine($"{x} {y} {angle}");
            _po.Body.Rotation = MathHelper.Lerp(_po.Body.Rotation, angle, 0.3f);
            //_go.rotation = MathHelper.Lerp(_go.rotation, angle, 0.3f);
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
            _mat.Render(device, _go, time, Layer);
        }

        public void FixedUpdate(GameTime gameTime)
        {
        }

        public void LateFixedUpdate(GameTime gameTime)
        {
            _go.pos = _po.Body.Position;
            _go.rotation = _po.Body.Rotation;
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}