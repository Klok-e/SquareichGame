using System.Collections.Generic;
using Lab3Game.Entities;
using Lab3Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game
{
    public class Renderer
    {
        private readonly List<IRenderable> _renderables = new List<IRenderable>();
        private List<IEffectMatrices> _effectsMatrices = new List<IEffectMatrices>();

        public Renderer(params IEffectMatrices[] effects)
        {
            _effectsMatrices.AddRange(effects);
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

        public void RenderAll(GraphicsDevice device, GameTime time, Camera camera)
        {
            var view = camera.GetView();
            var projection = camera.GetProjection();
            foreach (var effect in _effectsMatrices)
            {
                effect.View = view;
                effect.Projection = projection;
            }

            // draw all the stuff
            foreach (var renderable in _renderables)
                renderable.Render(device, time);
        }
    }
}