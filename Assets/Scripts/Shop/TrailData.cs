using UnityEngine;

[CreateAssetMenu(fileName = "NewTrailData",menuName = "ShopItem/TrailData")]
public class TrailData : ScriptableObject
{
    public string Name = "Trail";
    public int Price = 100;
    public int Id;
    public bool IsBought;
    public bool IsEquiped;
    public Material Material;
    public int Speed;
}
