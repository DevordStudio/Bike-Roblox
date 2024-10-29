using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LocationsShopController : MonoBehaviour
{
    [SerializeField] private LocationController _locationController;
    [SerializeField] private Image _locationImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private GameObject _buttonBuy;
    [SerializeField] private GameObject _buttonPlay;
    [SerializeField] private GameObject _buttonThisLocation;
    [SerializeField] private BankVolute _bank;
    [SerializeField] private TMP_Text _priceText;
    private int _currentIndex = 0;

    private void Start()
    {
        _locationController ??= FindAnyObjectByType<LocationController>();
        YandexGame.SwitchLangEvent += UpdateName;
        InitButtons();
        UpdateUI();
    }
    private void OnDisable()
    {
        YandexGame.SwitchLangEvent -= UpdateName;
    }
    private void InitButtons()
    {
        _nextButton.onClick.AddListener(Next);
        _previousButton.onClick.AddListener(Previous);
        Button buttonBuy = _buttonBuy.GetComponent<Button>();
        Button buttonPlay = _buttonPlay.GetComponent<Button>();
        buttonBuy.onClick.AddListener(Buy);
        buttonPlay.onClick.AddListener(Play);
    }
    public void Next()
    {
        if (_currentIndex != _locationController.Locations.Length - 1)
        {
            _currentIndex++;
        }
        else _currentIndex = 0;
        UpdateUI();
    }
    public void Previous()
    {
        if (_currentIndex != 0)
        {
            _currentIndex--;
        }
        else _currentIndex = _locationController.Locations.Length - 1;
        UpdateUI();
    }
    private void UpdateName(string lang)
    {
        LocationData currentLocation = _locationController.Locations[_currentIndex];
        if (lang == "ru")
            _nameText.text = currentLocation.LocationInfo.NameRus;
        else if (lang == "en")
            _nameText.text = currentLocation.LocationInfo.NameEn;
        else if (lang == "tr")
            _nameText.text = currentLocation.LocationInfo.NameTr;
    }
    public void UpdateUI()
    {
        LocationData currentLocation = _locationController.Locations[_currentIndex];
        UpdateName(YandexGame.lang);
        if (currentLocation)
        {
            _locationImage.sprite = currentLocation.LocationInfo.Sprite;
            //_nameText.text = currentLocation.LocationInfo.Name;
            if (!currentLocation.LocationInfo.IsBought) //Если карта не куплена
            {
                _buttonBuy.SetActive(true);
                _buttonPlay.SetActive(false);
                _buttonThisLocation.SetActive(false);
                if (_priceText && !currentLocation.LocationInfo.IsDonate)
                {
                    _priceText.gameObject.SetActive(true);
                    _priceText.text = currentLocation.LocationInfo.Price.ToString();
                    if (_bank.GetMoney() >= currentLocation.LocationInfo.Price) _priceText.color = Color.white;
                    else _priceText.color = Color.red;
                }
                else _priceText.gameObject.SetActive(false);
            }
            else if (currentLocation.LocationInfo.IsBought && !currentLocation.LocationInfo.IsEquiped)// Если куплена и не является текущей
            {
                _buttonBuy.SetActive(false);
                _buttonPlay.SetActive(true);
                _buttonThisLocation.SetActive(false);
                _priceText.gameObject.SetActive(false);
            }
            else if (currentLocation.LocationInfo.IsBought && currentLocation.LocationInfo.IsEquiped)
            {
                _buttonBuy.SetActive(false);
                _buttonPlay.SetActive(false);
                _buttonThisLocation.SetActive(true);
                _priceText.gameObject.SetActive(false);
            }
        }
    }
    public void Buy()
    {
        LocationData loc = _locationController.Locations[_currentIndex];
        if (!loc.LocationInfo.IsBought && _bank.GetMoney() >= loc.LocationInfo.Price && !loc.LocationInfo.IsDonate)
        {
            _bank.DecreaseMoney(loc.LocationInfo.Price);
            loc.LocationInfo.Buy();
            UpdateUI();
        }
        else if (loc.LocationInfo.IsDonate)
        {
            YandexGame.BuyPayments(loc.LocationInfo.Id.ToString());
        }
    }
    public void Play()
    {
        LocationData loc = _locationController.Locations[_currentIndex];
        if (!loc.LocationInfo.IsEquiped)
            loc.LocationInfo.Equip();
    }
}
