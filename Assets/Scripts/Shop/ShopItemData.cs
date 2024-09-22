using UnityEngine;

public abstract class ShopItemData : MonoBehaviour
{
    public string Name = "Location";
    public int Price = 100;
    public int Id;
    public bool IsBought;
    public bool IsEquiped;
    public Sprite Sprite;

    public virtual void Buy()
    {
        if (!IsBought && BankVolute.Instance.Money >= Price)
        {
            BankVolute.Instance.Money -= Price;
            IsBought = true;
            Debug.Log($"Предмет {Name} был куплен за {Price} монет");
        }
        else Debug.LogError("Предмет уже куплен или не хватает денег!");
    }
    public abstract void Use();
}
