using Lab3Game.CustomEffects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.ResourceManagers
{
    public class Effects
    {
        public static Effects Instance { get; private set; }

        public readonly BasicEffect basicEffect;

        public readonly CloudyBackgroundEffect cloudsEffect;

        public readonly RandomSampleTextureEffect randomSampleTextureEffect;

        private Effects(GraphicsDevice device, ContentManager contentManager)
        {
            basicEffect = new BasicEffect(device)
            {
                Texture = new Texture2D(device, 1, 1),
                TextureEnabled = true,
            };
            basicEffect.Texture.SetData(new[] {Color.Pink});

            cloudsEffect = CloudyBackgroundEffect.Create(contentManager.Load<Effect>("CloudyBackground"));

            randomSampleTextureEffect =
                RandomSampleTextureEffect.Create(contentManager.Load<Effect>("RandomSampleTexture"));
        }

        public static void Initialize(GraphicsDevice device, ContentManager contentManager)
        {
            Instance = new Effects(device, contentManager);
        }

        ~Effects()
        {
            cloudsEffect.Dispose();
            randomSampleTextureEffect.Dispose();
            basicEffect.Dispose();
        }
    }
}