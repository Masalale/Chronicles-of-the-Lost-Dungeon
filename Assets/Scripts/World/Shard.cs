using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Shard : MonoBehaviour, ICollectable
{
    public static event System.Action AnyCollected;

    bool collected;

    void Start()
    {
        Color color = GameManager.Instance.CurrentLevel.color;
        GetComponent<SpriteRenderer>().color = color;
        GetComponentInChildren<Light2D>().color = color;
    }

    public void Collect(GameObject collector)
    {
        // Destroy waits for the frame to end, so it can be touched twice.
        if (collected)
        {
            return;
        }

        collected = true;
        GetComponentInParent<RoomController>().ShardTaken();

        AnyCollected?.Invoke();

        Destroy(gameObject);
    }
}
