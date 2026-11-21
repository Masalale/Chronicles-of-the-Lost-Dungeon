using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] Shard shardPrefab;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite openSprite;
    [SerializeField] Vector2 dropOffset = new Vector2(0f, -0.8f);
    [SerializeField] Color sealedColor = new Color(0.5f, 0.5f, 0.6f);

    // Not just "sealed": that's a C# keyword.
    bool isSealed = true;
    bool opened;

    public bool CanInteract
    {
        get
        {
            return !isSealed && !opened;
        }
    }

    void Awake()
    {
        spriteRenderer.color = sealedColor;
    }

    public void Unseal()
    {
        isSealed = false;
        spriteRenderer.color = Color.white;
    }

    public void Interact(GameObject interactor)
    {
        if (!CanInteract)
        {
            return;
        }

        opened = true;
        spriteRenderer.sprite = openSprite;

        // Parent to the room so it unloads with it and finds RoomController.
        Instantiate(shardPrefab, transform.position + (Vector3)dropOffset, Quaternion.identity, transform.parent);
    }
}
