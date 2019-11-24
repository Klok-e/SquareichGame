using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game
{
    public class Renderer
    {
        private readonly List<IRenderable> _renderables = new List<IRenderable>();

        public Renderer()
        {
        }

        public bool Register(IRenderable renderable)
        {
            if (_renderables.Contains(renderable))
                return false;
            _renderables.Add(renderable);
            return true;
        }

        public bool Unregister(IRenderable renderable)
        {
            if (!_renderables.Contains(renderable))
                return false;
            _renderables.Remove(renderable);
            return true;
        }

        public void RenderAll(GraphicsDevice device)
        {
            // draw all the stuff
            foreach (var renderable in _renderables)
                renderable.Render(device);
        }
    }
}