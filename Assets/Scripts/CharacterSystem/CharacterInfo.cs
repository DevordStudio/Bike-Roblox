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
            Debug.Log($"������� {Name} ��� ������ �� {Price} �����");
        }
        else Debug.LogError("������� ��� ������ ��� �� ������� �����!");
    }
    public void Use()
    {
        if (IsBought && !IsEquiped)
        {
            OnCharacterChanged?.Invoke(Id);
            IsEquiped = true;
            Debug.Log($"���������� ���� ��� ��������� {Name}");
        }
        else Debug.LogError("�������� �� ������");
    }
}
