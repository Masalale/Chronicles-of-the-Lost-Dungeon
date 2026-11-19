// Projectiles hurt through this, so they don't care who they hit.
public interface IDamageable
{
    void TakeDamage(int amount);
}
