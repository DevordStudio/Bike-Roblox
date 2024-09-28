using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BankVolute",menuName ="BankVolute",order =53)]
public class BankVolute : ScriptableObject
{
    [SerializeField] private int _money;

    public int Money
    {
        get
        {
            return _money;
        }
        set
        {
            if (value >= 0)
            {
                _money = value;
                OnMoneyValueChanged?.Invoke(_money);
            }
            else throw new Exception("Ошибка! Число денег не может быть отрицательным");
        }
    }

    public static BankVolute Instance { get; private set; }

    public static event Action<int> OnMoneyValueChanged;

    private void Awake()
    {
        Instance = this;
    }
}
