using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interactRange = 1.2f;
    [SerializeField] GameObject prompt;
    [SerializeField] Vector3 promptOffset = new Vector3(0f, 1f, 0f);

    Health health;

    void Awake()
    {
        health = GetComponent<Health>();
        health.Died += OnDied;
    }

    void Update()
    {
        if (prompt == null)
        {
            return;
        }

        Transform target;
        IInteractable interactable = FindInteractable(out target);
        prompt.SetActive(interactable != null);
        if (interactable != null)
        {
            prompt.transform.position = target.position + promptOffset;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!enabled || !context.performed)
        {
            return;
        }

        Transform target;
        IInteractable interactable = FindInteractable(out target);
        if (interactable != null)
        {
            interactable.Interact(gameObject);
        }
    }

    IInteractable FindInteractable(out Transform target)
    {
        target = null;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange);
        for (int i = 0; i < hits.Length; i++)
        {
            IInteractable interactable = hits[i].GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract)
            {
                target = hits[i].transform;
                return interactable;
            }
        }
        return null;
    }

    // The player does the picking up, so an ICollectable needs no trigger code.
    void OnTriggerEnter2D(Collider2D other)
    {
        ICollectable item = other.GetComponent<ICollectable>();
        if (item != null)
        {
            item.Collect(gameObject);
        }
    }

    void OnDied()
    {
        enabled = false;
        if (prompt != null)
        {
            prompt.SetActive(false);
        }
    }
}
