using UnityEngine;

public class ItemInit : MonoBehaviour
{
    [SerializeField] private ShopItemData[] _items;

    private void Awake()
    {
        foreach (var item in _items)
            item.LoadItemData();
    }
}
