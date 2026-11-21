using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite openSprite;
    [SerializeField] Collider2D blocker;

    bool isOpen;
    bool entered;

    public void Open()
    {
        if (isOpen)
        {
            return;
        }

        isOpen = true;
        spriteRenderer.sprite = openSprite;
        blocker.enabled = false;
    }

    // Stay, not Enter: the player may already be standing in the doorway
    // when it opens. entered stops it finishing the level every frame.
    void OnTriggerStay2D(Collider2D other)
    {
        if (!isOpen || entered || !other.CompareTag("Player"))
        {
            return;
        }

        entered = true;
        GameManager.Instance.CompleteLevel(other.GetComponent<Health>().Current);
    }
}
