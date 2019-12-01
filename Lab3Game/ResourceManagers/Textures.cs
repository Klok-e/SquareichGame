using System;
using System.Collections.Generic;
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
        public readonly Texture2D transparent;

        public readonly Texture2D castle;
        public readonly Texture2D rocks;
        public readonly Texture2D rocksBrown;
        public readonly Texture2D player;
        public readonly Texture2D squareRed;
        public readonly Texture2D squareBlue;

        public readonly Texture2D[] explosionAnim;

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
            transparent = new Texture2D(device, 1, 1);
            transparent.SetData(new[] {new Color(0, 0, 0, 0)});

            castle = contentManager.Load<Texture2D>("Castle");
            rocks = contentManager.Load<Texture2D>("rocky_texture");
            rocksBrown = contentManager.Load<Texture2D>("rocky_tex2");
            player = contentManager.Load<Texture2D>("Player");
            squareRed = contentManager.Load<Texture2D>("SquareRed");
            squareBlue = contentManager.Load<Texture2D>("SquareBlue");
            explosionAnim = TearApartAnAtlas(device, contentManager.Load<Texture2D>("explosion 3"));
        }

        private static Texture2D[] TearApartAnAtlas(GraphicsDevice device, Texture2D atlas)
        {
            var atlasData = new Color[atlas.Width * atlas.Height];
            atlas.GetData(atlasData);

            var list = new List<Texture2D>();

            for (var y = 0; y < 8; y++)
            for (var x = 0; x < 8; x++)
            {
                list.Add(ExtractImage(device, atlasData, x * 512, y * 512, 512, 512, atlas.Width));
            }

            return list.ToArray();
        }

        private static Texture2D ExtractImage(GraphicsDevice device, Color[] atlas, int x, int y, int width, int height,
                                              int atlasWidth)
        {
            var tex = new Texture2D(device, width, height);
            var data = new Color[width * height];
            for (var yy = 0; yy < height; yy++)
            for (var xx = 0; xx < width; xx++)
            {
                var a = atlas[atlasWidth * (y + yy) + (x + xx)];
                data[width * yy + xx] = a;
            }

            tex.SetData(data);
            return tex;
        }

        public static void Initialize(GraphicsDevice device, ContentManager contentManager)
        {
            Instance = new Textures(device, contentManager);
        }
    }
}