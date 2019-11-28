using System.Collections.Generic;
using Lab3Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game
{
    public class Ticker
    {
        private readonly List<IUpdatable> _renderables = new List<IUpdatable>();

        public Ticker()
        {
        }

        public bool Register(IUpdatable renderable)
        {
            if (_renderables.Contains(renderable))
                return false;
            _renderables.Add(renderable);
            return true;
        }

        public bool Unregister(IUpdatable renderable)
        {
            if (!_renderables.Contains(renderable))
                return false;
            _renderables.Remove(renderable);
            return true;
        }
    }
}