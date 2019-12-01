using System.Collections.Generic;
using System.Linq;
using Lab3Game.Entities;
using Lab3Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game
{
    public class Renderer
    {
        private readonly HashSet<IRenderable> _renderables = new HashSet<IRenderable>();
        private readonly HashSet<IRenderable> _toAdd = new HashSet<IRenderable>();
        private readonly HashSet<IRenderable> _toRemove = new HashSet<IRenderable>();
        private List<IEffectMatrices> _effectsMatrices = new List<IEffectMatrices>();
        public Camera Camera { get; set; }

        public Renderer(params IEffectMatrices[] effects)
        {
            _effectsMatrices.AddRange(effects);
        }

        public bool Register(IRenderable renderable)
        {
            if (_renderables.Contains(renderable))
                return false;
            _toAdd.Add(renderable);
            return true;
        }

        public bool Unregister(IRenderable renderable)
        {
            if (!_renderables.Contains(renderable))
                return false;
            _toRemove.Add(renderable);
            return true;
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
            foreach (var add in _toAdd)
                _renderables.Add(add);
            _toAdd.Clear();
            foreach (var add in _toRemove)
                _renderables.Remove(add);
            _toRemove.Clear();
            
            var view = Camera.GetView();
            var projection = Camera.GetProjection();
            foreach (var effect in _effectsMatrices)
            {
                effect.View = view;
                effect.Projection = projection;
            }

            var lst = _renderables.ToList();
            lst.Sort((x, y) =>
            {
                if (x.Layer > y.Layer)
                    return 1;
                if (x.Layer < y.Layer)
                    return -1;
                return 0;
            });

            // draw all the stuff
            foreach (var renderable in lst)
                renderable.Render(device, time);
        }
    }
}