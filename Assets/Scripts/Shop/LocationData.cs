using UnityEngine;

public class LocationData : ShopItemData
{
    public GameObject Location;

    public override void Use()
    {
        if (IsBought)
        {
            LocationController.Instance.LoadLocation(Id);
            Debug.Log($"��������� ������� � �������� {Id}");
        }
        else Debug.LogError("������� �� �������! ����� ������ ������� ����� �� ��� ������");
    }
}
