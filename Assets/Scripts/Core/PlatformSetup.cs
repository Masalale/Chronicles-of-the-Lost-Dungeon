using UnityEngine;

public class PlatformSetup : MonoBehaviour
{
    [SerializeField] GameObject[] mobileOnly;
    [SerializeField] GameObject[] desktopOnly;
    [SerializeField] GameObject[] hiddenOnWeb;
    [SerializeField] RectTransform[] leftSideHud;
    [SerializeField] RectTransform[] rightSideHud;
    [SerializeField] RectTransform siloText;
    [SerializeField] float roomWidth = 1.52f;
    [SerializeField] float mobileHudScale = 1.25f;

    void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        SetAll(mobileOnly, true);
        SetAll(desktopOnly, false);

        // Phones are wider than 16:9, so the dark bars beside the room get wide.
        // roomWidth is the room's width in screen heights; each bar centre sits
        // a quarter of the leftover width in from its edge.
        float barCentre = (1f - roomWidth * Screen.height / Screen.width) / 4f;
        CentreAll(leftSideHud, barCentre, mobileHudScale);
        CentreAll(rightSideHud, 1f - barCentre, mobileHudScale);
        if (siloText != null)
        {
            CentreAt(siloText, barCentre, 1f);
        }

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

    void CentreAll(RectTransform[] list, float x, float scale)
    {
        for (int i = 0; i < list.Length; i++)
        {
            CentreAt(list[i], x, scale);
        }
    }

    void CentreAt(RectTransform item, float x, float scale)
    {
        item.anchorMin = new Vector2(x, item.anchorMin.y);
        item.anchorMax = new Vector2(x, item.anchorMax.y);
        item.pivot = new Vector2(0.5f, item.pivot.y);
        // Scaling the gap from the top as well keeps the pause button clear of the bigger shard bar.
        item.anchoredPosition = new Vector2(0f, item.anchoredPosition.y * scale);
        item.localScale = Vector3.one * scale;
    }
}
