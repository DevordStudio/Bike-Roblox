using UnityEngine;
using YG;

public class VoluteSaver : MonoBehaviour
{
    [SerializeField] private BankVolute _bank;

    private void Start()
    {
        BankVolute.OnMoneyValueChanged += SaveVolute;
        LoadVolute();
    }
    private void OnDisable()
    {
        BankVolute.OnMoneyValueChanged -= SaveVolute;
    }
    void SaveVolute()
    {
        YandexGame.savesData.Money = _bank.GetMoney();
        Debug.Log("Монеты сохранены");
        YandexGame.SaveProgress();
    }
    void LoadVolute()
    {
        _bank.SetVolute(YandexGame.savesData.Money);
        Debug.Log("Монеты загружены");
    }
}
