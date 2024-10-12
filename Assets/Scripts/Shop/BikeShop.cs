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

    private int _currentIndex;

    private void Start()
    {
        _bikeController ??= FindAnyObjectByType<BikeController>();
        InitButtons();
        UpdateVisual();
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
    public void UpdateVisual()
    {
        BikeData currentBike = _bikeController.Bikes[_currentIndex];
        currentBike.ChangeMaterials(_chasicsMR, _frontWheelMR, _backWheelMR);
        _bikeName.text = currentBike.Name;
        if (!currentBike.IsBought)//Если велосипед не куплен
        {
            _buttonBuy.SetActive(true);
            _buttonEquip.SetActive(false);
            _buttonEquiped.SetActive(false);
        }
        else if (currentBike.IsBought && !currentBike.IsEquiped)//Если куплен, но не выбран
        {
            _buttonBuy.SetActive(false);
            _buttonEquip.SetActive(true);
            _buttonEquiped.SetActive(false);
        }
        else if (currentBike.IsBought && currentBike.IsEquiped)//Если куплен и выбран
        {
            _buttonBuy.SetActive(false);
            _buttonEquip.SetActive(false);
            _buttonEquiped.SetActive(true);
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
