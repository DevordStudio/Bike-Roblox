using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationInfo", menuName = "ShopItem/LocationInfo")]
public class LocationInfo : ShopItemData
{
    public Sprite Sprite;
    public Material Skybox;

    public static event Action<int> OnLocationChanged;
    
    public void Equip()
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
