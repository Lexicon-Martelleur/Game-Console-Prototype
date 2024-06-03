namespace Game.Model.Weapon;

internal class Sword : IWeapon
{    
    public uint ReduceHealth => 5;

    public string Symbol => "🗡️";

    public string Name => "Sword";
}
