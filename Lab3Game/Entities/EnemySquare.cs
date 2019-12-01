using System;
using Lab3Game.Components;
using Lab3Game.Interfaces;
using Lab3Game.Materials;
using Lab3Game.Materials.Abstract;
using Lab3Game.ResourceManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;

namespace Lab3Game.Entities
{
    public class EnemySquare : IRenderable, IUpdatable, IActor, IPrototype
    {
        private GameObjectComponent _go;
        private MaterialComponent _mat;
        private PhysicsObjectComponent _po;

        private SuperCoolGame _game;
        private BulletDeactivator _deactivator = new BulletDeactivator();
        private Texture2D _tex;
        private string _bulletName;

        private double _lastShotTime;
        private const float bulletSpawnDistance = 2f;
        private const float bulletForce = 20f;
        private const double shootTimeOut = 1f;
        private const float moveForceMult = 10f;
        private const float torque = 2f;

        public float Layer { get; }

        public EnemySquare(SuperCoolGame game, string bulletName, float health, Texture2D texture, Vector2 pos,
                           Vector2 scale, float rotation)
        {
            _go = new GameObjectComponent(Models.Instance.quad, pos, scale, rotation);
            _mat = game.CreateMaterial(MaterialType.Basic, texture);
            _po = new PhysicsObjectComponent(game.World, _go, BodyType.Dynamic,
                Category.Cat1 | Category.Cat2 | Category.Cat3, Category.Cat2);
            _po.Body.Tag = this;
            _po.Body.LinearDamping = 5f;
            _game = game;
            _bulletName = bulletName;
            Health = health;
            _tex = texture;
        }

        public void Activate()
        {
            _po.Body.Enabled = true;
        }

        public void Deactivate()
        {
            _po.Body.Enabled = false;
        }

        private void Shoot(Vector2 where)
        {
            if (_lastShotTime + shootTimeOut > _game.CurrentFixedTime)
                return;

            var bullet = _game.BulletManager.Get(_bulletName);
            var dir = where - _go.pos;
            dir.Normalize();
            bullet.Activate(_go.pos + dir * bulletSpawnDistance);
            bullet.ApplyForce(dir * bulletForce);
            bullet.SetVelocity(_po.Body.LinearVelocity);
            _game.Register(bullet);


            var exploded = false;

            void OnBulletHit()
            {
                if (exploded)
                    return;
                _deactivator.Add(_bulletName, bullet);
                bullet.Explode();
                exploded = true;
            }

            bullet.OnHitActor += OnBulletHit;

            void OnBulletHitTerrain()
            {
                _game.SetTimeout(() =>
                {
                    if (exploded)
                        return;
                    _deactivator.Add(_bulletName, bullet);

                    bullet.Explode();
                    exploded = true;
                }, 1f);
            }

            bullet.OnHitTerrain += OnBulletHitTerrain;

            void Action() => _deactivator.Add(_bulletName, bullet);
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
            Shoot(_game.Player.Position);

            _po.Body.ApplyForce(new Vector2(-1f, 0f) * moveForceMult);
            _po.Body.ApplyTorque(torque);
        }

        public void LateFixedUpdate(GameTime gameTime)
        {
            _go.pos = _po.Body.Position;
            _go.rotation = _po.Body.Rotation;
        }

        public void Update(GameTime gameTime)
        {
        }

        public float Health { get; private set; }

        public void GetDamage(float amount)
        {
            Health -= amount;
            if (Health < 0f)
            {
                Console.WriteLine("Enemy died!");
                _game.Unregister(this);
                _game.SetTimeout(Deactivate, 0);
            }
        }

        public IPrototype DeepClone()
        {
            var square = new EnemySquare(_game, _bulletName, Health, _tex, _go.pos, _go.scale, _go.rotation);
            return square;
        }
    }
}