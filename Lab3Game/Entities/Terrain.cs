using Lab3Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.Entities
{
    public class Terrain : IRenderable
    {
        private GameObjectComponent<BasicEffect> _go;
        private readonly Texture2D _texture;

        public Terrain(Mesh<VertexPositionTexture> mesh, Vector2 pos, Vector2 scale, float rotation, BasicEffect effect,
                       Texture2D texture)
        {
            _go = new GameObjectComponent<BasicEffect>(mesh, pos, scale, rotation, effect);
            _texture = texture;
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
            _go.effect.Texture = _texture;
            _go.effect.World = Matrix.CreateScale(new Vector3(_go.scale, 1f)) *
                               Matrix.CreateRotationZ(_go.rotation) *
                               Matrix.CreateWorld(new Vector3(_go.pos, 0f), Vector3.Forward, Vector3.Up);
            _go.Render(device);
        }
    }
}