using System;
using Microsoft.Xna.Framework;

namespace Lab3Game
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new SuperCoolGame();
            game.Run();
        }

        public static Vector2 ToVec2(this Vector3 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }

        public static Vector3 ToVec3(this Vector2 vec,float z)
        {
            return new Vector3(vec.X, vec.Y, z);
        }
    }
}