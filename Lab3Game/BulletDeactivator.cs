using System.Collections.Generic;
using Lab3Game.Entities;

namespace Lab3Game
{
    public class BulletDeactivator
    {
        private Queue<(string name, Bullet bull)> _toDeactivate = new Queue<(string, Bullet)>();

        public void Add(string name, Bullet bullet)
        {
            _toDeactivate.Enqueue((name, bullet));
        }

        public void DeactivateAll(SuperCoolGame game)
        {
            while (_toDeactivate.Count > 0)
            {
                var (name, bull) = _toDeactivate.Dequeue();
                bull.Deactivate();
                game.Unregister(bull);
                game.BulletManager.Pool(name, bull);
            }
        }
    }
}