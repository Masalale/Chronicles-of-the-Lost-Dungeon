using UnityEngine;

public class GuardBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [SerializeField] float shieldTime = 2f;
    [SerializeField] float openTime = 2f;
    [SerializeField] Color shieldColor = new Color(0.5f, 0.7f, 1f);

    float timer;
    bool shieldUp;

    void Awake()
    {
        // Shield starts down, so the player gets the first chance to hit.
        timer = openTime;
    }

    public void Tick(Enemy enemy, Transform player)
    {
        enemy.PathTowards(player.position);

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            shieldUp = !shieldUp;
            timer = shieldUp ? shieldTime : openTime;
            enemy.Health.Invulnerable = shieldUp;
            enemy.Sprite.color = shieldUp ? shieldColor : Color.white;
        }
    }
}
