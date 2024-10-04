using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfo", menuName = "ShopItem/CharacterInfo")]
public class CharacterInfo : ShopItemData
{
    public static event Action<int> OnCharacterChanged;

    public void Equip()
    {
        if (IsBought && !IsEquiped)
        {
            OnCharacterChanged?.Invoke(Id);
            IsEquiped = true;
            Debug.Log($"Установлен скин под названием {Name}");
        }
        else Debug.LogError("Персонаж не куплен");
    }
}
