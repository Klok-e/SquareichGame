using Lab3Game.Interfaces;

namespace Lab3Game.InputHandling
{
    public class RotateCommand : ICommand
    {
        public void Execute(InputHandler handler, IMovable actor)
        {
            actor.RotateTo(handler.GetMousePos());
        }
    }
}