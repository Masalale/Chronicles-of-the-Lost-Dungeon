using UnityEngine;

public class ChaseBehaviour : MonoBehaviour, IEnemyBehaviour
{
    // The pathing lives in Enemy, so the Guard can chase the same way.
    public void Tick(Enemy enemy, Transform player)
    {
        enemy.PathTowards(player.position);
    }
}
