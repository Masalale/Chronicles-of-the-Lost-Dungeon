using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] Projectile prefab;
    [SerializeField] int defaultCapacity = 10;
    [SerializeField] int maxSize = 30;

    ObjectPool<Projectile> pool;

    void Awake()
    {
        // true = Unity checks we never release the same projectile twice.
        pool = new ObjectPool<Projectile>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile, true, defaultCapacity, maxSize);
    }

    void OnDestroy()
    {
        // Projectiles are not children of the pool, so clear them by hand.
        pool.Clear();
    }

    public Projectile Get()
    {
        return pool.Get();
    }

    public void Release(Projectile projectile)
    {
        pool.Release(projectile);
    }

    Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(prefab);
        projectile.gameObject.SetActive(false);
        return projectile;
    }

    void OnGetProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    void OnReleaseProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    void OnDestroyProjectile(Projectile projectile)
    {
        // Might already be gone if the scene is unloading.
        if (projectile != null)
        {
            Destroy(projectile.gameObject);
        }
    }
}
