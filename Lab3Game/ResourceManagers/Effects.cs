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

        private Effects(GraphicsDevice device, ContentManager contentManager)
        {
            basicEffect = new BasicEffect(device)
            {
                Texture = new Texture2D(device, 1, 1),
                TextureEnabled = true,
            };
            basicEffect.Texture.SetData(new[] {Color.Pink});

            cloudsEffect = CloudyBackgroundEffect.Create(contentManager.Load<Effect>("CloudyBackground"));
        }

        public static void Initialize(GraphicsDevice device, ContentManager contentManager)
        {
            Instance = new Effects(device, contentManager);
        }
    }
}