using UnityEngine;

// PlayerCombat uses any ability through this without knowing which one.
public interface IAbility
{
    Sprite Icon { get; }
    float Cooldown { get; }
    float CooldownRemaining { get; }
    bool Activate();
}
