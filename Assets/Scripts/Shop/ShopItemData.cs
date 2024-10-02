using UnityEngine;

//reateAssetMenu(fileName = "LocationData", menuName = "LocationData")]
public class ShopItemData : ScriptableObject
{
    [SerializeField] private BankVolute _bank;

    public string Name = "Location";
    public int Price = 100;
    public int Id;
    public bool IsBought;
    public bool IsEquiped;
    public Sprite Sprite;

    public virtual void Buy()
    {
        if (!IsBought && _bank.Money >= Price)
        {
            _bank.DecreaseMoney(Price);
            IsBought = true;
            Debug.Log($"������� {Name} ��� ������ �� {Price} �����");
        }
        else Debug.LogError("������� ��� ������ ��� �� ������� �����!");
    }
    public virtual void Use()
    {
        Debug.Log("������� �����������");
    }
}
