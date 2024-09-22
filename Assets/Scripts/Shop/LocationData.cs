using UnityEngine;

public class LocationData : ShopItemData
{
    public GameObject Location;

    public override void Use()
    {
        if (IsBought)
        {
            LocationController.Instance.LoadLocation(Id);
            Debug.Log($"Загружена локация с индексом {Id}");
        }
        else Debug.LogError("Локация не куплена! Нужно купить локацию чтобы на ней играть");
    }
}
