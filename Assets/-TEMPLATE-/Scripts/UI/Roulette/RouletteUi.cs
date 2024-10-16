using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class RouletteUi : MonoBehaviour
{
    [SerializeField] private Roulette _roulette;
    [SerializeField] private string _prefixTimerInPanel = "Бесплатная крутка ";
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _countKeyText;
    [SerializeField] private TextMeshProUGUI _countLastAdsText;
    [SerializeField] private TextMeshProUGUI _timerInPanelText;
    [SerializeField] private Button _startRouletteButton;
    [SerializeField] private Button _viewAdsForKeyButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private GameObject _roulettePanel;
    [SerializeField] private PurchaseForKeysRoulette[] _purchaseForKey;
    private int _neededAmountAdsForKey = 3;
    private int _tempNeededAmountAdsForKey;
    private bool _freeSpin = false;

    private void Start()
    {
        _startRouletteButton.onClick.AddListener(RotateRoulette);
        _viewAdsForKeyButton.onClick.AddListener(ViewAdsForKey);

        _roulette.EndRotateEvent += EndRotate;

        foreach (var item in _purchaseForKey)
        {
            item.BuyButton.onClick.AddListener(() => YandexGame.BuyPayments(item.Id));
        }

        YandexGame.PurchaseSuccessEvent += HandlerPurchaseForKey;
        YandexGame.RewardVideoEvent += HandlerAdsForKey;

        _tempNeededAmountAdsForKey = _neededAmountAdsForKey;

        //AddCountKeys(0);
        //ChangeTextCountNeededAdsForKey(_tempNeededAmountAdsForKey);

        //if (YandexGame.savesData.TempKdRotateRoulette > 0)
            //Utils.Timer.Instance.CreateCheckTime(YandexGame.savesData.TempKdRotateRoulette, UnlockFreeSpin, ChangeTime);
        //else
        //{
        //    ChangeTime(0);
        //    UnlockFreeSpin();
        //}
    }
    private void OnDestroy()
    {
        YandexGame.PurchaseSuccessEvent -= HandlerPurchaseForKey;
        YandexGame.RewardVideoEvent -= HandlerAdsForKey;

        _roulette.EndRotateEvent -= EndRotate;
    }

    private void ViewAdsForKey()
    {
        YandexGame.RewVideoShow(1);
    }

    private void HandlerAdsForKey(int idAds)
    {
        if (idAds == 1)
        {
            _tempNeededAmountAdsForKey--;

            if (_tempNeededAmountAdsForKey <= 0)
            {
                _tempNeededAmountAdsForKey = _neededAmountAdsForKey;

                AddCountKeys(1);
            }

            ChangeTextCountNeededAdsForKey(_tempNeededAmountAdsForKey);
        }
        
    }

    private void HandlerPurchaseForKey(string id)
    {
        int length = _purchaseForKey.Length;
        for (int i = 0; i < length; i++)
        {
            if (_purchaseForKey[i].Id == id)
            {
                AddCountKeys(_purchaseForKey[i].CountKeyForPurchase);
                break;
            }
        }
    }

    private void UnlockFreeSpin()
    {
        _freeSpin = true;
    }

    private void RotateRoulette()
    {
        if (_freeSpin == true)
        {
            _freeSpin = false;
            _closeButton.interactable = false;
            _roulette.Rotate();

            //Utils.Timer.Instance.CreateCheckTime(YandexGame.savesData.KdRotateRoulette, UnlockFreeSpin, ChangeTime);
        }
        //else if (YandexGame.savesData.CountKeyRoulette > 0)
        //{
        //    if (_roulette.Rotate() == true)
        //    {
        //        _closeButton.interactable = false;
        //        AddCountKeys(-1);
        //    }
        //}
    }

    private void AddCountKeys(int newCount)
    {
        if (newCount != 0)
        {

            YandexGame.SaveProgress();
        }

        //_countKeyText.text = YandexGame.savesData.CountKeyRoulette.ToString();
    }
    private void ChangeTextCountNeededAdsForKey(int newCount)
    {
        _countLastAdsText.text = $"X{newCount}";
    }

    private void EndRotate()
    {
        _closeButton.interactable = true;
    }

    private void ChangeTime(float time)
    {
        //YandexGame.savesData.TempKdRotateRoulette = time > 0 ? time : 0;
        
        //_timerText.text = time.ToTime();
        //_timerInPanelText.text = time > 0 ? _prefixTimerInPanel + time.ToTime() : "Доступна бесплатная крутка";
    }

    private void OnApplicationQuit()
    {
        YandexGame.SaveProgress();
    }
}

[System.Serializable]
public class PurchaseForKeysRoulette
{
    public string Id = "5YGfor1Key";
    public int CountKeyForPurchase = 1;
    public Button BuyButton;
}