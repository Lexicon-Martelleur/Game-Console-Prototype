namespace Game.Model.Weapon;

public class Arrow : IWeapon
{
    public uint ReduceHealth => 5;

    public string Symbol => "🏹";

    public string Name => "Hammer";
}
