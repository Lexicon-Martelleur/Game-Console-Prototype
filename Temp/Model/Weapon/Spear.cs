namespace Game.Model.Weapon;

internal class Spear : IWeapon
{
    public uint ReduceHealth => 5;

    public string Symbol => "🔱";

    public string Name => "Spear";
}
