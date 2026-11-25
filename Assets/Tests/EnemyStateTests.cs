using NUnit.Framework;
using UnityEngine;

public class EnemyStateTests
{
    [Test]
    public void IdleState_WhenPlayerInDetectRange_ChangesToActiveState()
    {
        GameObject enemyObject = new GameObject();
        enemyObject.transform.position = Vector3.zero;
        Enemy enemy = enemyObject.AddComponent<Enemy>();

        GameObject playerObject = new GameObject();
        playerObject.transform.position = new Vector3(2f, 0f, 0f);

        // Start never runs in an EditMode test, so hand it the player.
        enemy.SetPlayer(playerObject.transform);
        enemy.ChangeState(new IdleState());
        enemy.CurrentState.Tick(enemy);

        Assert.IsInstanceOf<ActiveState>(enemy.CurrentState);

        Object.DestroyImmediate(enemyObject);
        Object.DestroyImmediate(playerObject);
    }
}
