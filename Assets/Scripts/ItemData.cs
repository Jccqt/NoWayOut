using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Horror System/Item")]
public class ItemData : ScriptableObject
{
    public string id;          
    public string displayName;  
    public Sprite icon; 
    [TextArea] public string description;
}
