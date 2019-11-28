using Lab3Game.CustomEffects;
using Lab3Game.Interfaces;
using Lab3Game.ResourceManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.Entities
{
    public class Background : IRenderable
    {
        private GameObjectComponent<CloudyBackgroundEffect> _go;

        public Background(Mesh<VertexPositionTexture> mesh, Vector2 pos, Vector2 scale)
        {
            _go = new GameObjectComponent<CloudyBackgroundEffect>(mesh, pos, scale, 0f, Effects.Instance.cloudsEffect);
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
            _go.effect.World = Matrix.CreateScale(new Vector3(_go.scale, 1f)) *
                               Matrix.CreateRotationZ(_go.rotation) *
                               Matrix.CreateWorld(new Vector3(_go.pos, 0f), Vector3.Forward, Vector3.Up);
            _go.effect.TimeFromStart = (float) time.TotalGameTime.TotalSeconds;
            _go.Render(device);
        }
    }
}