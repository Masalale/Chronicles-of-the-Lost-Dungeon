using UnityEngine;

public abstract class EnemyState
{
    public virtual void Enter(Enemy enemy) { }

    public abstract void Tick(Enemy enemy);

    public virtual void Exit(Enemy enemy) { }
}

public class IdleState : EnemyState
{
    public override void Enter(Enemy enemy)
    {
        if (enemy.Sprite != null)
        {
            enemy.Sprite.color = enemy.IdleColor;
        }
    }

    public override void Tick(Enemy enemy)
    {
        if (enemy.Player != null && enemy.DistanceToPlayer() <= enemy.DetectRange)
        {
            enemy.ChangeState(new ActiveState());
        }
    }

    public override void Exit(Enemy enemy)
    {
        if (enemy.Sprite != null)
        {
            enemy.Sprite.color = Color.white;
        }
    }
}

public class ActiveState : EnemyState
{
    public override void Tick(Enemy enemy)
    {
        if (enemy.Behaviour != null)
        {
            enemy.Behaviour.Tick(enemy, enemy.Player);
        }
    }
}

public class DeadState : EnemyState
{
    public override void Enter(Enemy enemy)
    {
        enemy.Stop();

        Collider2D hitbox = enemy.GetComponent<Collider2D>();
        if (hitbox != null)
        {
            // Fireballs pass through while the body waits to be removed.
            hitbox.enabled = false;
        }

        enemy.NotifyDied();
        Object.Destroy(enemy.gameObject, 0.3f);
    }

    public override void Tick(Enemy enemy)
    {
    }
}
