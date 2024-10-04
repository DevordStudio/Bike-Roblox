using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class CharacterShop : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _shopCamera;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _buttonBuy;
    [SerializeField] private GameObject _buttonEquip;
    [SerializeField] private GameObject _buttonEquiped;
    [SerializeField] private BankVolute _bank;

    private List<GameObject> _models = new List<GameObject>();
    private int _currentIndex;

    private void Start()
    {
        _characterController ??= FindAnyObjectByType<CharacterController>();
        GenerateModels();
        Button buy = _buttonBuy.GetComponent<Button>();
        Button equip = _buttonEquip.GetComponent<Button>();
        buy.onClick.AddListener(Buy);
        equip.onClick.AddListener(Equip);
        UpdateVisual();
    }
    public void GenerateModels()
    {
        foreach (var character in _characterController.Characters)
        {
            var model = Instantiate(character.CharacterModel, _spawnPoint.transform);
            model.name = character.Info.Id.ToString();
            _models.Add(model);
        }
    }
    public void OpenShop()
    {
        _mainCamera.gameObject.SetActive(false);
        _shopCamera.gameObject.SetActive(true);
    }
    public void CloseShop()
    {
        _shopCamera.gameObject.SetActive(false);
        _mainCamera.gameObject.SetActive(true);
    }
    public void Next()
    {
        if (_currentIndex != _characterController.Characters.Length - 1)
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
        else _currentIndex = _characterController.Characters.Length - 1;
        UpdateVisual();
    }
    public void UpdateVisual()
    {
        CharacterData character = _characterController.Characters[_currentIndex];
        foreach (var model in _models)
        {
            model.SetActive(character.Info.Id.ToString() == model.name);
        }
        _nameText.text = character.Info.Name;
        if (!character.Info.IsBought) //Если персонаж не куплен
        {
            _buttonBuy.SetActive(true);
            _buttonEquip.SetActive(false);
            _buttonEquiped.SetActive(false);
        }
        else if (character.Info.IsBought && !character.Info.IsEquiped)//Если куплен и не выбран
        {
            _buttonBuy.SetActive(false);
            _buttonEquip.SetActive(true);
            _buttonEquiped.SetActive(false);
        }
        else if (character.Info.IsBought && character.Info.IsEquiped)// Если выбран
        {
            _buttonBuy.SetActive(false);
            _buttonEquip.SetActive(false);
            _buttonEquiped.SetActive(true);
        }
    }
    public void Buy()
    {
        CharacterData character = _characterController.Characters[_currentIndex];
        if (!character.Info.IsBought && _bank.GetMoney() >= character.Info.Price && !character.Info.IsDonate)
        {
            _bank.DecreaseMoney(character.Info.Price);
            character.Info.Buy();
            UpdateVisual();
        }
        else if (character.Info.IsDonate)
        {
            YandexGame.BuyPayments(character.Info.Id.ToString());
        }
    }
    public void Equip()
    {
        CharacterData character = _characterController.Characters[_currentIndex];
        if (character.Info.IsBought && !character.Info.IsEquiped)
        {
            foreach (var items in _characterController.Characters)
            {
                items.Info.IsEquiped = false;
            }
            character.Info.Equip();
            UpdateVisual();
        }
    }
}
