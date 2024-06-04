namespace Game.Model.Weapon;

public class Sword : IWeapon
{    
    public uint ReduceHealth => 5;

    public string Symbol => "🗡️";

    public string Name => "Sword";
}
