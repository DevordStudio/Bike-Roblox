using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class BikeShop : MonoBehaviour
{
    [SerializeField] private MeshRenderer _chasicsMR;
    [SerializeField] private MeshRenderer _frontWheelMR;
    [SerializeField] private MeshRenderer _backWheelMR;
    [SerializeField] private Camera _shopCamera;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private GameObject _buttonBuy;
    [SerializeField] private GameObject _buttonEquip;
    [SerializeField] private GameObject _buttonEquiped;
    [SerializeField] private TMP_Text _bikeName;
    [SerializeField] private BikeController _bikeController;
    [SerializeField] private BankVolute _bank;
    [SerializeField] private ParticleSystem _particleSwitch;
    [SerializeField] private AudioClip _switchClip;
    [SerializeField] private AudioSource _effectSource;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Sprite _yanIcon;
    [SerializeField] private Image _coinIcon;

    private int _currentIndex;
    private Sprite _coinSprite;

    private void Start()
    {
        _bikeController ??= FindAnyObjectByType<BikeController>();
        YandexGame.SwitchLangEvent += UpdateText;
        _coinSprite = _coinIcon.sprite;
        InitButtons();
        UpdateVisual();
    }
    private void OnDisable()
    {
        YandexGame.SwitchLangEvent -= UpdateText;
    }
    private void PlaySwitchSound()
    {
        _particleSwitch.Play();
        _effectSource.clip = _switchClip;
        _effectSource.Play();
    }
    private void InitButtons()
    {
        _nextButton.onClick.AddListener(Next);
        _previousButton.onClick.AddListener(Previous);
        Button buy = _buttonBuy.GetComponent<Button>();
        Button equip = _buttonEquip.GetComponent<Button>();
        buy.onClick.AddListener(Buy);
        equip.onClick.AddListener(Equip);
    }
    public void OpenShop()
    {
        _shopCamera.gameObject.SetActive(true);
        _mainCamera.gameObject.SetActive(false);
        UpdateVisual();
        PlaySwitchSound();
    }
    public void CloseShop()
    {
        _shopCamera.gameObject.SetActive(false);
        _mainCamera.gameObject.SetActive(true);
    }
    public void Next()
    {
        if (_currentIndex != _bikeController.Bikes.Length - 1)
        {
            _currentIndex++;
        }
        else _currentIndex = 0;
        UpdateVisual();
        PlaySwitchSound();
    }
    public void Previous()
    {
        if (_currentIndex != 0)
        {
            _currentIndex--;
        }
        else _currentIndex = _bikeController.Bikes.Length - 1;
        UpdateVisual();
        PlaySwitchSound();
    }
    private void UpdateText(string lang)
    {
        BikeData currentBike = _bikeController.Bikes[_currentIndex];
        if (lang == "ru")
            _bikeName.text = currentBike.NameRus;
        else if (lang == "en")
            _bikeName.text = currentBike.NameEn;
        else if (lang == "tr")
            _bikeName.text = currentBike.NameTr;
        else _bikeName.text = currentBike.NameEn;
    }
    public void UpdateVisual()
    {
        BikeData currentBike = _bikeController.Bikes[_currentIndex];
        currentBike.ChangeMaterials(_chasicsMR, _frontWheelMR, _backWheelMR);

        //_bikeName.text = currentBike.Name;
        UpdateText(YandexGame.lang);
        if (!currentBike.IsBought)//Если велосипед не куплен
        {
            _buttonBuy.SetActive(true);
            _buttonEquip.SetActive(false);
            _buttonEquiped.SetActive(false);
            _priceText.text = currentBike.Price.ToString();
            _priceText.gameObject.SetActive(true);
            if (_priceText && !currentBike.IsDonate)
            {
                _coinIcon.sprite = _coinSprite;
                _bikeName.color = Color.white;
                if (_bank.GetMoney() >= currentBike.Price) _priceText.color = Color.white;
                else _priceText.color = Color.red;
            }
            else if (currentBike.IsDonate)
            {
                _coinIcon.sprite = _yanIcon;
                _bikeName.color = Color.yellow;
            }
        }
        else if (currentBike.IsBought && !currentBike.IsEquiped)//Если куплен, но не выбран
        {
            _buttonBuy.SetActive(false);
            _buttonEquip.SetActive(true);
            _buttonEquiped.SetActive(false);
            _priceText.gameObject.SetActive(false);
        }
        else if (currentBike.IsBought && currentBike.IsEquiped)//Если куплен и выбран
        {
            _buttonBuy.SetActive(false);
            _buttonEquip.SetActive(false);
            _buttonEquiped.SetActive(true);
            _priceText.gameObject.SetActive(false);
        }
    }
    public void Buy()
    {
        BikeData currentBike = _bikeController.Bikes[_currentIndex];
        if (_bank.GetMoney() >= currentBike.Price && !currentBike.IsDonate)
        {
            _bank.DecreaseMoney(currentBike.Price);
            currentBike.Buy();
        }
        else if (currentBike.IsDonate)
        {
            YandexGame.BuyPayments(currentBike.Id.ToString());
        }
        else Debug.Log($"Недостаточно денег для покупки велосипеда {currentBike.Name}");//Прикрутить предупреждение для игрока
        UpdateVisual();
    }
    public void Equip()
    {
        BikeData currentBike = _bikeController.Bikes[_currentIndex];
        if (currentBike.IsBought && !currentBike.IsEquiped)
        {
            foreach (var items in _bikeController.Bikes)
            {
                items.IsEquiped = false;
            }
            currentBike.IsEquiped = true;
            _bikeController.ChangeBike(currentBike.Id);
        }
        UpdateVisual();
    }
}
