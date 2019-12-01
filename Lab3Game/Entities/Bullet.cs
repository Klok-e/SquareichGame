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
    public class Bullet : IUpdatable, IRenderable, IPrototype
    {
        public bool IsExploded { get; private set; } = false;
        private GameObjectComponent _go;
        private MaterialComponent _mat;
        private PhysicsObjectComponent _po;

        private SuperCoolGame _game;
        private Texture2D _tex;

        public float Layer => 0f;
        private float _rotation;

        public event Action OnHit;
        //public event Action OnHitTerrain;

        public Bullet(Vector2 pos, Vector2 scale, Texture2D texture, SuperCoolGame game)
        {
            _go = new GameObjectComponent(Models.Instance.quad, new Vector2(), scale, 0f);
            _mat = game.CreateMaterial(MaterialType.Basic, texture);
            _po = new PhysicsObjectComponent(game.World, _go, BodyType.Dynamic,
                Category.Cat1 | Category.Cat2, Category.Cat3);
            _po.Body.IsBullet = true;
            _po.Body.Mass = 0.02f;
            _po.Body.Position = pos;
            _go.pos = pos;

            _game = game;
            _rotation = (float) game.Random.NextDouble() * 2f / 50f;
            _tex = texture;
            _po.Body.OnCollision += (sender, other, contact) =>
            {
                if (other.Body.Tag is IActor actor && !(other.Body.Tag is Castle))
                {
                    actor.GetDamage(5f);
                }
                else
                {
                    //OnHitTerrain?.Invoke();
                    //OnHitTerrain = null;
                }

                OnHit?.Invoke();
                OnHit = null;

                return true;
            };
        }

        public Explosion Explode()
        {
            var expl = new Explosion(_go.pos, new Vector2(2f), 0f, _game);
            IsExploded = true;
            return expl;
        }

        public void ApplyForce(Vector2 force)
        {
            _po.Body.ApplyForce(force);
        }

        public void SetVelocity(Vector2 vel)
        {
            _po.Body.LinearVelocity = vel;
        }

        public void Activate(Vector2 pos)
        {
            _po.Body.Position = pos;
            _go.pos = pos;
        }

        public void Deactivate()
        {
            _po.Body.Enabled = false;
            _po.Body.World.Remove(_po.Body);
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
            if (!IsExploded)
                _mat.Render(device, _go, time, Layer);
        }

        public void FixedUpdate(GameTime gameTime)
        {
            //TODO: fix this (this code shouldn't be needed)
            if (IsExploded)
            {
                _po.Body.Enabled = false;
            }

            _po.Body.ApplyTorque(_rotation);
        }

        public void LateFixedUpdate(GameTime gameTime)
        {
            _go.pos = _po.Body.Position;
            _go.rotation = _po.Body.Rotation;
        }

        public void Update(GameTime gameTime)
        {
        }

        public IPrototype DeepClone()
        {
            var bullet = new Bullet(_go.pos, _go.scale, _tex, _game);
            bullet.Activate(_go.pos);
            return bullet;
        }
    }
}