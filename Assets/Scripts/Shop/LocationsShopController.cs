using TMPro;
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
    private int _currentIndex = 0;

    private void Start()
    {
        _nextButton.onClick.AddListener(Next);
        _previousButton.onClick.AddListener(Previous);
        Button buttonBuy = _buttonBuy.GetComponent<Button>();
        Button buttonPlay = _buttonPlay.GetComponent<Button>();
        buttonBuy.onClick.AddListener(Buy);
        buttonPlay.onClick.AddListener(Play);
        UpdateUI();
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
    public void UpdateUI()
    {
        LocationData currentLocation = _locationController.Locations[_currentIndex];
        if (currentLocation)
        {
            _locationImage.sprite = currentLocation.LocationInfo.Sprite;
            _nameText.text = currentLocation.LocationInfo.Name;
            if (!currentLocation.LocationInfo.IsBought) //Если карта не куплена
            {
                _buttonBuy.SetActive(true);
                _buttonPlay.SetActive(false);
                _buttonThisLocation.SetActive(false);
            }
            else if (currentLocation.LocationInfo.IsBought && !currentLocation.LocationInfo.IsEquiped)// Если куплена и не является текущей
            {
                _buttonBuy.SetActive(false);
                _buttonPlay.SetActive(true);
                _buttonThisLocation.SetActive(false);
            }
            else if (currentLocation.LocationInfo.IsBought && currentLocation.LocationInfo.IsEquiped)
            {
                _buttonBuy.SetActive(false);
                _buttonPlay.SetActive(false);
                _buttonThisLocation.SetActive(true);
            }
        }
    }
    public void Buy()
    {
        LocationData loc = _locationController.Locations[_currentIndex];
        if (!loc.LocationInfo.IsBought && _bank.Money >= loc.LocationInfo.Price && !loc.LocationInfo.IsDonate)
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
