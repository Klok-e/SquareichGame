using System;
using System.Collections.Generic;
using Lab3Game.Components;
using Lab3Game.InputHandling;
using Lab3Game.Interfaces;
using Lab3Game.Materials;
using Lab3Game.Materials.Abstract;
using Lab3Game.ResourceManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace Lab3Game.Entities
{
    public class Player : IRenderable, IUpdatable, IControllable, IPosition, IActor
    {
        private GameObjectComponent _go;
        private MaterialComponent _mat;
        private PhysicsObjectComponent _po;

        private SuperCoolGame _game;

        private const float forceMult = 5f;

        //private const float torqueMult = 5f;
        private const float dotInvert = 20f;
        private const float bulletSpawnDistance = 0.8f;
        private const float bulletForce = 20f;
        private const double shootTimeOut = 0.1f;

        public float Layer => 0.1f;
        public Vector2 Position => _go.pos;
        public float Health { get; private set; }

        private Vector2 _rotatingTo;

        private BulletDeactivator _deactivator = new BulletDeactivator();
        private InputHandler _inputHandler;

        public Player(Vector2 pos, Vector2 scale, float rotation, SuperCoolGame game, float health)
        {
            _go = new GameObjectComponent(Models.Instance.quad, pos, scale, rotation);
            _mat = game.CreateMaterial(MaterialType.Basic, Textures.Instance.player);
            _po = new PhysicsObjectComponent(game.World, _go, BodyType.Dynamic,
                Category.Cat1 | Category.Cat2 | Category.Cat3, Category.Cat2);
            _po.Body.Tag = this;
            _po.Body.IgnoreGravity = true;
            _po.Body.LinearDamping = 5f;
            _game = game;
            Health = health;

            _inputHandler = new InputHandler(_game, new MoveAxis(Keys.W, Keys.S, Keys.A, Keys.D), new MoveCommand(),
                new RotateCommand(), Keys.Space, new ShootCommand());
        }

        public void GetDamage(float amount)
        {
            Health -= amount;
            if (Health < 0f)
            {
                Console.WriteLine("You lose!");
                _game.Unregister(this);
                _game.SetTimeout(() => _po.Body.Enabled = false, 0);
            }
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
            _rotatingTo = pos;
        }

        private double _lastShotTime;

        public void Shoot()
        {
            if (_game.CurrentFixedTime < _lastShotTime + shootTimeOut)
                return;

            const string name = "triangle";
            var bullet = _game.BulletManager.Get(name);
            var rot = _go.rotation - MathHelper.PiOver2;
            var dir = new Vector2(MathF.Cos(rot), MathF.Sin(rot));
            bullet.Activate(_go.pos + dir * bulletSpawnDistance);
            bullet.ApplyForce(dir * bulletForce);
            bullet.SetVelocity(_po.Body.LinearVelocity);
            _game.Register(bullet);

            var exploded = false;

            void OnBulletHit()
            {
                if (exploded)
                    return;
                _deactivator.Add(name, bullet);
                _game.Register(bullet.Explode());
                exploded = true;
            }

            bullet.OnHitActor += OnBulletHit;

            void OnBulletHitTerrain()
            {
                _game.SetTimeout(() =>
                {
                    if (exploded)
                        return;
                    _deactivator.Add(name, bullet);

                    _game.Register(bullet.Explode());
                    exploded = true;
                }, 1f);
            }

            bullet.OnHitTerrain += OnBulletHitTerrain;

            void Action() => _deactivator.Add(name, bullet);
            _game.SetTimeout(Action, 10);
            _lastShotTime = _game.CurrentFixedTime;
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
            _mat.Render(device, _go, time, Layer);
        }

        public void FixedUpdate(GameTime gameTime)
        {
            _deactivator.DeactivateAll(_game);

            var (x, y) = _rotatingTo - _go.pos;
            var angle = MathF.Atan2(y, x) + MathHelper.PiOver2;

            if (angle > _go.rotation)
                while (angle - _go.rotation > MathHelper.Pi)
                    angle -= MathHelper.TwoPi;
            else
                while (angle - _go.rotation < -MathHelper.Pi)
                    angle += MathHelper.TwoPi;

            //Console.WriteLine($"{x} {y} {angle}");
            _po.Body.Rotation = MathHelper.Lerp(_po.Body.Rotation, angle, 0.4f);
            //_go.rotation = MathHelper.Lerp(_go.rotation, angle, 0.3f);

            foreach (var commands in _inputHandler.HandleInput(_game.CurrentKeyboardState, _game.CurrentMouseState))
                commands.Execute(_inputHandler, this);
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