using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class RoomController : MonoBehaviour
{
    [SerializeField] RiddleKeeper riddleKeeper;
    [SerializeField] Chest chest;
    [SerializeField] Transform playerSpawn;
    [SerializeField] Tilemap floor;
    [SerializeField] Tilemap walls;
    [SerializeField] Light2D roomLight;

    int enemiesLeft;
    float fullLight;
    public static event System.Action<int> EnemiesLeftChanged;
    public static event System.Action ShardFound;

    Health playerHealth;
    HashSet<Vector2Int> blocked = new HashSet<Vector2Int>();
    RectInt bounds;

    void Awake()
    {
        fullLight = roomLight.intensity;

        BoundsInt area = floor.cellBounds;
        bounds = new RectInt(area.xMin, area.yMin, area.size.x, area.size.y);

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                if (walls.HasTile(new Vector3Int(x, y, 0)))
                {
                    blocked.Add(new Vector2Int(x, y));
                }
            }
        }

        // Pillars and other wall pieces are not on the walls tilemap, so mark
        // their cells too. The 0.1 inset stops an edge spilling into a cell.
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Wall") && colliders[i].gameObject != walls.gameObject)
            {
                Vector3Int min = walls.WorldToCell(colliders[i].bounds.min + new Vector3(0.1f, 0.1f, 0f));
                Vector3Int max = walls.WorldToCell(colliders[i].bounds.max - new Vector3(0.1f, 0.1f, 0f));
                for (int x = min.x; x <= max.x; x++)
                {
                    for (int y = min.y; y <= max.y; y++)
                    {
                        blocked.Add(new Vector2Int(x, y));
                    }
                }
            }
        }
    }

    void Start()
    {
        enemiesLeft = GetComponentsInChildren<Enemy>().Length;

        EnemiesLeftChanged?.Invoke(enemiesLeft);

        Enemy.AnyDied += OnEnemyDied;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = playerSpawn.position;
            playerHealth = player.GetComponent<Health>();
            playerHealth.Died += OnPlayerDied;
            playerHealth.Changed += OnPlayerHealthChanged;

            int torch = SaveManager.Instance.Data.torch;
            // 0 means a new game, so keep the full torch.
            if (torch > 0)
            {
                playerHealth.SetCurrent(torch);
            }

            OnPlayerHealthChanged(playerHealth.Current, playerHealth.Max);
        }

        if (enemiesLeft == 0)
        {
            chest.Unseal();
        }
    }

    void OnDestroy()
    {
        Enemy.AnyDied -= OnEnemyDied;

        if (playerHealth != null)
        {
            playerHealth.Died -= OnPlayerDied;
            playerHealth.Changed -= OnPlayerHealthChanged;
        }
    }

    public bool IsBlocked(Vector2 from, Vector2 to, float radius)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(from, radius, to - from, Vector2.Distance(from, to));
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.CompareTag("Wall"))
            {
                return true;
            }
        }
        return false;
    }

    public Vector2 NextStepTowards(Vector2 from, Vector2 to)
    {
        Vector3Int startCell = walls.WorldToCell(from);
        Vector3Int goalCell = walls.WorldToCell(to);
        Vector2Int start = new Vector2Int(startCell.x, startCell.y);
        Vector2Int goal = new Vector2Int(goalCell.x, goalCell.y);

        Vector2Int next = GridPathfinder.NextStep(blocked, bounds, start, goal);
        if (next == start)
        {
            return from;
        }

        return walls.GetCellCenterWorld(new Vector3Int(next.x, next.y, 0));
    }

    public void ShardTaken()
    {
        GameManager.Instance.Inventory.AddShard(GameManager.Instance.CurrentLevelIndex);
        riddleKeeper.Wake();
        ShardFound?.Invoke();
    }

    void OnEnemyDied(Enemy enemy)
    {
        enemiesLeft = Mathf.Max(0, enemiesLeft - 1);

        EnemiesLeftChanged?.Invoke(enemiesLeft);

        if (enemiesLeft == 0)
        {
            chest.Unseal();
        }
    }

    void OnPlayerDied()
    {
        GameManager.Instance.PlayerDied();
    }

    void OnPlayerHealthChanged(int current, int max)
    {
        roomLight.intensity = fullLight * current / max;
    }
}
