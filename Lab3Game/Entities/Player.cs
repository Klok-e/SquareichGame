using Lab3Game.Components;
using Lab3Game.Interfaces;
using Lab3Game.Materials;
using Lab3Game.Materials.Abstract;
using Lab3Game.ResourceManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.Entities
{
    public class Player : IRenderable, IUpdatable
    {
        private GameObjectComponent _go;
        private MaterialComponent _mat;
        private PhysicsObjectComponent _po;

        public Player(Vector2 pos, Vector2 scale, float rotation, SuperCoolGame game)
        {
            _go = new GameObjectComponent(Models.Instance.quad, pos, scale, rotation);
            _mat = game.CreateMaterial(MaterialType.Basic, Textures.Instance.player);
            _po = new PhysicsObjectComponent(_go, game.World);
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
        }

        public void FixedUpdate(GameTime gameTime)
        {
        }

        public void LateFixedUpdate(GameTime gameTime)
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}