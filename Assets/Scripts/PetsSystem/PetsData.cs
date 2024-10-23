using System;
using UnityEngine;

[CreateAssetMenu(fileName ="PetData",menuName ="PetData")]
public class PetsData : ShopItemData
{
    public GameObject PetModel;

    public Sprite Sprite;
    public int Reward;

    public static event Action<PetsData> OnPetDropped;
    public void Drop()
    {
        OnPetDropped.Invoke(this);
        //Buy();
    }
}
