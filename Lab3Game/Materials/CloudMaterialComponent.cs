using Lab3Game.Components;
using Lab3Game.CustomEffects;
using Lab3Game.Entities;
using Lab3Game.Materials.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.Materials
{
    public class CloudMaterialComponent : MaterialComponent
    {
        public CloudMaterialComponent(Effect effect) : base(effect)
        {
        }

        protected override void PreRender(GameObjectComponent go, GameTime time)
        {
            var effect = (CloudyBackgroundEffect) Effect;
            effect.World = Matrix.CreateScale(new Vector3(go.scale, 1f)) *
                           Matrix.CreateRotationZ(go.rotation) *
                           Matrix.CreateWorld(new Vector3(go.pos, 0f), Vector3.Forward, Vector3.Up);
            effect.TimeFromStart = (float) time.TotalGameTime.TotalSeconds;
        }
    }
}