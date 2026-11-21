using UnityEngine;

public interface IInteractable
{
    // PlayerInteraction hides the F prompt while this is false.
    bool CanInteract { get; }

    void Interact(GameObject interactor);
}
