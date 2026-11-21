using UnityEngine;

// Each enemy type plugs in its own Tick; ActiveState just calls it.
public interface IEnemyBehaviour
{
    void Tick(Enemy enemy, Transform player);
}
