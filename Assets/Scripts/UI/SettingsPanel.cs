using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    void OnEnable()
    {
        // WithoutNotify, or opening the panel would call OnMusicChanged.
        musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("musicVolume", 1f));
        sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("sfxVolume", 1f));
    }

    public void OnMusicChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    public void OnSfxChanged(float value)
    {
        AudioManager.Instance.SetSfxVolume(value);
    }
}
