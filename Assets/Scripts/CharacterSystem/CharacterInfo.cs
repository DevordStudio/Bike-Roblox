using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfo", menuName = "ShopItem/CharacterInfo")]
public class CharacterInfo : ScriptableObject
{
    public string Name = "Location";
    public int Price = 100;
    public int Id;
    public bool IsBought;
    public bool IsEquiped;

    public static event Action<int> OnCharacterChanged;

    public void Buy()
    {
        if (!IsBought)
        {
            IsBought = true;
            Debug.Log($"Предмет {Name} был куплен за {Price} монет");
        }
        else Debug.LogError("Предмет уже куплен или не хватает денег!");
    }
    public void Use()
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
