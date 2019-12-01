using System;
using Lab3Game.Components;
using Lab3Game.Interfaces;
using Lab3Game.Materials;
using Lab3Game.Materials.Abstract;
using Lab3Game.ResourceManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.Entities
{
    public class Explosion : IUpdatable, IRenderable
    {
        private GameObjectComponent _go;
        private MaterialComponent _mat;

        private SuperCoolGame _game;

        private const float nextImageTimeout = 0.01f;
        private int _currentImage;
        private readonly Texture2D[] _textures;

        private readonly double _spawnTime;

        public Explosion(Vector2 pos, Vector2 scale, float rotation, SuperCoolGame game)
        {
            _game = game;
            _go = new GameObjectComponent(Models.Instance.quad, pos, scale, rotation);
            _textures = Textures.Instance.explosionAnim;
            _currentImage = 0;
            _spawnTime = _game.CurrentFixedTime;

            _mat = _game.CreateMaterial(MaterialType.Basic, _textures[_currentImage]);
        }

        public void FixedUpdate(GameTime gameTime)
        {
        }

        public void LateFixedUpdate(GameTime gameTime)
        {
        }

        public void Update(GameTime gameTime)
        {
            _currentImage = (int) ((_game.CurrentFixedTime - _spawnTime) / nextImageTimeout);
            if (_currentImage >= _textures.Length)
            {
                _game.Unregister(this);
                return;
            }

            //Console.WriteLine($"time {_game.CurrentFixedTime} img {_currentImage}");

            _mat = _game.CreateMaterial(MaterialType.Basic, _textures[_currentImage]);
        }

        public float Layer => 0.1f;

        public void Render(GraphicsDevice device, GameTime time)
        {
            _mat.Render(device, _go, time, Layer);
        }
    }
}