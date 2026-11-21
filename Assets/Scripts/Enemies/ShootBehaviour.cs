using UnityEngine;

public class ShootBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [SerializeField] float fireInterval = 2f;
    [SerializeField] ProjectilePool pool;
    [SerializeField] AudioClip shootClip;

    float timer;

    public void Tick(Enemy enemy, Transform player)
    {
        enemy.Stop();

        // Starts at 0, so the first shot comes a full interval after waking.
        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            timer = 0f;
            Vector2 toPlayer = (Vector2)(player.position - enemy.transform.position);
            pool.Get().Launch(enemy.transform.position, toPlayer, pool);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySfx(shootClip);
            }
        }
    }
}
