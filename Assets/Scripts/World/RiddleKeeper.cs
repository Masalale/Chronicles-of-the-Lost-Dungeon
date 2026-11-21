using UnityEngine;

public class RiddleKeeper : MonoBehaviour, IInteractable
{
    public static event System.Action<RiddleKeeper, TriviaResult> Asked;
    public static event System.Action<bool> Answered;

    [SerializeField] TriviaGate trivia;
    [SerializeField] ExitDoor door;
    [SerializeField] int rightHeal = 1;
    [SerializeField] int wrongDamage = 1;

    bool awake;
    bool asked;
    bool missed;
    Health playerHealth;

    public bool CanInteract
    {
        get
        {
            return awake && !asked && trivia.IsReady;
        }
    }

    public void Wake()
    {
        awake = true;
        trivia.Fetch();
    }

    public void Interact(GameObject interactor)
    {
        if (!CanInteract)
        {
            return;
        }

        asked = true;
        playerHealth = interactor.GetComponent<Health>();
        Asked?.Invoke(this, trivia.Next());
    }

    public void Answer(bool correct)
    {
        Answered?.Invoke(correct);

        if (correct)
        {
            // Only a first-try answer heals, so guessing isn't free.
            if (!missed)
            {
                playerHealth.Heal(rightHeal);
            }
            door.Open();
            return;
        }

        missed = true;
        asked = false;
        playerHealth.TakeDamage(wrongDamage);
    }
}
