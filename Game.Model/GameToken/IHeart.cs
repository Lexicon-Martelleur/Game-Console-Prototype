using Game.Model.Base;

namespace Game.Model.GameToken;

public interface IHeart : IGameToken
{
    public uint HealthInjection { get; }
}
