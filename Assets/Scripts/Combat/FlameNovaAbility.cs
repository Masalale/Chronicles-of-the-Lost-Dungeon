using UnityEngine;

public class FlameNovaAbility : MonoBehaviour, IAbility
{
    [SerializeField] Sprite icon;
    [SerializeField] float cooldown = 6f;
    [SerializeField] int count = 8;
    [SerializeField] ProjectilePool pool;

    float cooldownRemaining;

    public Sprite Icon { get { return icon; } }
    public float Cooldown { get { return cooldown; } }
    public float CooldownRemaining { get { return cooldownRemaining; } }

    void Update()
    {
        if (cooldownRemaining > 0f)
        {
            cooldownRemaining = Mathf.Max(0f, cooldownRemaining - Time.deltaTime);
        }
    }

    public bool Activate()
    {
        if (cooldownRemaining > 0f)
        {
            return false;
        }

        for (int i = 0; i < count; i++)
        {
            // Deg2Rad because Cos and Sin work in radians.
            float angle = i * 360f / count * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            pool.Get().Launch(transform.position, direction, pool);
        }

        cooldownRemaining = cooldown;
        return true;
    }
}
