using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class ShopItemData : ScriptableObject
{
    public string Name = "Item";
    public int Price = 100;
    public int Id;
    public bool IsBought;
    [SerializeField] private bool _isEquiped;
    public bool IsEquiped
    {
        get
        {
            return _isEquiped;
        }
        set
        {
            _isEquiped = value;
            if (value == true) OnItemEquiped?.Invoke();
            SaveItemData();
        }
    }
    public string NameRus;
    public string NameEn;
    public string NameTr;
    public bool IsDonate;

    public static event Action OnItemBought;
    public static event Action OnItemEquiped;
    private void OnEnable()
    {
        if (IsDonate)
            YandexGame.PurchaseSuccessEvent += Purchase;
    }
    private void OnDisable()
    {
        if (IsDonate)
            YandexGame.PurchaseSuccessEvent -= Purchase;
    }

    private void Purchase(string Id)
    {
        if (Id == this.Id.ToString())
            Buy();
    }

    public virtual void Buy()
    {
        if (!IsBought)
        {
            IsBought = true;
            OnItemBought?.Invoke();
            Debug.Log($"������� {Name} ��� ������ �� {Price} �����");
            SaveItemData();
        }
        else
        {
            Debug.LogError("������� ��� ������ ��� �� ������� �����!");
        }
    }

    public void SaveItemData()
    {
        List<ShopItemSaveData> savedItems = YandexGame.savesData.shopItemsData;

        if (savedItems == null)
        {
            Debug.LogError("savedItems is null!");
            savedItems = new List<ShopItemSaveData>();
        }

        ShopItemSaveData existingData = savedItems.Find(item => item.Id == Id);
        if (existingData != null)
        {
            existingData.IsBought = IsBought;
            existingData.IsEquiped = IsEquiped;
            Debug.Log($"������ � {Name} ���� ��������� � ��������������� ������");
        }
        else
        {
            ShopItemSaveData newItemData = new ShopItemSaveData
            {
                Id = Id,
                IsBought = IsBought,
                IsEquiped = IsEquiped
            };
            savedItems.Add(newItemData);
            Debug.Log($"���������� ��� {Name} ���� �������, ������ ��� ��������������� ������ �� ���� �������");
        }

        YandexGame.savesData.shopItemsData = savedItems;
        YandexGame.SaveProgress();

        Debug.Log($"������� {Name} �������.");
    }
    [Button]
    public void LoadItemData()
    {
        List<ShopItemSaveData> savedItems = YandexGame.savesData.shopItemsData;

        ShopItemSaveData savedData = savedItems.Find(item => item.Id == Id);
        if (savedData != null)
        {
            IsBought = savedData.IsBought;
            IsEquiped = savedData.IsEquiped;
            Debug.Log($"��������� ������ ��� {Name}: ������ - {IsBought}, ���������� - {IsEquiped}");
        }
        else
        {
            Debug.Log($"������ ��� {Name} �� �������.");
        }
    }
}

[Serializable]
public class ShopItemSaveData
{
    public int Id;
    public bool IsBought;
    public bool IsEquiped;
}