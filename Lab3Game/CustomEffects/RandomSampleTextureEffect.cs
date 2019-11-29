using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.CustomEffects
{
    public class RandomSampleTextureEffect : Effect, IEffectMatrices
    {
        public Matrix Projection { get; set; }
        public Matrix View { get; set; }
        public Matrix World { get; set; }
        public Texture2D Texture { get; set; }

        private RandomSampleTextureEffect(Effect cloneSource) : base(cloneSource)
        {
        }

        public RandomSampleTextureEffect(GraphicsDevice graphicsDevice, byte[] effectCode) : base(graphicsDevice,
            effectCode)
        {
        }

        public RandomSampleTextureEffect(GraphicsDevice graphicsDevice, byte[] effectCode, int index, int count) : base(
            graphicsDevice, effectCode, index, count)
        {
        }

        public static RandomSampleTextureEffect Create(Effect effect)
        {
            return new RandomSampleTextureEffect(effect);
        }

        protected override void OnApply()
        {
            Parameters["WorldViewProjection"].SetValue(World * View * Projection);
            Parameters["World"].SetValue(World);
            Parameters["Texture"].SetValue(Texture);
        }
    }
}