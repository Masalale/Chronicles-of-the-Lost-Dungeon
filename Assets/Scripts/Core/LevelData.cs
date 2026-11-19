using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Dungeon/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName;
    // Addressables: only loaded when this Silo starts, not with the menu.
    public AssetReferenceGameObject room;
    public AssetReferenceT<AudioClip> music;
    public Color color = Color.white;
}
