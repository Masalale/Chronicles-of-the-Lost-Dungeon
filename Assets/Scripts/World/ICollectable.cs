using UnityEngine;

// Picked up on touch, unlike IInteractable which needs F.
public interface ICollectable
{
    void Collect(GameObject collector);
}
