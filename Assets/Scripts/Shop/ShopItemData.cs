using System;
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
        }
    }
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
    void Purchase(string Id)
    {
        if (Id == this.Id.ToString()) Buy();
    }
    public virtual void Buy()
    {
        if (!IsBought)
        {
            IsBought = true;
            OnItemBought?.Invoke();
            Debug.Log($"������� {Name} ��� ������ �� {Price} �����");
        }
        else Debug.LogError("������� ��� ������ ��� �� ������� �����!");
    }
}
