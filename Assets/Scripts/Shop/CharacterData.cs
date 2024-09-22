using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterData : ShopItemData
{
    public GameObject Character;
    public GameObject CharacterModel;

    public override void Use()
    {
        if (IsBought && Character && !IsEquiped)
        {
            CharacterController.Instance.ChangeCharacter(Id);
            Debug.Log($"Установлен скин под названием {Name}");
        }
        else Debug.LogError("Персонаж не куплен");
    }
}
