using Lab3Game.Entities;
using Lab3Game.Interfaces;
using Microsoft.Xna.Framework;

namespace Lab3Game
{
    public class EnemySpawner : IUpdatable
    {
        private SuperCoolGame _game;

        private double _lastSpawnTime;
        private const double probabSpawnPowerful = 0.1f;
        private const double _spawnTimeout = 1f;

        public EnemySpawner(SuperCoolGame game)
        {
            _game = game;
        }

        public void FixedUpdate(GameTime gameTime)
        {
            if (_game.CurrentFixedTime < _lastSpawnTime + _spawnTimeout)
                return;

            var enemy = _game.EnemyManager.Get(_game.Random.NextDouble() < probabSpawnPowerful
                ? "powerful"
                : "regular");
            enemy.Activate();
            _game.Register(enemy);
            _lastSpawnTime = _game.CurrentFixedTime;
        }

        public void LateFixedUpdate(GameTime gameTime)
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}