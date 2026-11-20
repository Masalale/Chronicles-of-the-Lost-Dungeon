using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 8f;
    [SerializeField] int damage = 1;
    [SerializeField] float lifetime = 3f;
    [SerializeField] string targetTag = "Enemy";

    Rigidbody2D body;
    ProjectilePool owner;
    float lifetimeRemaining;
    bool active;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 position, Vector2 direction, ProjectilePool owner)
    {
        this.owner = owner;
        // Set both, or a reused projectile flashes at its old spot for a frame.
        transform.position = position;
        body.position = position;
        body.linearVelocity = direction.normalized * speed;
        lifetimeRemaining = lifetime;
        active = true;
    }

    void Update()
    {
        lifetimeRemaining -= Time.deltaTime;

        if (lifetimeRemaining <= 0f)
        {
            ReturnToPool();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!active)
        {
            return;
        }

        if (other.CompareTag(targetTag))
        {
            IDamageable target = other.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
                ReturnToPool();
            }
        }
        else if (other.CompareTag("Wall"))
        {
            ReturnToPool();
        }
    }

    // It can hit two things in one frame. Without the active check it
    // would be released twice and the pool throws an error.
    void ReturnToPool()
    {
        if (!active)
        {
            return;
        }

        active = false;
        owner.Release(this);
    }
}
