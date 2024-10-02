using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BankVolute", menuName = "BankVolute", order = 53)]
public class BankVolute : ScriptableObject
{
    [SerializeField] private int _money;

    public bool Is2X;
    public int Money
    {
        get
        {
            return _money;
        }
        private set 
        {

        }
    }
    public static event Action<int> OnMoneyValueChanged;
    public void IncreaseMoney(int amount)
    {
        if (amount < 0) return;
        if (Is2X)
            _money += 2 * amount;
        else _money += amount;
        OnMoneyValueChanged?.Invoke(_money);
    }
    public void DecreaseMoney(int amount)
    {
        if(_money > amount)
            _money -= amount;
        OnMoneyValueChanged?.Invoke(_money);
    }
}
