using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game
{
    public class Textures
    {
        public static Textures Instance { get; private set; }

        public readonly Texture2D green;
        public readonly Texture2D blue;
        public readonly Texture2D brown;

        private Textures(GraphicsDevice device, ContentManager contentManager)
        {
            green = new Texture2D(device, 1, 1);
            green.SetData(new[] {Color.Green});
            blue = new Texture2D(device, 1, 1);
            blue.SetData(new[] {Color.Blue});
            brown = new Texture2D(device, 1, 1);
            brown.SetData(new[] {Color.Brown});
        }

        public static void Initialize(GraphicsDevice device, ContentManager contentManager)
        {
            Instance = new Textures(device, contentManager);
        }
    }
}