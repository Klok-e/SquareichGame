using System;
using System.Collections.Generic;
using Lab3Game.Interfaces;
using Microsoft.Xna.Framework;

namespace Lab3Game
{
    public class Updater : IUpdatable
    {
        private readonly HashSet<IUpdatable> _updatables = new HashSet<IUpdatable>();
        private readonly HashSet<IUpdatable> _toAdd = new HashSet<IUpdatable>();
        private readonly HashSet<IUpdatable> _toRemove = new HashSet<IUpdatable>();

        public bool Register(IUpdatable updatable)
        {
            if (_updatables.Contains(updatable))
                return false;
            _toAdd.Add(updatable);
            return true;
        }

        public bool Unregister(IUpdatable updatable)
        {
            if (!_updatables.Contains(updatable))
                return false;

            _toRemove.Add(updatable);
            return true;
        }

        public void FixedUpdate(GameTime gameTime)
        {
            foreach (var upd in _updatables)
                upd.FixedUpdate(gameTime);

            foreach (var add in _toAdd)
                _updatables.Add(add);
            _toAdd.Clear();
            foreach (var add in _toRemove)
                _updatables.Remove(add);
            _toRemove.Clear();
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