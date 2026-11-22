using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] Health playerHealth;
    [SerializeField] PlayerCombat playerCombat;
    [SerializeField] Image[] torches;
    [SerializeField] Sprite fullTorch;
    [SerializeField] Sprite emptyTorch;
    [SerializeField] Image[] abilityIcons;
    [SerializeField] Image[] abilityCooldownFills;
    [SerializeField] Image[] shardSlots;
    [SerializeField] Color emptyShardColor = new Color(0.2f, 0.2f, 0.25f, 1f);
    [SerializeField] TMP_Text enemiesText;
    [SerializeField] TMP_Text levelNameText;

    void OnEnable()
    {
        RoomController.EnemiesLeftChanged += OnEnemiesLeftChanged;
        RoomController.ShardFound += OnShardFound;
        RiddleKeeper.Answered += OnRiddleAnswered;
    }

    void OnDisable()
    {
        RoomController.EnemiesLeftChanged -= OnEnemiesLeftChanged;
        RoomController.ShardFound -= OnShardFound;
        RiddleKeeper.Answered -= OnRiddleAnswered;
    }

    void Start()
    {
        playerHealth.Changed += RefreshTorches;
        GameManager.Instance.Inventory.Changed += RefreshInventory;

        levelNameText.text = GameManager.Instance.CurrentLevel.levelName;

        for (int i = 0; i < abilityIcons.Length; i++)
        {
            IAbility ability = playerCombat.GetAbility(i);
            if (ability != null)
            {
                abilityIcons[i].sprite = ability.Icon;
            }
        }

        RefreshTorches(playerHealth.Current, playerHealth.Max);
        RefreshInventory();
    }

    void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.Changed -= RefreshTorches;
        }

        if (GameManager.Instance != null && GameManager.Instance.Inventory != null)
        {
            GameManager.Instance.Inventory.Changed -= RefreshInventory;
        }
    }

    // Cooldowns change every frame, so they are read here, not by event.
    void Update()
    {
        for (int i = 0; i < abilityCooldownFills.Length; i++)
        {
            IAbility ability = playerCombat.GetAbility(i);
            if (ability == null)
            {
                continue;
            }

            abilityCooldownFills[i].fillAmount = ability.Cooldown > 0f ? ability.CooldownRemaining / ability.Cooldown : 0f;
        }
    }

    void RefreshTorches(int current, int max)
    {
        for (int i = 0; i < torches.Length; i++)
        {
            torches[i].gameObject.SetActive(i < max);
            torches[i].sprite = i < current ? fullTorch : emptyTorch;
        }
    }

    void RefreshInventory()
    {
        for (int i = 0; i < shardSlots.Length; i++)
        {
            shardSlots[i].color = GameManager.Instance.Inventory.HasShard(i) ? GameManager.Instance.GetLevel(i).color : emptyShardColor;
        }
    }

    void OnEnemiesLeftChanged(int count)
    {
        if (count == 0)
        {
            enemiesText.text = "Open the chest";
        }
        else
        {
            enemiesText.text = "Enemies left: " + count;
        }
    }

    void OnShardFound()
    {
        enemiesText.text = "Talk to the Riddle Keeper";
    }

    void OnRiddleAnswered(bool correct)
    {
        enemiesText.text = correct ? "Exit is open!" : "Wrong! Talk to the Riddle Keeper again";
    }
}
