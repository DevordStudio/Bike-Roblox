using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "TrailData",menuName = "ShopItem/TrailData")]
public class TrailData : ShopItemData
{
    public Material Material;
    //public bool HasSprite;
    //[ShowIf("HasSprite")] public Sprite Sprite;
    public int Speed;
}
