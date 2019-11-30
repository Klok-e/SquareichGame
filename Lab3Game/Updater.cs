using System.Collections.Generic;
using Lab3Game.Interfaces;
using Microsoft.Xna.Framework;

namespace Lab3Game
{
    public class Updater : IUpdatable
    {
        private readonly List<IUpdatable> _updatables = new List<IUpdatable>();

        public bool Register(IUpdatable updatable)
        {
            if (_updatables.Contains(updatable))
                return false;
            _updatables.Add(updatable);
            return true;
        }

        public bool Unregister(IUpdatable updatable)
        {
            if (!_updatables.Contains(updatable))
                return false;
            _updatables.Remove(updatable);
            return true;
        }

        public void FixedUpdate(GameTime gameTime)
        {
            foreach (var upd in _updatables)
                upd.FixedUpdate(gameTime);
        }

        public void LateFixedUpdate(GameTime gameTime)
        {
            foreach (var upd in _updatables)
                upd.LateFixedUpdate(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var upd in _updatables)
                upd.Update(gameTime);
        }
    }
}