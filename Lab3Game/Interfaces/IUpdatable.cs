using Microsoft.Xna.Framework;

namespace Lab3Game.Interfaces
{
    public interface IUpdatable
    {
        void FixedUpdate(GameTime gameTime);
        void Update(GameTime gameTime);
    }
}