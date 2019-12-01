using System.Collections.Generic;
using Lab3Game.Interfaces;

namespace Lab3Game
{
    public class PrototypeManager<T>
        where T : class, IPrototype
    {
        private Dictionary<string, T> _prototypes = new Dictionary<string, T>();

        public PrototypeManager()
        {
        }

        public void AddPrototype(string name, T prototype)
        {
            _prototypes.Add(name, prototype);
        }

        public T Get(string name)
        {
            var prot = _prototypes[name];
            return prot.DeepClone() as T;
        }
    }
}