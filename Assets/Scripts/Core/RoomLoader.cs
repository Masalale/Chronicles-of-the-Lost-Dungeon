using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RoomLoader : MonoBehaviour
{
    public event System.Action LoadFinished;
    public event System.Action<string> LoadFailed;

    AsyncOperationHandle<GameObject> roomHandle;
    AsyncOperationHandle<AudioClip> musicHandle;

    void Start()
    {
        LevelData level = GameManager.Instance.CurrentLevel;

        roomHandle = level.room.InstantiateAsync(transform);
        roomHandle.Completed += OnRoomLoaded;

        if (level.music.RuntimeKeyIsValid())
        {
            musicHandle = level.music.LoadAssetAsync();
            musicHandle.Completed += OnMusicLoaded;
        }
    }

    void OnDestroy()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }

        if (roomHandle.IsValid())
        {
            // Addressables instances need releasing, not Destroy.
            Addressables.ReleaseInstance(roomHandle);
        }

        if (musicHandle.IsValid())
        {
            Addressables.Release(musicHandle);
        }
    }

    void OnRoomLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            LoadFinished?.Invoke();
        }
        else
        {
            LoadFailed?.Invoke("The room could not be loaded.");
        }
    }

    void OnMusicLoaded(AsyncOperationHandle<AudioClip> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(handle.Result);
        }
    }
}
