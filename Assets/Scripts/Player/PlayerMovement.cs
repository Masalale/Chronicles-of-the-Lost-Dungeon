using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 4f;
    [SerializeField] SpriteRenderer sprite;

    Rigidbody2D body;
    Vector2 input;

    public Vector2 FacingDirection { get; private set; } = Vector2.right;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        GetComponent<Health>().Died += OnDied;
    }

    void FixedUpdate()
    {
        body.linearVelocity = input * speed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!enabled)
        {
            return;
        }

        input = context.ReadValue<Vector2>();

        // Keep the last direction when the stick is let go, for the fireball.
        if (input != Vector2.zero)
        {
            FacingDirection = input;
        }

        if (sprite != null && input.x != 0f)
        {
            sprite.flipX = input.x < 0f;
        }
    }

    void OnDied()
    {
        body.linearVelocity = Vector2.zero;
        enabled = false;
    }
}
