using System.Collections.Generic;
using UnityEngine;

public class FireballAbility : MonoBehaviour, IAbility
{
    [SerializeField] Sprite icon;
    [SerializeField] float cooldown = 1.5f;
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

        List<Vector2> positions = new List<Vector2>();
        foreach (Enemy enemy in Enemy.All)
        {
            positions.Add(enemy.transform.position);
        }

        // Plain points, so NearestFinder can be tested without enemies.
        int nearest = NearestFinder.FindNearest(transform.position, positions);

        Vector2 direction;
        if (nearest == -1)
        {
            direction = GetComponent<PlayerMovement>().FacingDirection;
        }
        else
        {
            direction = positions[nearest] - (Vector2)transform.position;
        }

        pool.Get().Launch(transform.position, direction, pool);
        cooldownRemaining = cooldown;
        return true;
    }
}
