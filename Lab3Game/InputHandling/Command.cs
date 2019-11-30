using Lab3Game.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace Lab3Game.InputHandling
{
    public interface ICommand
    {
        void Execute(InputHandler handler, IMovable actor);
    }
}