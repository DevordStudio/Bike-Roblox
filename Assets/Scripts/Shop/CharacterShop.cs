using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterShop : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _shopCamera;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private GameObject _buttonBuy;
    [SerializeField] private GameObject _buttonEquip;
    [SerializeField] private GameObject _buttonEquiped;

    private int _currentIndex = 0;
    private List<GameObject> _models;

    private void Start()
    {
        GenerateModels();
        UpdateVisual();
    }
    public void GenerateModels()
    {
        foreach(var character in CharacterController.Instance.Characters)
        {
            var model = Instantiate(character.CharacterModel, _spawnPoint.transform);
            model.name = character.Id.ToString();
            _models.Add(model);
        }
    }
    public void Next()
    {
        if (_currentIndex != CharacterController.Instance.Characters.Length - 1)
        {
            _currentIndex++;
            UpdateVisual();
        }
        else _currentIndex = 0;
    }
    public void Previous()
    {
        if (_currentIndex != 0)
        {
            _currentIndex--;
            UpdateVisual();
        }
        else _currentIndex = CharacterController.Instance.Characters.Length - 1;
    }
    public void UpdateVisual()
    {
        CharacterData character = CharacterController.Instance.Characters[_currentIndex];
        foreach (var model in _models)
        {
            model.SetActive(character.Id.ToString() == model.name);
        }
        _nameText.text = character.Name;
        if (!character.IsBought) //Если персонаж не куплен
        {
            _buttonBuy.SetActive(true);
            _buttonEquip.SetActive(false);
            _buttonEquiped.SetActive(false);
        }
        else if (character.IsBought && CharacterController.Instance.ActiveCharacterId != character.Id)//Если куплен и не выбран
        {
            _buttonBuy.SetActive(false);
            _buttonEquip.SetActive(true);
            _buttonEquiped.SetActive(false);
        }
        else if (character.IsBought && CharacterController.Instance.ActiveCharacterId != character.Id)// Если выбран
        {
            _buttonBuy.SetActive(false);
            _buttonEquip.SetActive(false);
            _buttonEquiped.SetActive(true);
        }
    }
    public void Buy()
    {
        CharacterData character = CharacterController.Instance.Characters[_currentIndex];
        if (!character.IsBought)
        {
            character.Buy();
            UpdateVisual();
        }
        else
            throw new Exception("Location is already bought");
    }
    public void Equip()
    {
        CharacterData character = CharacterController.Instance.Characters[_currentIndex];
        if(character.IsBought && !character.IsEquiped)
        {
            character.Use();
            UpdateVisual();
        }
    }
}
