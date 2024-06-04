namespace Game.Model.Weapon;

internal class Hammer : IWeapon
{
    public uint ReduceHealth => 5;

    public string Symbol => "🔨";

    public string Name => "Hammer";
}
