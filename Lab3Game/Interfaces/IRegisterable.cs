namespace Lab3Game.Interfaces
{
    public interface IRegisterable<out T>
    {
        T GetRegistered(SuperCoolGame game);
    }
}