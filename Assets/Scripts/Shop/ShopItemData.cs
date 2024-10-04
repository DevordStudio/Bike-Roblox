using UnityEngine;
using YG;

public class ShopItemData : ScriptableObject
{
    public string Name = "Item";
    public int Price = 100;
    public int Id;
    public bool IsBought;
    public bool IsEquiped;
    public bool IsDonate;
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
            Debug.Log($"Предмет {Name} был куплен за {Price} монет");
        }
        else Debug.LogError("Предмет уже куплен или не хватает денег!");
    }
}
