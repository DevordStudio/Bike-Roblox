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
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private GameObject _buttonBuy;
    [SerializeField] private GameObject _buttonEquip;
    [SerializeField] private GameObject _buttonEquiped;
    [SerializeField] private TMP_Text _bikeName;
    [SerializeField] private BikeController _bikeController;
    [SerializeField] private BankVolute _bank;

    private Camera _mainCamera;
    private int _currentIndex;

    private void Start()
    {
        UpdateVisual();
    }
    public void OpenShop()
    {
        _shopCamera.gameObject.SetActive(true);
        _mainCamera.gameObject.SetActive(false);
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
    }
    public void Previous()
    {
        if (_currentIndex != 0)
        {
            _currentIndex--;
        }
        else _currentIndex = _bikeController.Bikes.Length - 1;
        UpdateVisual();
    }
    public void UpdateVisual()
    {
        BikeData currentBike = _bikeController.Bikes[_currentIndex];
        currentBike.ChangeMaterials(_chasicsMR, _frontWheelMR, _backWheelMR);
        _bikeName.text = currentBike.Name;
        if (!currentBike.IsBought)//���� ��������� �� ������
        {
            _buttonBuy.SetActive(true);
            _buttonEquip.SetActive(false);
            _buttonEquiped.SetActive(false);
        }
        else if (currentBike.IsBought && !currentBike.IsEquiped)//���� ������, �� �� ������
        {
            _buttonBuy.SetActive(false);
            _buttonEquip.SetActive(true);
            _buttonEquiped.SetActive(false);
        }
        else if (currentBike.IsBought && currentBike.IsEquiped)//���� ������ � ������
        {
            _buttonBuy.SetActive(false);
            _buttonEquip.SetActive(false);
            _buttonEquiped.SetActive(true);
        }
    }
    public void Buy()
    {
        BikeData currentBike = _bikeController.Bikes[_currentIndex];
        if (_bank.Money >= currentBike.Price && !currentBike.IsDonate)
        {
            _bank.DecreaseMoney(currentBike.Price);
            currentBike.Buy();
            UpdateVisual();
        }
        else if (currentBike.IsDonate)
        {
            YandexGame.BuyPayments(currentBike.Id.ToString());
        }
        else Debug.Log($"������������ ����� ��� ������� ���������� {currentBike.Name}");//���������� �������������� ��� ������
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
    }
}
