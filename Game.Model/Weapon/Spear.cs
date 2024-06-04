namespace Game.Model.Weapon;

public class Spear : IWeapon
{
    public uint ReduceHealth => 5;

    public string Symbol => "🔱";

    public string Name => "Spear";
}
