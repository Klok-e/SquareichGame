using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.CustomEffects
{
    public class CloudyBackgroundEffect : Effect, IEffectMatrices
    {
        public Matrix World { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public float TimeFromStart { get; set; }

        private CloudyBackgroundEffect(Effect cloneSource) : base(cloneSource)
        {
        }

        public CloudyBackgroundEffect(GraphicsDevice graphicsDevice, byte[] effectCode) :
            base(graphicsDevice, effectCode)
        {
        }

        public CloudyBackgroundEffect(GraphicsDevice graphicsDevice, byte[] effectCode, int index, int count) :
            base(graphicsDevice, effectCode, index, count)
        {
        }

        public static CloudyBackgroundEffect Create(Effect effect)
        {
            return new CloudyBackgroundEffect(effect);
        }

        protected override void OnApply()
        {
            Parameters["WorldViewProjection"].SetValue(World * View * Projection);
            Parameters["World"].SetValue(World);
            Parameters["Time"].SetValue(TimeFromStart);
        }
    }
}