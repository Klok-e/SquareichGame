namespace Lab3Game.Interfaces
{
    public interface IActor
    {
        float Health { get; }

        void GetDamage(float amount);
    }
}