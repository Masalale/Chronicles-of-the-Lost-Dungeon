using NUnit.Framework;
using UnityEngine;

public class HealthTests
{
    [Test]
    public void TakeDamage_WhenInvulnerable_DoesNotReduceHealth()
    {
        GameObject go = new GameObject();
        Health health = go.AddComponent<Health>();
        // Awake doesn't run in EditMode, so Init sets the health.
        health.Init(5);

        health.TakeDamage(2);
        Assert.AreEqual(3, health.Current);

        health.Invulnerable = true;
        health.TakeDamage(2);
        Assert.AreEqual(3, health.Current);

        Object.DestroyImmediate(go);
    }
}
