using Microsoft.Xna.Framework;

namespace Lab3Game.Interfaces
{
    public interface IControllable
    {
        void ApplyForce(Vector2 force);
        void RotateTo(Vector2 pos);
        void Shoot();
    }
}