using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float detectRange = 6f;
    [SerializeField] int contactDamage = 1;
    [SerializeField] float contactCooldown = 1f;
    [SerializeField] Color idleColor = new Color(0.5f, 0.5f, 0.6f);
    [SerializeField] float laneCentering = 12f;

    public static readonly List<Enemy> All = new List<Enemy>();
    public static event System.Action<Enemy> AnyDied;

    Rigidbody2D body;
    float contactTimer;
    RoomController room;
    float radius;

    public float DetectRange { get { return detectRange; } }
    public Color IdleColor { get { return idleColor; } }
    public Transform Player { get; private set; }
    public Health Health { get; private set; }
    public IEnemyBehaviour Behaviour { get; private set; }
    public SpriteRenderer Sprite { get; private set; }
    public EnemyState CurrentState { get; private set; }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        Health = GetComponent<Health>();
        Behaviour = GetComponent<IEnemyBehaviour>();
        Sprite = GetComponentInChildren<SpriteRenderer>();
        radius = GetComponent<Collider2D>().bounds.extents.x;

        Health.Died += OnHealthDied;
        ChangeState(new IdleState());
    }

    void OnEnable()
    {
        All.Add(this);
    }

    void OnDisable()
    {
        All.Remove(this);
    }

    void Start()
    {
        room = GetComponentInParent<RoomController>();
        Health.Changed += OnHealthChanged;
        AnyDied += OnAnyEnemyDied;

        if (Player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                Player = playerObject.transform;
            }
        }
    }

    void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Tick(this);
        }

        if (contactTimer > 0f)
        {
            contactTimer -= Time.deltaTime;
        }
    }

    public void SetPlayer(Transform player)
    {
        Player = player;
    }

    public void ChangeState(EnemyState next)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit(this);
        }

        CurrentState = next;

        if (CurrentState != null)
        {
            CurrentState.Enter(this);
        }
    }

    public float DistanceToPlayer()
    {
        if (Player == null)
        {
            return float.MaxValue;
        }

        return Vector2.Distance(transform.position, Player.position);
    }

    public void PathTowards(Vector2 target)
    {
        // Grid only when a wall is in the way; straight is smoother.
        if (room == null || !room.IsBlocked(transform.position, target, radius))
        {
            MoveTowards(target);
            return;
        }

        MoveAlongLane(room.NextStepTowards(transform.position, target));
    }

    void MoveTowards(Vector2 target)
    {
        Vector2 direction = target - (Vector2)transform.position;
        body.linearVelocity = direction.normalized * moveSpeed;

        if (Sprite != null && direction.x != 0f)
        {
            Sprite.flipX = direction.x < 0f;
        }
    }

    void MoveAlongLane(Vector2 next)
    {
        Vector2 direction = next - body.position;
        if (direction.sqrMagnitude < 0.0001f)
        {
            Stop();
            return;
        }

        Vector2 velocity;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Pull back to the middle of the lane, or the collider catches on
            // wall corners.
            velocity = new Vector2(Mathf.Sign(direction.x) * moveSpeed, Mathf.Clamp(direction.y * laneCentering, -moveSpeed, moveSpeed));
        }
        else
        {
            velocity = new Vector2(Mathf.Clamp(direction.x * laneCentering, -moveSpeed, moveSpeed), Mathf.Sign(direction.y) * moveSpeed);
        }

        body.linearVelocity = velocity;

        if (Sprite != null && direction.x != 0f)
        {
            Sprite.flipX = direction.x < 0f;
        }
    }

    public void Stop()
    {
        body.linearVelocity = Vector2.zero;
    }

    public void NotifyDied()
    {
        AnyDied?.Invoke(this);
    }

    void OnDestroy()
    {
        AnyDied -= OnAnyEnemyDied;
    }

    // Any death wakes every idle enemy, as if they heard the fight.
    void OnAnyEnemyDied(Enemy other)
    {
        if (CurrentState is IdleState)
        {
            ChangeState(new ActiveState());
        }
    }

    void OnHealthChanged(int current, int max)
    {
        if (CurrentState is IdleState && current > 0)
        {
            ChangeState(new ActiveState());
        }
    }

    // Leave All now, not in OnDisable: the body stays for 0.3s and
    // fireballs would keep aiming at it.
    void OnHealthDied()
    {
        All.Remove(this);
        ChangeState(new DeadState());
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            return;
        }

        if (contactTimer > 0f || CurrentState is DeadState)
        {
            return;
        }

        IDamageable target = collision.collider.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(contactDamage);
            contactTimer = contactCooldown;
        }
    }
}
