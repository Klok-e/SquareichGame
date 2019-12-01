using System.Collections.Generic;
using Lab3Game.Interfaces;

namespace Lab3Game
{
    public class PrototypeManager<T>
        where T : class, IPrototype
    {
        private readonly bool _pool;
        private Dictionary<string, (T prot, Queue<T> pool)> _prototypes = new Dictionary<string, (T, Queue<T>)>();

        public PrototypeManager(bool pool)
        {
            _pool = pool;
        }

        public void AddPrototype(string name, T prototype)
        {
            _prototypes.Add(name, (prototype, new Queue<T>()));
        }

        public T Get(string name)
        {
            var (prot, pool) = _prototypes[name];
            return _pool && pool.Count > 0 ? pool.Dequeue() : prot.DeepClone() as T;
        }

        public void Pool(string name, T prototype)
        {
            var (_, pool) = _prototypes[name];
            pool.Enqueue(prototype);
        }
    }
}