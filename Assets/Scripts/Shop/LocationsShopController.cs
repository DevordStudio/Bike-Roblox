using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocationsShopController : MonoBehaviour
{
    [SerializeField] private Image _locationImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private GameObject _buttonBuy;
    [SerializeField] private GameObject _buttonPlay;
    [SerializeField] private GameObject _buttonThisLocation;
    private int _currentIndex = 0;

    private void Start()
    {
        _nextButton.onClick.AddListener(Next);
        Button buttonBuy = _buttonBuy.GetComponent<Button>();
        Button buttonPlay = _buttonPlay.GetComponent<Button>();
        buttonBuy.onClick.AddListener(Buy);
        buttonBuy.onClick.AddListener(Play);
        UpdateUI();
    }
    public void Next()
    {
        if (_currentIndex != LocationController.Instance.Locations.Length - 1)
        {
            _currentIndex++;
            UpdateUI();
        }
        else _currentIndex = 0;
    }
    public void Previous()
    {
        if (_currentIndex != 0)
        {
            _currentIndex--;
            UpdateUI();
        }
        else _currentIndex = LocationController.Instance.Locations.Length - 1;
    }
    public void UpdateUI()
    {
        LocationData currentLocation = LocationController.Instance.Locations[_currentIndex];
        _locationImage.sprite = currentLocation.Sprite;
        _nameText.text = currentLocation.Sprite.name;
        if (!currentLocation.IsBought) //Если карта не куплена
        {
            _buttonBuy.SetActive(true);
            _buttonPlay.SetActive(false);
            _buttonThisLocation.SetActive(false);
        }
        else if (currentLocation.IsBought && LocationController.Instance.ActiveLocationId != currentLocation.Id)// Если куплена и не является текущей
        {
            _buttonBuy.SetActive(false);
            _buttonPlay.SetActive(true);
            _buttonThisLocation.SetActive(false);
        }
        else if (currentLocation.IsBought && LocationController.Instance.ActiveLocationId != currentLocation.Id)
        {
            _buttonBuy.SetActive(false);
            _buttonPlay.SetActive(false);
            _buttonThisLocation.SetActive(true);
        }
        else throw new Exception("Невозможно запустить локацию, так как она ещё не куплена");
    }
    public void Buy()
    {
        LocationData loc = LocationController.Instance.Locations[_currentIndex];
        if (!loc.IsBought)
        {
            loc.Buy();
            UpdateUI();
        }
        else
            throw new Exception("Location is already bought");
    }
    public void Play()
    {
        LocationData loc = LocationController.Instance.Locations[_currentIndex];
        if (LocationController.Instance.ActiveLocationId != loc.Id)
            loc.Use();
    }
}
