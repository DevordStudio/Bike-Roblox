using UnityEngine;
using YG;

public class MoneyRecord : MonoBehaviour
{
    [SerializeField] BankVolute _bank;

    private int _lastMoneyRecord;

    private void Start()
    {
        _lastMoneyRecord = YandexGame.savesData.MoneyRecord;
        BankVolute.OnMoneyIncrease += Record;
    }
    private void OnDisable() => BankVolute.OnMoneyIncrease -= Record;
    private void Record()
    {
        int currentMoney = _bank.GetMoney();
        if (currentMoney > _lastMoneyRecord)
        {
            _lastMoneyRecord = currentMoney;
            YandexGame.savesData.MoneyRecord = _lastMoneyRecord;
            YandexGame.NewLeaderboardScores("MoneyLB", _lastMoneyRecord);
            Debug.Log("<color=yellow>Новый рекорд записан</color>");
        }
    }
}
