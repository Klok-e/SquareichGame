using Microsoft.Xna.Framework;

namespace Lab3Game.Interfaces
{
    public interface IMovable
    {
        void ApplyForce(Vector2 force);
        void RotateTo(Vector2 pos);
    }
}