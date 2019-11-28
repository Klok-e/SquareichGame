using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.Interfaces
{
    public interface IRenderable
    {
        void Render(GraphicsDevice device, GameTime time);
    }
}