using Lab3Game.Interfaces;

namespace Lab3Game.InputHandling
{
    public class ShootCommand : ICommand
    {
        public void Execute(InputHandler handler, IControllable actor)
        {
            actor.Shoot();
        }
    }
}