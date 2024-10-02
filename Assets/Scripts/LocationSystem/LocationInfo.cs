using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationInfo", menuName = "ShopItem/LocationInfo")]
public class LocationInfo : ScriptableObject
{
    public string Name = "Location";
    public int Price = 100;
    public int Id;
    public bool IsBought;
    public bool IsEquiped;
    public Sprite Sprite;

    public static event Action<int> OnLocationChanged;
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
        if (IsBought)
        {
            OnLocationChanged?.Invoke(Id);
            IsEquiped = true;
            Debug.Log($"��������� ������� � �������� {Id}");
        }
        else Debug.LogError("������� �� �������! ����� ������ ������� ����� �� ��� ������");
    }
}
