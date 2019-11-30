using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.Interfaces
{
    public interface IRenderable
    {
        float Layer { get; }
        
        void Render(GraphicsDevice device, GameTime time);
    }
}