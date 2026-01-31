using UnityEngine;

[CreateAssetMenu(fileName = "Masks", menuName = "Scriptable Objects/Mask")]
public class Mask : ScriptableObject
{
    public Sprite img;
    public MaskMode mode;
}
