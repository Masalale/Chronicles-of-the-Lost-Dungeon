using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public event System.Action<IAbility> AbilityUsed;

    IAbility[] abilities;
    Health health;

    void Awake()
    {
        // Slot order is the component order on the Player in the Inspector.
        abilities = GetComponents<IAbility>();
        health = GetComponent<Health>();
        health.Died += OnDied;
    }

    public IAbility GetAbility(int index)
    {
        if (index < 0 || index >= abilities.Length)
        {
            return null;
        }

        return abilities[index];
    }

    // PlayerInput still calls disabled components, so check enabled.
    public void OnAbility1(InputAction.CallbackContext context)
    {
        if (!enabled)
        {
            return;
        }

        if (context.performed)
        {
            UseAbility(0);
        }
    }

    public void OnAbility2(InputAction.CallbackContext context)
    {
        if (!enabled)
        {
            return;
        }

        if (context.performed)
        {
            UseAbility(1);
        }
    }

    void UseAbility(int index)
    {
        IAbility ability = GetAbility(index);
        if (ability != null && ability.Activate())
        {
            AbilityUsed?.Invoke(ability);
        }
    }

    void OnDied()
    {
        enabled = false;
    }
}
