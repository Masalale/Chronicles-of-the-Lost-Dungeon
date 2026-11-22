using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip clickClip;
    [SerializeField] AudioClip enemyDeathClip;
    [SerializeField] AudioClip pickupClip;
    [SerializeField] AudioClip levelCompleteClip;
    [SerializeField] AudioClip gameOverClip;

    public static AudioManager Instance;

    void Awake()
    {
        // Coming back to the menu scene makes a new copy; keep the first.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 1f);
        sfxSource.volume = PlayerPrefs.GetFloat("sfxVolume", 1f);

        Enemy.AnyDied += OnEnemyDied;
        Shard.AnyCollected += OnShardCollected;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LevelCompleted += OnLevelCompleted;
            GameManager.Instance.GameOver += OnGameOver;
        }
    }

    void OnDestroy()
    {
        Enemy.AnyDied -= OnEnemyDied;
        Shard.AnyCollected -= OnShardCollected;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LevelCompleted -= OnLevelCompleted;
            GameManager.Instance.GameOver -= OnGameOver;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        // Same song already playing: don't restart it from the top.
        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return;
        }

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySfx(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    public void PlayClick()
    {
        PlaySfx(clickClip);
    }

    // Save straight away; closing a browser tab won't save PlayerPrefs.
    public void SetMusicVolume(float v)
    {
        musicSource.volume = v;
        PlayerPrefs.SetFloat("musicVolume", v);
        PlayerPrefs.Save();
    }

    public void SetSfxVolume(float v)
    {
        sfxSource.volume = v;
        PlayerPrefs.SetFloat("sfxVolume", v);
        PlayerPrefs.Save();
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    void OnEnemyDied(Enemy enemy)
    {
        PlaySfx(enemyDeathClip);
    }

    void OnShardCollected()
    {
        PlaySfx(pickupClip);
    }

    void OnLevelCompleted()
    {
        PlaySfx(levelCompleteClip);
    }

    void OnGameOver()
    {
        PlaySfx(gameOverClip);
    }
}
