using Lab3Game.Components;
using Lab3Game.Entities;
using Lab3Game.Materials.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.Materials
{
    public class BasicMaterialComponent : MaterialComponent
    {
        private Texture2D _texture;

        public BasicMaterialComponent(Effect effect, Texture2D texture) : base(effect)
        {
            _texture = texture;
        }

        protected override void PreRender(GameObjectComponent go, GameTime time)
        {
            var effect = (BasicEffect) Effect;
            effect.World = Matrix.CreateScale(new Vector3(go.scale, 1f)) *
                           Matrix.CreateRotationZ(go.rotation) *
                           Matrix.CreateWorld(new Vector3(go.pos, go.layer), Vector3.Forward, Vector3.Up);
            effect.Texture = _texture;
        }
    }
}