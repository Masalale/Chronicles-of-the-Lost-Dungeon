using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchLight : MonoBehaviour
{
    [SerializeField] Light2D torch;
    [SerializeField] float radiusPerTorch;

    Health health;

    void Awake()
    {
        health = GetComponent<Health>();
        health.Changed += OnHealthChanged;
    }

    void Start()
    {
        // Health may have fired Changed before we subscribed, so sync once.
        OnHealthChanged(health.Current, health.Max);
    }

    void OnHealthChanged(int current, int max)
    {
        torch.pointLightOuterRadius = radiusPerTorch * current;
        torch.enabled = current > 0;
    }
}
