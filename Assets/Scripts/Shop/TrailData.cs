using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTrailData",menuName ="TrailData")]
public class TrailData : ScriptableObject
{
    public string Name = "Location";
    public int Price = 100;
    public int Id;
    public bool IsBought;
    public bool IsEquiped;
    public Material Material;
    public int Speed;
}
