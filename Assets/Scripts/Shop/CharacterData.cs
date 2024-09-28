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
            IsEquiped = true;
            Debug.Log($"���������� ���� ��� ��������� {Name}");
        }
        else Debug.LogError("�������� �� ������");
    }
}
