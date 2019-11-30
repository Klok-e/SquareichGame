using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.ResourceManagers
{
    public class Textures
    {
        public static Textures Instance { get; private set; }

        public readonly Texture2D none;
        public readonly Texture2D green;
        public readonly Texture2D blue;
        public readonly Texture2D brown;

        public readonly Texture2D castle;
        public readonly Texture2D rocks;
        public readonly Texture2D player;

        private Textures(GraphicsDevice device, ContentManager contentManager)
        {
            none = new Texture2D(device, 1, 1);
            none.SetData(new[] {Color.Pink});
            green = new Texture2D(device, 1, 1);
            green.SetData(new[] {Color.Green});
            blue = new Texture2D(device, 1, 1);
            blue.SetData(new[] {Color.Blue});
            brown = new Texture2D(device, 1, 1);
            brown.SetData(new[] {Color.Brown});

            castle = contentManager.Load<Texture2D>("Castle");
            rocks = contentManager.Load<Texture2D>("rocky_texture");
            player = contentManager.Load<Texture2D>("Player");
        }

        public static void Initialize(GraphicsDevice device, ContentManager contentManager)
        {
            Instance = new Textures(device, contentManager);
        }
    }
}