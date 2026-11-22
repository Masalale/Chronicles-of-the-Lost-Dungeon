using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioClip abilityClip;
    [SerializeField] AudioClip hurtClip;

    PlayerCombat combat;
    Health health;
    int lastHealth;

    void Awake()
    {
        combat = GetComponent<PlayerCombat>();
        health = GetComponent<Health>();
        lastHealth = health.Current;

        combat.AbilityUsed += OnAbilityUsed;
        health.Changed += OnHealthChanged;
    }

    void OnAbilityUsed(IAbility ability)
    {
        PlaySfx(abilityClip);
    }

    void OnHealthChanged(int current, int max)
    {
        // Changed also fires on a heal, so only play hurt when it went down.
        if (current < lastHealth)
        {
            PlaySfx(hurtClip);
        }

        lastHealth = current;
    }

    void PlaySfx(AudioClip clip)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySfx(clip);
        }
    }
}
