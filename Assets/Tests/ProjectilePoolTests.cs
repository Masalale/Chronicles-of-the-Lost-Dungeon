using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class ProjectilePoolTests
{
    [Test]
    public void Get_AfterRelease_ReusesSameProjectile()
    {
        GameObject prefabObject = new GameObject();
        prefabObject.AddComponent<Projectile>();
        prefabObject.SetActive(false);

        GameObject poolObject = new GameObject();
        poolObject.SetActive(false);
        ProjectilePool pool = poolObject.AddComponent<ProjectilePool>();

        // Reflection sets the private prefab and runs Awake, because an
        // EditMode test has no scene to do it.
        BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
        typeof(ProjectilePool).GetField("prefab", flags).SetValue(pool, prefabObject.GetComponent<Projectile>());
        typeof(ProjectilePool).GetMethod("Awake", flags).Invoke(pool, null);

        Projectile p1 = pool.Get();
        pool.Release(p1);
        Projectile p2 = pool.Get();

        Assert.AreSame(p1, p2);

        Object.DestroyImmediate(p1.gameObject);
        Object.DestroyImmediate(poolObject);
        Object.DestroyImmediate(prefabObject);
    }
}
