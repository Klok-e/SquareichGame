using Lab3Game.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace Lab3Game.InputHandling
{
    public class MoveCommand : ICommand
    {
        public void Execute(InputHandler handler, IControllable actor)
        {
            var axis = handler.GetAxis(InputHandler.Axes.Move);
            actor.ApplyForce(axis);
        }
    }
}