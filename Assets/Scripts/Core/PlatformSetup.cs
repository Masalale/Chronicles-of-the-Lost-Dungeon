using UnityEngine;

public class PlatformSetup : MonoBehaviour
{
    [SerializeField] GameObject[] mobileOnly;
    [SerializeField] GameObject[] desktopOnly;
    [SerializeField] GameObject[] hiddenOnWeb;

    void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        SetAll(mobileOnly, true);
        SetAll(desktopOnly, false);

        // Android caps at 30 fps unless told otherwise.
        Application.targetFrameRate = 60;
#else
        SetAll(mobileOnly, false);
        SetAll(desktopOnly, true);
#endif

#if UNITY_WEBGL
        // Quit can't close a browser tab, so the Exit button is hidden.
        SetAll(hiddenOnWeb, false);
#endif
    }

    void SetAll(GameObject[] list, bool active)
    {
        for (int i = 0; i < list.Length; i++)
        {
            list[i].SetActive(active);
        }
    }
}
