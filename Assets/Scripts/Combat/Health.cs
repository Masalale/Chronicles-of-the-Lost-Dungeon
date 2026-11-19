using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHealth = 5;

    public int Current { get; private set; }
    public int Max { get { return maxHealth; } }
    public bool Invulnerable { get; set; }

    public event System.Action<int, int> Changed;
    public event System.Action Died;

    void Awake()
    {
        Init(maxHealth);
    }

    public void Init(int max)
    {
        maxHealth = max;
        Current = max;

        Changed?.Invoke(Current, Max);
    }

    public void TakeDamage(int amount)
    {
        // Already dead: stops Died firing a second time.
        if (Invulnerable || Current <= 0)
        {
            return;
        }

        Current = Mathf.Max(0, Current - amount);

        Changed?.Invoke(Current, Max);

        if (Current == 0)
        {
            Died?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (Current <= 0)
        {
            return;
        }

        Current = Mathf.Min(Max, Current + amount);

        Changed?.Invoke(Current, Max);
    }

    // Never below 1, or the next Silo would start already dead.
    public void SetCurrent(int value)
    {
        Current = Mathf.Clamp(value, 1, Max);

        Changed?.Invoke(Current, Max);
    }
}
