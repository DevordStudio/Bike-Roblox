using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationData : MonoBehaviour, IPurchasable
{
    [SerializeField] private int _price = 100;
    [SerializeField] private string _name = "Location";
    public GameObject Location;
    public int Id;
    public bool IsBought;
    public bool IsEquiped;
    public void Buy()
    {
        if (!IsBought && BankVolute.Instance.Money >= _price)
        {
            BankVolute.Instance.Money -= _price;
            IsBought = true;
            Debug.Log($"Предмет {_name} был куплен за {_price} монет");
        }
        else Debug.LogError("Предмет уже куплен или не хватает денег!");
    }
    public void Use()
    {
        if (!IsEquiped && IsBought)
        {
            IsEquiped = true;
            Debug.Log($"Предмет {_name} был экипирован");
        }
        else Debug.LogError("Предмет уже экипирован или ещё не куплен!");
    }
}
